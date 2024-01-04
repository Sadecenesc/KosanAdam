
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
    public partial class Level1 : Form
    {
        private HighScoreManager highScoreManager = new HighScoreManager();
        private bool isGamePaused = false;
        private Random random = new Random();
        public string oyuncuname;
        public Timer gameTimer;
        private readonly int step = 52;
        public int elapsedTimeInSeconds = 0;
        public int remainingLives = 3;
     
        public int totalElapsedSeconds = 0;
        public int lastremainingLives;
        private Timer secretBoxTimer;
        public PictureBox Secretbox;

        public Level1(string Playername)
        {
            InitializeComponent();
            InitializeSecretBoxTimer();
           PlayerName.Text = Playername;
            oyuncuname = Playername;
            Secretbox = e13;
            // Initialize the timer
            gameTimer = new Timer();
            gameTimer.Interval = 1000; // Set the interval to 1000 milliseconds (1 second)
            gameTimer.Tick += GameTimer_Tick;
        }
        private void SaveHighScoresToFile()
        {
            // Load the current high scores
            List<ScoreEntry> currentHighScores = highScoreManager.LoadHighScores();

            // Add the current player's score (use the player's name and current score)
            ScoreEntry currentPlayerScore = new ScoreEntry
            {
                PlayerName = oyuncuname, // Player's name
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
            remainingLives++;
            MessageBox.Show("You gained +1 health!");
            UpdatePlayerHealthLabel();
        }

        private void CauseDamageFromSecretBox()
        {
            // Reduce -1 health
            remainingLives--;
            MessageBox.Show("Ouch! You lost -1 health!");
            UpdatePlayerHealthLabel();



        }
        private void UpdatePlayerHealthLabel()
        {
            // Update the player health label
            Kalancanlabel.Text = $"{remainingLives}";
        }
        private void level1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGamePaused)
            {
                // Your existing code for handling key events
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
            if (e.KeyCode == Keys.P)
            {
                TogglePause();
            }
        }
        private void CheckForCollision()
        {

            PictureBox[] trapBoxes = {e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };

            foreach (PictureBox trapBox in trapBoxes)
            {
                // Check if pictureBox1 bounds intersect with trapBox bounds
                if (e11.Bounds.IntersectsWith(trapBox.Bounds))
                {
                    trapBox.Visible = true;
                    DecreaseLife();
                }
                else
                {
                    trapBox.Visible = false;
                }
            }

        }
        private void CheckForwinning()
        {
            if (e11.Bounds.IntersectsWith(e12.Bounds))

            {
                StopGame();
                UpdateScoreLabel();
                SaveHighScoresToFile();
                MessageBox.Show("Congradulation You Win!");
                ResetGame();
                
                Level2 gamepage = new Level2(this);
                gamepage.Show();
                gamepage.StartGame();
                this.Hide();

            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            elapsedTimeInSeconds++;
            totalElapsedSeconds++;

            surelabel.Text = $"{elapsedTimeInSeconds}";



        }

        // Method to start the game and the timer
        public void StartGame()
        {
            gameTimer.Start();
        }

        // Method to stop the game and the timer
        public void StopGame()
        {
            gameTimer.Stop();
        }
        private void DecreaseLife()
        {
            remainingLives--;

            // Optionally, perform actions when a life is lost
            if (remainingLives <= 0)
            {




                Kalancanlabel.Text = $"{0}";
                // Game over logic (e.g., show game over message, reset game, etc.)
                UpdateScoreLabel();
                SaveHighScoresToFile();
                StopGame();
                ResetGame();
                MessageBox.Show("Game Over! No more lives.");
                ResetPictureBoxPosition(e11);



            }
            else
            {
                // Update UI or perform other actions when a life is lost
                lastremainingLives = remainingLives;
                Kalancanlabel.Text = $"{remainingLives}";
            }
        }


        private int CalculateScore()
        {
            // Calculate the score based on the given formula
            int score = remainingLives * 500 + (1000 - elapsedTimeInSeconds);
            return score;
        }
        private void UpdateScoreLabel()
        {
            // Update the score label text with the current score
            scoreLabel.Text = $"{CalculateScore()}";
        }
        private void ResetGame()
        {

            // Optionally, implement logic to reset the game state, including resetting lives and positions

            remainingLives = 3;
            Kalancanlabel.Text = $"{remainingLives}";


            // Reset other game state variables or controls as needed
        }

        private void ResetPictureBoxPosition(PictureBox e11)
        {
            // Optionally, reset the position of the specified PictureBox control
            // This can be customized based on your game's requirements
            e11.Location = new Point(65,331);
        }
        public int GetTotalElapsedSeconds()
        {
            return totalElapsedSeconds;
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
    }
}