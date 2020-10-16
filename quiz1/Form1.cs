using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quiz1
{
    public partial class Form1 : Form
    {
        Form2 frm2 = new Form2();
        public Form1()
        {
            InitializeComponent();
            this.BackColor = core.data.get_color();
            button5.Enabled = false;
        }
        public int circle = 0;
        public double gameTime = 0;
        public bool gameEnd = false;
        public List<Point> circleList = new List<Point>();
        public List<Point> allcircleList = new List<Point>();
        public Random rd = new Random();
        

        public void init()
        {
            circle = 0;
            gameTime = 0;
            gameEnd = false;
            circleList.Clear();
            allcircleList.Clear();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gameTime += 1;
            label1.Text = "time : " + (gameTime / 10).ToString() + " s";
            if (gameTime >= 100)
            {
                gameEnd = true;
                timer1.Stop();
                button1.Enabled = true;
            }
            if (gameTime % 5 == 0)
            {
                int cx = rd.Next(pictureBox1.Width);
                int cy = rd.Next(pictureBox1.Height);
                Point cc = new Point(cx + 10, cy + 10);
                circleList.Add(cc);
                allcircleList.Add(cc);
                label2.Text = "circle : " + circleList.Count;
                frm2.set_text(circleList.Count.ToString());
            }
            Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            button1.Enabled = false;
            if (gameEnd == true) init();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameEnd == false)
            {
                foreach (Point i in allcircleList)
                {
                    double distX = Math.Abs(e.X - i.X);
                    double distY = Math.Abs(e.Y - i.Y);
                    if (Math.Sqrt(distX * distX + distY * distY) <= 20)
                    {
                        circleList.Remove(i);
                    }
                }
                label2.Text = "circle : " + circleList.Count;
                frm2.set_text(circleList.Count.ToString());
            }
            else return;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush br = new SolidBrush(Color.Yellow);
            foreach (Point i in circleList)
            {
                g.FillEllipse(br, i.X, i.Y, 20, 20);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frm2.Show();
            button2.Enabled = false;
            button5.Enabled = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            frm2.Hide();
            button2.Enabled = true;
            button5.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            frm2.myparent = this;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                timer1.Start();
                return;
            }
            StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
            foreach (Point i in circleList)
            {
                sw.WriteLine(i.X.ToString() + "," + i.Y.ToString());
            }
            sw.Close();
            timer1.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            timer1.Stop();
            init();
            String line;
            String[] coord = new String[2];
            StreamReader sr = new StreamReader(openFileDialog1.FileName);
            while ((line = sr.ReadLine()) != null)
            {
                coord = line.Split(',');
                int cx = Convert.ToInt32(coord[0]);
                int cy = Convert.ToInt32(coord[1]);
                Point cc = new Point(cx, cy);
                circleList.Add(cc);
                allcircleList.Add(cc);
            }
            sr.Close();
            Refresh();
        }
    }
}
