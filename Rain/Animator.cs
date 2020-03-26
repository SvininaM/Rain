using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Rain
{
    class Animator
    {
        private Graphics mainG;
        private int width, height;
        private List<Drop> drops = new List<Drop>();
        private Thread t;
        private bool stop = false;
        private BufferedGraphics bg;
        public Animator(Graphics g, Rectangle r)
        {
            Update(g, r);
        }

        public void Update(Graphics g, Rectangle r)
        {
            mainG = g;
            width = r.Width;
            height = r.Height;
            bg = BufferedGraphicsManager.Current.Allocate(
                mainG,
                new Rectangle(0, 0, width, height)
            );
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Update(r);
            }
            Monitor.Exit(drops);
        }

        private void Animate()
        {
            while (!stop)
            {
                Graphics g = bg.Graphics;
                g.Clear(Color.White);
                Monitor.Enter(drops);
                int cnt = drops.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (!drops[i].IsAlive) drops.Remove(drops[i]);
                    i--;
                    cnt--;
                }
                foreach (var d in drops)
                {
                    Brush br = new SolidBrush(d.DropColor);
                    Pen p = new Pen(d.DropColor, 2);
                    var path = new GraphicsPath();
                    path.StartFigure();
                    path.AddLine(d.X, d.Y, (float)(d.X + 1.5), d.Y + 6);
                    path.AddArc((float)(d.X - 1.5), d.Y + 6, 3, 3, 0, 180);
                    path.AddLine((float)(d.X - 1.5), d.Y + 6, d.X, d.Y);
                    path.CloseFigure();
                    if (Drop.dx !=0)
                    {
                        Matrix myMatrix = new Matrix();
                        myMatrix.RotateAt(-Drop.dx * 7, new Point(d.X, d.Y));
                        path.Transform(myMatrix);
                    }
                    g.FillPath(br, path);
                    g.DrawPath(p, path);
                }
                Monitor.Exit(drops);
                try
                {
                    bg.Render();
                }
                catch (Exception e) { }
                Thread.Sleep(30);
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Animate);
                t = new Thread(th);
                t.Start();
            }
            var rect = new Rectangle(0, 0, width, height);
            Drop d = new Drop(rect);
            d.Start();
            Monitor.Enter(drops);
            drops.Add(d);
            Monitor.Exit(drops);
        }

        public void Stop()
        {
            stop = true;
            Monitor.Enter(drops);
            foreach (var d in drops)
            {
                d.Stop();
            }
            drops.Clear();
            Monitor.Exit(drops);
        }
        public void SetValue (int val)
        {
            Drop.dx = val;
        }
    }
}
