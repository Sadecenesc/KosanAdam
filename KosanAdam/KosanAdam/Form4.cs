using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;
using System.Windows.Forms;


namespace KosanAdam
{
    public partial class Form3 : Form
    {
        private HighScoreManager highScoreManager = new HighScoreManager();
        private bool isGamePaused = false;
        private bool isFirstTick = true;
        public PictureBox Secretbox;
        public PictureBox[] newplaces;
        public PictureBox[] trapBoxes;
        private readonly int step = 52;
        private readonly Level2 _level2;
        private Timer trapBoxTimer;
        private Timer secretBoxTimer;
        public Form3(Level2 level2)
        {
            InitializeComponent();

            InitializeTimer();

            _level2 = level2;

            Secretbox = e13;

            PlayerNameLabel.Text = _level2.oyuncuname;

            _level2.OyunSuresi.Tick += GameTimer_Tick;

            _level2.LastRemainingLives = _level2.LastRemainingLives + 1;
            Kalancanlabel.Text = $"{_level2.LastRemainingLives}";

            trapBoxes = new PictureBox[] { e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            newplaces = new PictureBox[] { a7, a8, a9, a10, a11, a12, a13, a14, a15,
                a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30 };
            InitializeSecretBoxTimer();

        }
        private void SaveHighScoresToFile()
        {
            // Load the current high scores
            List<ScoreEntry> currentHighScores = highScoreManager.LoadHighScores();

            // Add the current player's score (use the player's name and current score)
            ScoreEntry currentPlayerScore = new ScoreEntry
            {
                PlayerName = _level2.oyuncuname, // Player's name
                Score = CalculateScore() // Current score
            };

            currentHighScores.Add(currentPlayerScore);

            // Save the updated scores to a Notepad file
            highScoreManager.SaveHighScores(currentHighScores);
        }
        private void InitializeSecretBoxTimer()
        {
            secretBoxTimer = new Timer();
            secretBoxTimer.Interval = 10000; // 10 seconds
            secretBoxTimer.Tick += SecretBoxTimer_Tick;
            secretBoxTimer.Start();
        }
        private void SecretBoxTimer_Tick(object sender, EventArgs e)
        {
            if (!isGamePaused)
            {
                // Show the SecretBox when the timer ticks
                e13.Visible = !e13.Visible;
            }
        }
        private void SecretBox_Click_1(object sender, EventArgs e)
        {
            int randomNumber = random.Next(100);

            if (!isGamePaused)
            {
                // 80% chance of benefiting
                if (randomNumber < 80)
                {
                    BenefitFromSecretBox();
                }
                // 20% chance of causing damage
                else
                {
                    CauseDamageFromSecretBox();
                }

                // Hide the SecretBox after it's clicked
                e13.Visible = false;
            }
        }

        private void BenefitFromSecretBox()
        {
            // Add +1 health
            _level2.LastRemainingLives++;
            MessageBox.Show("You gained +1 health!");
            UpdatePlayerHealthLabel();
        }

        private void CauseDamageFromSecretBox()
        {
            // Reduce -1 health
            _level2.LastRemainingLives--;
            MessageBox.Show("Ouch! You lost -1 health!");
            UpdatePlayerHealthLabel();



        }
        private void UpdatePlayerHealthLabel()
        {
            // Update the player health label
            Kalancanlabel.Text = $"{_level2.LastRemainingLives}";
        }


        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGamePaused)
            {
                // Check for movement keys only if the game is not paused
                switch (e.KeyCode)
                {
                    case Keys.Right:
                        e11.Left += step;
                        break;
                    case Keys.Left:
                        e11.Left -= step;
                        break;
                    case Keys.Up:
                        e11.Top -= step;
                        break;
                    case Keys.Down:
                        e11.Top += step;
                        break;
                }

                CheckForCollision();
                CheckForwinning();
            }

            // Check for the "P" key to toggle pause/resume
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
            }
        }
        public void CheckForCollision()
        {

            //PictureBox[] trapBoxes = { TrapBox, TrapBox1, TrapBox2, TrapBox3, TrapBox4, TrapBox5, TrapBox6, TrapBox7, TrapBox8, TrapBox9 };

            foreach (PictureBox trapBox in trapBoxes)
            {
                // Check if pictureBox1 bounds intersect with trapBox bounds
                if (trapBox.Visible && e11.Bounds.IntersectsWith(trapBox.Bounds))
                {

                    DecreaseLife();
                }

            }
        }
        private void DecreaseLife()
        {
            _level2.LastRemainingLives--;

            // Optionally, perform actions when a life is lost
            if (_level2.LastRemainingLives <= 0)
            {




                Kalancanlabel.Text = $"{0}";
                // Game over logic (e.g., show game over message, reset game, etc.)
                UpdateScoreLabel();
                SaveHighScoresToFile();
                StopGame();
                //ResetGame();
                MessageBox.Show("Game Over! No more lives.");
                //ResetPictureBoxPosition(e11);
                // yapamadım burayı!!! olmadı dön bak birdaha


            }
            else
            {
                // Update UI or perform other actions when a life is lost

                Kalancanlabel.Text = $"{_level2.LastRemainingLives}";
            }

        }
        private int CalculateScore()
        {
            // Calculate the score based on the given formula
            int score = _level2.LastRemainingLives * 500 + (1000 - _level2.TotalSeconds);
            return score;
        }
        private void UpdateScoreLabel()
        {
            // Update the score label text with the current score
            scoreLabel.Text = $"{CalculateScore()}";
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {



            Surelable.Text = $"{_level2.TotalSeconds}";



        }
        public void StartGame()
        {
            _level2.OyunSuresi.Start();
        }

        // Method to stop the game and the timer
        public void StopGame()
        {
            _level2.OyunSuresi.Stop();
        }

        private void InitializeTimer()
        {

            trapBoxTimer = new Timer();
            trapBoxTimer.Interval = 1000; // Set the interval to 1000 milliseconds (1 seconds)
            trapBoxTimer.Tick += TrapBoxTimer_Tick;
            trapBoxTimer.Start(); // Start the timer
        }

        private void CheckForwinning()
        {
            if (e11.Bounds.IntersectsWith(e12.Bounds))

            {
                StopGame();
                UpdateScoreLabel();
                SaveHighScoresToFile();
                MessageBox.Show("Congradulation You Win!");
                //ResetGame();
                //ResetPictureBoxPosition(e11);




            }
        }
        Random random = new Random();



        
        public void UpdateTrapBoxes()
        {
            PictureBox[] newplaces = {  a7, a8, a9, a10, a11, a12, a13, a14, a15,
                               a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30 };

            // Ensure that trapBoxes is declared at the class level
            //PictureBox[] trapBoxes = { TrapBox, TrapBox1, TrapBox2, TrapBox3, TrapBox4, TrapBox5, TrapBox6,  TrapBox8, TrapBox9 };

            List<PictureBox> selectedPictureBoxes = new List<PictureBox>();
            Random random = new Random();

            // Randomly select 10 PictureBoxes
            while (selectedPictureBoxes.Count < 10)
            {
                PictureBox randomPictureBox = trapBoxes[random.Next(trapBoxes.Length)];

                // Ensure the PictureBox is not already selected
                if (!selectedPictureBoxes.Contains(randomPictureBox))
                {
                    selectedPictureBoxes.Add(randomPictureBox);
                }
            }

            foreach (PictureBox trapBox in selectedPictureBoxes)
            {
                // Randomly select a new location from the newplaces array
                PictureBox newLocation = newplaces[random.Next(newplaces.Length)];

                // Set the new position
                trapBox.Location = newLocation.Location;

                // Toggle visibility
                trapBox.Visible = !trapBox.Visible;
            }
        }





        private void TrapBoxTimer_Tick(object sender, EventArgs e)
        {


            if (isFirstTick)
            {
                // Update trap box positions and visibility when they first appear
                if (!isGamePaused)
                {
                    UpdateTrapBoxes();
                }
            }
            else
            {
                // Move the trap boxes to the left one step after one second
                if (!isGamePaused)
                {
                    MoveTrapBoxesLeft();
                }
            }

            // Toggle the flag for the next tick
            isFirstTick = !isFirstTick;
        }

        private void MoveTrapBoxesLeft()
        {
            // Move each trap box to the left by one step
            foreach (PictureBox trapBox in trapBoxes)
            {
                trapBox.Left -= step;
            }
        }


        private void TogglePause()
        {
            isGamePaused = !isGamePaused;

            if (isGamePaused)
            {
                StopGame(); // Stop the timer or any other game-related activities
                MessageBox.Show("Game Paused");
            }
            else
            {
                StartGame(); // Resume the timer or any other game-related activities
                MessageBox.Show("Game Resumed");
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
