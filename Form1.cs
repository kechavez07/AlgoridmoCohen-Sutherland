using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoridmoCohen_Sutherland
{
    public partial class Form1 : Form
    {
        // Variables para la línea
        private List<Point> puntos = new List<Point>();

        // Ventana de recorte (puedes cambiar estos valores)
        private Rectangle rectRecorte = new Rectangle(100, 80, 300, 200); // x, y, ancho, alto


        public Form1()
        {
            InitializeComponent();
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
        }
        // Códigos de región (outcode)
        const int INSIDE = 0; // 0000
        const int LEFT = 1;   // 0001
        const int RIGHT = 2;  // 0010
        const int BOTTOM = 4; // 0100
        const int TOP = 8;    // 1000

        // Calcular código de región
        private int CalcularCodigo(Point p)
        {
            int code = INSIDE;

            if (p.X < rectRecorte.Left)
                code |= LEFT;
            else if (p.X > rectRecorte.Right)
                code |= RIGHT;

            if (p.Y < rectRecorte.Top)
                code |= TOP;
            else if (p.Y > rectRecorte.Bottom)
                code |= BOTTOM;

            return code;
        }
        private bool CohenSutherland(Point p1, Point p2, out Point rec1, out Point rec2)
        {
            int xMin = rectRecorte.Left;
            int xMax = rectRecorte.Right;
            int yMin = rectRecorte.Top;
            int yMax = rectRecorte.Bottom;

            int code1 = CalcularCodigo(p1);
            int code2 = CalcularCodigo(p2);

            bool accept = false;

            rec1 = p1;
            rec2 = p2;

            while (true)
            {
                if ((code1 | code2) == 0)
                {
                    // Ambos dentro: Aceptar
                    accept = true;
                    break;
                }
                else if ((code1 & code2) != 0)
                {
                    // Ambos fuera y en la misma región: Rechazar
                    break;
                }
                else
                {
                    // Alguno afuera: recortar
                    int codeOut = code1 != 0 ? code1 : code2;
                    int x = 0, y = 0;

                    if ((codeOut & TOP) != 0)
                    {
                        x = p1.X + (p2.X - p1.X) * (yMin - p1.Y) / (p2.Y - p1.Y);
                        y = yMin;
                    }
                    else if ((codeOut & BOTTOM) != 0)
                    {
                        x = p1.X + (p2.X - p1.X) * (yMax - p1.Y) / (p2.Y - p1.Y);
                        y = yMax;
                    }
                    else if ((codeOut & RIGHT) != 0)
                    {
                        y = p1.Y + (p2.Y - p1.Y) * (xMax - p1.X) / (p2.X - p1.X);
                        x = xMax;
                    }
                    else if ((codeOut & LEFT) != 0)
                    {
                        y = p1.Y + (p2.Y - p1.Y) * (xMin - p1.X) / (p2.X - p1.X);
                        x = xMin;
                    }

                    if (codeOut == code1)
                    {
                        p1 = new Point(x, y);
                        code1 = CalcularCodigo(p1);
                        rec1 = p1;
                    }
                    else
                    {
                        p2 = new Point(x, y);
                        code2 = CalcularCodigo(p2);
                        rec2 = p2;
                    }
                }
            }
            return accept;
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Dibujar área de recorte (ventana)
            e.Graphics.DrawRectangle(Pens.Blue, rectRecorte);

            // Dibujar línea original
            if (puntos.Count == 2)
            {
                e.Graphics.DrawLine(Pens.Gray, puntos[0], puntos[1]);

                // Recorte
                if (CohenSutherland(puntos[0], puntos[1], out Point r1, out Point r2))
                {
                    e.Graphics.DrawLine(new Pen(Color.Red, 2), r1, r2); // Línea recortada en rojo
                }
            }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (puntos.Count == 2)
                puntos.Clear();

            puntos.Add(e.Location);
            pictureBox1.Invalidate();
        }
    }
}
