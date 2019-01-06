using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.WinForms
{
    public partial class Form1 : Form
    {
        private string teapotPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName + @"\Files\teapot.off";
        private string mushroomPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName + @"\Files\mushroom_triang.off";
        private string cubePath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName + @"\Files\cube.off";

        private List<Vertice> vertices;
        private List<Face> faces;
        private List<(int, int)> verticesIndLines;

        private List<PointF> points;

        private Vertice cameraPosition = new Vertice(0, 0, 4); //3D position of camera (point that is representing the camera)
        private Vertice cameraOrientation = new Vertice(0, 0, 0); //camera orientation
        private Vertice surfacePosition = new Vertice(50, 50, 50); //display surface's position relative to the camera

        private float[,] zLevel;

        public Form1()
        {
            InitializeComponent();

            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.raster_MouseWheel);

            //loadFile(mushroomPath, "mushroom_triang.off");
            //loadFile(teapotPath, "teapot.off");
            loadFile(cubePath, "cube.off");
        }

        private void CalculatePoints()
        {
            points = new List<PointF>();
            Vertice cos = new Vertice { X = Math.Cos(cameraOrientation.X), Y = Math.Cos(cameraOrientation.Y), Z = Math.Cos(cameraOrientation.Z) };
            Vertice sin = new Vertice { X = Math.Sin(cameraOrientation.X), Y = Math.Sin(cameraOrientation.Y), Z = Math.Sin(cameraOrientation.Z) };
            foreach (Vertice v in vertices)
            {
                Vertice cv = new Vertice { X = v.X - cameraPosition.X, Y = v.Y - cameraPosition.Y, Z = v.Z - cameraPosition.Z };
                Vertice d = new Vertice
                {
                    X = cos.Y * (sin.Z * cv.Y + cos.Z * cv.X) - sin.Y * cv.Z,
                    Y = sin.X * (cos.Y * cv.Z + sin.Y * (sin.Z * cv.Y + cos.Z * cv.X)) + cos.X * (cos.Z * cv.Y - sin.Z * cv.X),
                    Z = cos.X * (cos.Y * cv.Z + sin.Y * (sin.Z * cv.Y + cos.Z * cv.X)) - sin.X * (cos.Z * cv.Y - sin.Z * cv.X)
                };
                PointF p = new PointF
                {
                    X = (float)((surfacePosition.Z / d.Z) * d.X + surfacePosition.X),
                    Y = (float)((surfacePosition.Z / d.Z) * d.Y + surfacePosition.Y)
                };
                points.Add(p);
            }

        }

        private void Draw()
        {
            Bitmap b = new Bitmap(raster.Width, raster.Height);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);
            if (drawFacesCheckBox.Checked)
            {
                foreach (Face f in faces) //draw faces (solid color)
                {
                    PointF[] facePoints = new PointF[f.Count];
                    for (int i = 0; i < f.Count; i++)
                    {
                        facePoints[i] = points[f.VerticesInd[i]];
                    }
                    float[] facePointsZ = new float[f.Count];
                    for (int i = 0; i < f.Count; i++)
                    {
                        facePointsZ[i] = (float)vertices[f.VerticesInd[i]].Z;
                    }
                    f.Draw(ref b, facePoints, ref zLevel, facePointsZ);
                }
            }
            if (drawPointsCheckBox.Checked)
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
            vertices = new List<Vertice>();
            verticesIndLines = new List<(int, int)>();

            StreamReader sr = new StreamReader(filePath);
            sr.ReadLine();
            int[] nValues = (sr.ReadLine()).Split(' ').Select(x => int.Parse(x)).ToArray();
            for (int i = 0; i < nValues[0]; i++) //reading vertices
            {
                double[] tVertice = (sr.ReadLine()).Replace('.', ',').Split(' ').Select(x => double.Parse(x)).ToArray();
                vertices.Add(new Vertice(tVertice[0], tVertice[1], tVertice[2]));
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

            refreshScreen();
        }

        private void refreshScreen()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            zLevel = new float[raster.Width, raster.Height];
            for (int i = 0; i < raster.Width; i++)
            {
                for (int j = 0; j < raster.Height; j++)
                {
                    zLevel[i, j] = float.MinValue;
                }
            }
            CalculatePoints();
            Draw();
            stopWatch.Stop();
            FPSLabel.Text = $"FPS: {(int)(1.0 / (stopWatch.ElapsedMilliseconds / 1000.0))}";
        }

        private void raster_MouseWheel(object sender, MouseEventArgs e)
        {
            surfacePosition.Z += (float)e.Delta / 40;
            refreshScreen();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.W when e.Modifiers == Keys.None:
                    surfacePosition.Y--;
                    break;
                case Keys.S when e.Modifiers == Keys.None:
                    surfacePosition.Y++;
                    break;
                case Keys.A when e.Modifiers == Keys.None:
                    surfacePosition.X--;
                    break;
                case Keys.D when e.Modifiers == Keys.None:
                    surfacePosition.X++;
                    break;
                case Keys.E when e.Modifiers == Keys.None:
                    surfacePosition.Z++;
                    break;
                case Keys.Q when e.Modifiers == Keys.None:
                    surfacePosition.Z--;
                    break;
                case Keys.Z:
                case Keys.D when e.Modifiers == Keys.Shift:
                    cameraOrientation.X += 0.01;
                    break;
                case Keys.X:
                case Keys.A when e.Modifiers == Keys.Shift:
                    cameraOrientation.X -= 0.01;
                    break;
                case Keys.C:
                case Keys.S when e.Modifiers == Keys.Shift:
                    cameraOrientation.Y += 0.01;
                    break;
                case Keys.V:
                case Keys.W when e.Modifiers == Keys.Shift:
                    cameraOrientation.Y -= 0.01;
                    break;
                case Keys.B:
                case Keys.E when e.Modifiers == Keys.Shift:
                    cameraOrientation.Z += 0.01;
                    break;
                case Keys.N:
                case Keys.Q when e.Modifiers == Keys.Shift:
                    cameraOrientation.Z -= 0.01;
                    break;
                case Keys.R:
                case Keys.D when e.Modifiers == Keys.Control:
                    cameraPosition.X += 0.01;
                    break;
                case Keys.T:
                case Keys.A when e.Modifiers == Keys.Control:
                    cameraPosition.X -= 0.01;
                    break;
                case Keys.Y:
                case Keys.S when e.Modifiers == Keys.Control:
                    cameraPosition.Y += 0.01;
                    break;
                case Keys.U:
                case Keys.W when e.Modifiers == Keys.Control:
                    cameraPosition.Y -= 0.01;
                    break;
                case Keys.I:
                case Keys.E when e.Modifiers == Keys.Control:
                    cameraPosition.Z += 0.01;
                    break;
                case Keys.O:
                case Keys.Q when e.Modifiers == Keys.Control:
                    cameraPosition.Z -= 0.01;
                    break;
            }
            refreshScreen();
        }

        private void refreshScreenEvent(object sender, EventArgs e)
        {
            refreshScreen();
        }
    }
}