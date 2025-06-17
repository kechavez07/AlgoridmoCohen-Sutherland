using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoridmoCohen_Sutherland
{
    public partial class Form1 : Form
    {
        private List<Point> puntos = new List<Point>();
        private Bitmap canvas;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Crear el canvas solo una vez al iniciar
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = canvas;
            DibujarCuadricula();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Limitar a dos puntos para la línea
            if (puntos.Count == 3)
                puntos.Clear();

            puntos.Add(e.Location);
            Dibujarvector();
        }

        private void Dibujarvector()
        {
            using (Graphics g = Graphics.FromImage(canvas))
            {
                // 2) Dibujar puntos seleccionados
                DibujarPuntos(g);

                // 3) Dibujar línea entre los puntos (si hay dos)
                DibujarLinea(g);
            }

            // Refrescar PictureBox
            pictureBox1.Image = canvas;
        }

        //private void DibujarTodo()
        //{
        //    using (Graphics g = Graphics.FromImage(canvas))
        //    {
        //        g.Clear(Color.White);

        //        // 1) Dibujar cuadrícula
        //        DibujarCuadricula(g);

        //        // 2) Dibujar puntos seleccionados
        //        DibujarPuntos(g);

        //        // 3) Dibujar línea entre los puntos (si hay dos)
        //        DibujarLinea(g);
        //    }

        //    // Refrescar PictureBox
        //    pictureBox1.Image = canvas;
        //}

        private void DibujarCuadricula()
        {
            using (Graphics g = Graphics.FromImage(canvas))
            { 
                int width = pictureBox1.Width;
                int height = pictureBox1.Height;
                int cellWidth = width / 3;
                int cellHeight = height / 3;

                using (Pen gridPen = new Pen(Color.Black, 2))
                {
                    // Líneas verticales internas
                    g.DrawLine(gridPen, cellWidth, 0, cellWidth, height);
                    g.DrawLine(gridPen, 2 * cellWidth, 0, 2 * cellWidth, height);
                    // Líneas horizontales internas
                    g.DrawLine(gridPen, 0, cellHeight, width, cellHeight);
                    g.DrawLine(gridPen, 0, 2 * cellHeight, width, 2 * cellHeight);
                }
            
            }
        }

        private void DibujarPuntos(Graphics g)
        {
            foreach (var punto in puntos)
            {
                g.FillEllipse(Brushes.Red, punto.X - 2, punto.Y - 2, 4, 4);
            }
        }

        private void DibujarLinea(Graphics g)
        {
            if (puntos.Count == 3)
            {
                using (Pen linePen = new Pen(Color.Blue, 2))
                {
                    g.DrawLine(linePen, puntos[1], puntos[2]);
                }
            }
        }
    }
}
