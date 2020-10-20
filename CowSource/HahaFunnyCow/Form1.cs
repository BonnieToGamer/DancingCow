using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;

/*
    Hello There, What are you doing?
    Looking at my source code huh?
    Well... Sice you're already here,
    Take a look at my sphagetti ass code.
*/

namespace HahaFunnyCow
{
    public partial class Form1 : Form
    {

        // The boundry of the screen
        int boundryX { get; set; }
        int boundryY { get; set; }

        // The direction of the cow
        int vx { get; set; }
        int vy { get; set; }

        // The speed of the cow
        List<int> choice = new List<int> { -5, 5 };
        
        // Used to make a random direction when created
        Random r = new Random();
        int indexX { get; set; }
        int indexY { get; set; }

        // Initialize the Form
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            // Fixes a weird bug where cow bounces back and forth repeatedly
            Application.DoEvents();

            // Gets the screen dimensions
            boundryX = this.Size.Width;
            boundryY = this.Size.Height;
            // Label causes a weird bug if it's not present (exact same bug asApplication.DoEvents() fixes)
            label1.Hide();

            // General settings for the players
            WMP.settings.autoStart = true;
            Rick.settings.autoStart = true;
            Rick.Hide();

            // Makes the random start direction
            indexX = r.Next(choice.Count);
            indexY = r.Next(choice.Count);
            vx = choice[indexX];
            vy = choice[indexY];
        }

        // Starts the cow player
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            // Makes a Temporary file that starts playing then right after starting gets deleted
            string strTempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Cow.mp4");
            File.WriteAllBytes(strTempFile, Properties.Resources.dancing_cow);

            WMP.URL = strTempFile;
            WMP.settings.setMode("loop", true);

            Thread.Sleep(10);
            File.Delete(strTempFile);
        }

        // Checks wheter the button has been pressed, when it has the the program goes into fullscreen and plays a rickroll
        private void RickButton_Click(object sender, EventArgs e)
        {
            // Changes players
            WMP.Ctlcontrols.stop();
            Rick.Show();

            // Does the same as the start of the cow player
            string strTempFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Rick.mp4");
            File.WriteAllBytes(strTempFile, Properties.Resources.Never_Gonna_Give_You_Up);

            Rick.URL = strTempFile;

            Thread.Sleep(10);
            File.Delete(strTempFile);

            
        }

        // Checks collisions
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Moves the cow
            WMP.Left = WMP.Left + vx;
            WMP.Top = WMP.Top + vy;
            
            // Again with the weird bug
            label1.Text = "bruh";

            // The actuall collision checking
            if ((WMP.Left + WMP.Width) > boundryX || WMP.Left < 0)
            {
                vx *= -1;
            }


            if ((WMP.Top + WMP.Height) > boundryY || WMP.Top < 0)
            {
                vy *= -1;
            }

            // Checks if the rickroll has stopped and resumes the cow
            if (Rick.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                WMP.Ctlcontrols.play();
                Rick.Visible = false;
            }
        }
    }
}
