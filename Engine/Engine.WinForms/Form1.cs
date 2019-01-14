using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Engine.WinForms
{
    public partial class Form1 : Form
    {
        private string teapotPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName + @"\Files\teapot.off";
        private string mushroomPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName + @"\Files\mushroom_triang.off";
        private string cubePath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName + @"\Files\cube.off";

        private List<Vector3> vertices;
        private List<Face> faces;
        private List<(int, int)> verticesIndLines;

        private Point[] points;

        private Vector3 cameraPosition;
        private Vector3 cameraTarget;
        private Vector3 upVector;

        private Vector3 objectRotation;

        private float nearPlaneDistance = 1;
        private float farPlaneDistance = 100;
        private float fieldOfView = 45;
        private float aspectRatio = 1;

        private float[,] zLevel;
        private float[] pointsZLevel;

        private int fps;

        private Bitmap texture;

        private Color fogColor = Color.Gray;

        public Form1()
        {
            InitializeComponent();

            texture = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(texture);
            g.Clear(Color.Black);
            textureView.Image = texture;
            Bitmap fogColorBitmap = new Bitmap(50, 50);
            g = Graphics.FromImage(fogColorBitmap);
            g.Clear(fogColor);
            fogColorView.Image = fogColorBitmap;

            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.raster_MouseWheel);

            //loadFile(mushroomPath, "mushroom_triang.off");
            //loadFile(teapotPath, "teapot.off");
            loadFile(cubePath, "cube.off");
        }

        private void CalculatePoints()
        {
            zLevel = new float[raster.Width, raster.Height];
            for (int i = 0; i < raster.Width; i++)
            {
                for (int j = 0; j < raster.Height; j++)
                    zLevel[i, j] = float.MaxValue;
            }
            aspectRatio = (float)raster.Width / raster.Height;
            pointsZLevel = new float[vertices.Count];
            points = new Point[vertices.Count];
            Matrix4x4 LookAt = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, upVector);
            Matrix4x4 Perspective = Matrix4x4.CreatePerspectiveFieldOfView(fieldOfView * 0.0174532925f, aspectRatio, nearPlaneDistance, farPlaneDistance);
            Matrix4x4 RotateX = Matrix4x4.CreateRotationX(objectRotation.X * 0.0174532925f);
            Matrix4x4 RotateY = Matrix4x4.CreateRotationY(objectRotation.Y * 0.0174532925f);
            Matrix4x4 RotateZ = Matrix4x4.CreateRotationZ(objectRotation.Z * 0.0174532925f);
            Matrix4x4 Rotate = RotateZ * RotateY * RotateX;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector4 vr = MultiplyVector(Rotate, new Vector4(vertices[i], 1));
                Vector4 vlook = MultiplyVector(LookAt, vr);
                Vector4 v = MultiplyVector(Perspective, vlook);
                v = v / v.W;

                Point p = new Point
                {
                    X = (int)(v.X * raster.Width + raster.Width / 2),
                    Y = (int)(v.Y * raster.Height + raster.Height / 2)
                };
                pointsZLevel[i] = v.Z * (farPlaneDistance - nearPlaneDistance);
                points[i] = p;
            }
        }

        private Vector4 MultiplyVector(Matrix4x4 m, Vector4 v)
        {
            float x = m.M11 * v.X + m.M21 * v.Y + m.M31 * v.Z + m.M41 * v.W;
            float y = m.M12 * v.X + m.M22 * v.Y + m.M32 * v.Z + m.M42 * v.W;
            float z = m.M13 * v.X + m.M23 * v.Y + m.M33 * v.Z + m.M43 * v.W;
            float w = m.M14 * v.X + m.M24 * v.Y + m.M34 * v.Z + m.M44 * v.W;
            return new Vector4(x, y, z, w);
        }

        private void Draw()
        {
            Bitmap b = new Bitmap(raster.Width, raster.Height);
            Graphics g = Graphics.FromImage(b);
            if (drawFacesCheckBox.Checked && drawFogCheckBox.Checked)
                g.Clear(fogColor);
            else
                g.Clear(Color.White);
            if (drawFacesCheckBox.Checked)
            {
                foreach (Face f in faces) //draw faces
                {
                    Point[] facePoints = new Point[f.Count];
                    for (int i = 0; i < f.Count; i++)
                    {
                        facePoints[i] = points[f.VerticesInd[i]];
                    }
                    float[] facePointsZ = new float[f.Count];
                    for (int i = 0; i < f.Count; i++)
                    {
                        facePointsZ[i] = pointsZLevel[f.VerticesInd[i]];
                    }
                    if (backfaceCullingCheckBox.Checked)
                    {
                        Vector3 v12 = new Vector3(facePoints[1].X - facePoints[0].X, facePoints[1].Y - facePoints[0].Y, facePointsZ[1] - facePointsZ[0]);
                        Vector3 v13 = new Vector3(facePoints[f.Count - 1].X - facePoints[0].X, facePoints[f.Count - 1].Y - facePoints[0].Y, facePointsZ[f.Count - 1] - facePointsZ[0]);
                        Vector3 N = Vector3.Cross(v12, v13);
                        Vector3 S = new Vector3(cameraPosition.X - facePoints[0].X, cameraPosition.Y - facePoints[0].Y, cameraPosition.Z - facePointsZ[0]);

                        float dotProduct = N.X * S.X + N.Y * S.Y + N.Z * S.Z;
                        if (dotProduct < 0)
                            continue;
                    }
                    if (textureFaces.Checked)
                        f.Draw(ref b, facePoints, ref zLevel, facePointsZ, zBufforCheckBox.Checked, texture);
                    else
                        f.Draw(ref b, facePoints, ref zLevel, facePointsZ, zBufforCheckBox.Checked);
                }

                if (drawFogCheckBox.Checked)
                {
                    for (int i = 0; i < b.Width; i++)
                    {
                        for (int j = 0; j < b.Height; j++)
                        {
                            if (zLevel[i, j] > farPlaneDistance || zLevel[i, j] < nearPlaneDistance)
                            {
                                continue;
                            }
                            float f = (farPlaneDistance - Math.Abs(zLevel[i, j])) / (farPlaneDistance - nearPlaneDistance);
                            Color oldColor = b.GetPixel(i, j);
                            int nRed = (int)(f * fogColor.R + (1 - f) * oldColor.R);
                            int nGreen = (int)(f * fogColor.G + (1 - f) * oldColor.G);
                            int nBlue = (int)(f * fogColor.B + (1 - f) * oldColor.B);
                            Color nColor = Color.FromArgb(nRed, nGreen, nBlue);
                            b.SetPixel(i, j, nColor);
                        }
                    }
                }
            }
            if (drawPointsCheckBox.Checked && !drawEdgesCheckBox.Checked)
            {
                foreach (PointF p in points) //draw points
                {
                    if (p.X >= 0 && p.X < b.Width && p.Y >= 0 && p.Y < b.Height)
                        b.SetPixel((int)p.X, (int)p.Y, Color.Black);
                }
            }
            if (drawEdgesCheckBox.Checked)
            {
                foreach ((int, int) vl in verticesIndLines) //draw all edges
                {
                    new Line(points[vl.Item1], points[vl.Item2], (float)vertices[vl.Item1].Z, (float)vertices[vl.Item2].Z).Draw(ref b);
                }
            }
            raster.Image = b;
            raster.Refresh();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "OFF Files|*.off";
            openFileDialog1.Title = "Select OFF File";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                loadFile(openFileDialog1.FileName, openFileDialog1.SafeFileName);
            }
        }

        private void loadFile(string filePath, string fileName)
        {
            faces = new List<Face>();
            vertices = new List<Vector3>();
            verticesIndLines = new List<(int, int)>();

            StreamReader sr = new StreamReader(filePath);
            sr.ReadLine();
            int[] nValues = (sr.ReadLine()).Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int i = 0; i < nValues[0]; i++) //reading vertices
            {
                float[] tVertice = (sr.ReadLine()).Replace('.', ',').Split(' ').Select(x => float.Parse(x)).ToArray();
                vertices.Add(new Vector3(tVertice[0], tVertice[1], tVertice[2]));
            }
            Random rnd = new Random();
            for (int i = 0; i < nValues[1]; i++) //reading faces
            {
                int[] tFace = (sr.ReadLine()).Replace("  ", " ").Split(' ').Select(x => int.Parse(x)).ToArray();
                int[] tFaceVertices = new int[tFace[0]];
                for (int k = 0; k < tFace[0]; k++)
                    tFaceVertices[k] = tFace[k + 1];
                for (int k = 0; k < tFace[0] - 1; k++) //storing lines based on vertices indexes
                {
                    if (!verticesIndLines.Contains((tFaceVertices[k], tFaceVertices[k + 1])) && !verticesIndLines.Contains((tFaceVertices[k + 1], tFaceVertices[k])))
                        verticesIndLines.Add((tFaceVertices[k], tFaceVertices[k + 1]));
                }
                if (!verticesIndLines.Contains((tFaceVertices[0], tFaceVertices[tFace[0] - 1])) && !verticesIndLines.Contains((tFaceVertices[tFace[0] - 1], tFaceVertices[0])))
                    verticesIndLines.Add((tFaceVertices[0], tFaceVertices[tFace[0] - 1]));
                faces.Add(new Face(tFace[0], tFaceVertices, Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))));
            }
            sr.Close();
            loadLabel.Text = $"Loaded {fileName}\n{vertices.Count} vertices\n{faces.Count} faces\n{verticesIndLines.Count} lines";

            cameraPosition = new Vector3(10, 0.5f, 0.5f);
            cameraTarget = new Vector3(0, 0.5f, 0.5f);
            objectRotation = new Vector3(0, 0, 0);
            upVector = new Vector3(0, 0, 1);

            drawFacesCheckBox.Checked = false;
            drawEdgesCheckBox.Checked = false;
            drawPointsCheckBox.Checked = true;
            refreshScreen();
        }

        private void refreshScreen()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CalculatePoints();
            Draw();
            stopWatch.Stop();
            fps = (int)(1.0 / (stopWatch.ElapsedMilliseconds / 1000.0));
            updateText();
        }

        private void updateText()
        {
            FPSLabel.Text = $"FPS: {fps}";
            rotationLabel.Text = $"Rotation x: {objectRotation.X % 360}°\nRotation y: {objectRotation.Y % 360}°\nRotation z: {objectRotation.Z % 360}°";
        }

        private void raster_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
                cameraPosition.X -= (float)e.Delta / 1000;
            else
                cameraPosition.X -= (float)e.Delta / 100;
            refreshScreen();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W when e.Modifiers == Keys.None:
                    cameraTarget.Z -= 0.05f;
                    break;
                case Keys.S when e.Modifiers == Keys.None:
                    cameraTarget.Z += 0.05f;
                    break;
                case Keys.A when e.Modifiers == Keys.None:
                    cameraTarget.Y -= 0.05f;
                    break;
                case Keys.D when e.Modifiers == Keys.None:
                    cameraTarget.Y += 0.05f;
                    break;
                case Keys.E when e.Modifiers == Keys.None:
                    cameraPosition.X += 0.1f;
                    break;
                case Keys.Q when e.Modifiers == Keys.None:
                    cameraPosition.X -= 0.1f;
                    break;
                case Keys.Z:
                case Keys.D when e.Modifiers == Keys.Shift:
                    objectRotation.Z += 1f;
                    break;
                case Keys.X:
                case Keys.A when e.Modifiers == Keys.Shift:
                    objectRotation.Z -= 1f;
                    break;
                case Keys.C:
                case Keys.S when e.Modifiers == Keys.Shift:
                    objectRotation.Y += 1f;
                    break;
                case Keys.V:
                case Keys.W when e.Modifiers == Keys.Shift:
                    objectRotation.Y -= 1f;
                    break;
                case Keys.B:
                case Keys.E when e.Modifiers == Keys.Shift:
                    objectRotation.X += 1f;
                    break;
                case Keys.N:
                case Keys.Q when e.Modifiers == Keys.Shift:
                    objectRotation.X -= 1f;
                    break;
            }
            refreshScreen();
        }

        private void refreshScreenEvent(object sender, EventArgs e)
        {
            refreshScreen();
        }

        private void loadTextureButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Load texture";
            openFileDialog1.Filter = "JPG Files|*.jpg|PNG files|*.png*|BMP files|*.bmp*|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                texture = new Bitmap(openFileDialog1.OpenFile());
                textureView.Image = texture;
                textureView.Refresh();
                refreshScreen();
            }
        }

        private void fogColorView_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                fogColor = colorDialog1.Color;
                Graphics g = Graphics.FromImage(fogColorView.Image);
                g.Clear(fogColor);
                fogColorView.Refresh();
                refreshScreen();
            }
        }
    }
}