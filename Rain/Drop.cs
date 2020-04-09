using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;


namespace Rain
{
    class Drop
    {
        private int width;
        private int height;
        public Color DropColor = Color.LightSkyBlue;
        public int X { get; private set; }
        public int Y { get; private set; }
        public int DropD { get; private set; }
        public static int dx { get; set; }
        private static Random rand = null;
        private int dy;
        private Thread t = null;
        private bool stop = false;

        public bool IsAlive
        {
            get { return t != null && t.IsAlive; }
        }
        public Drop(Rectangle r)
        {
            Update(r);
            if (rand == null) rand = new Random();
            DropD = rand.Next(1, 6);
            X = rand.Next(0, width);
            Y = 0;
            dy = height/40;
        }

        public void Update(Rectangle r)
        {
            width = r.Width;
            height = r.Height;
            dy = height / 40;
        }

        private void Move()
        {
            while (!stop && Y < height)
            {
                Thread.Sleep(30);
                X += dx;
                Y += dy;
                if (X >= width) X = X%width;
                if (X <= 0) X = width+X;
            }
        }

        public void Start()
        {
            if (t == null || !t.IsAlive)
            {
                stop = false;
                ThreadStart th = new ThreadStart(Move);
                t = new Thread(th);
                t.Start();
            }
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
