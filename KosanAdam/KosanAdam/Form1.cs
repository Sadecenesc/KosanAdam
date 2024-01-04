using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KosanAdam
{
    public partial class ana : Form
    {
        public ana()
        {
            InitializeComponent();
        }
        private void oyuncuAdıtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Level1 gamepage = new Level1(oyuncuIsmıtextBox.Text);
                gamepage.Show();
               gamepage.StartGame();
                this.Hide();

            }
        }

        private void HighscoreLabel_Click(object sender, EventArgs e)
        {

            // Specify the path to the highscore.txt file
            string filePath = "highscores.txt";

            // Check if the file exists before attempting to open it
            //if (File.Exists(filePath))
            //{
            //    // Open the file with the default text editor
            //    Process.Start(filePath);
            //}



        }

        private void TuşTakımları_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Oyun başladığında oyuncu üç tuşu kullanarak hareket edecektir: yukarı, aşağı, sağ ve sol. Oynatmayı duraklatırsanız P tuşuna basacaksınız");
            MessageBox.Show("Oyunun tadını çıkarın :)");
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
