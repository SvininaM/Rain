using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rain
{
    public partial class Form1 : Form
    {
        private Animator a;
        public Form1()
        {
            InitializeComponent();
            a = new Animator(mainPanel.CreateGraphics(), mainPanel.ClientRectangle);
        }

        private void mainPanel_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;          
        }

        private void mainPanel_Resize(object sender, EventArgs e)
        {
            a.Update(mainPanel.CreateGraphics(),
                     mainPanel.ClientRectangle
                    );
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            a.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                a.Start();
            }
            else a.Stop();

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            a.SetValue(trackBar1.Value);
        }

    }
}
