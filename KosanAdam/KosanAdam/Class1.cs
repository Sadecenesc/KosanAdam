
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;
using System.Security.Cryptography.X509Certificates;

namespace KosanAdam
{




    public class ScoreEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }

    public class HighScoreManager
    {
        private const string FileName = "highscores.txt";

        public List<ScoreEntry> LoadHighScores()
        {
            List<ScoreEntry> highScores = new List<ScoreEntry>();

            try
            {
                if (File.Exists(FileName))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(FileName);

                    foreach (string line in lines)
                    {
                        // Split each line into PlayerName and Score
                        string[] parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                        {
                            ScoreEntry entry = new ScoreEntry
                            {
                                PlayerName = parts[0],
                                Score = score
                            };
                            highScores.Add(entry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle file reading errors
                Console.WriteLine($"Error reading high scores: {ex.Message}");
            }

            return highScores.OrderByDescending(entry => entry.Score).ToList();
        }

        public void SaveHighScores(List<ScoreEntry> highScores)
        {
            try
            {
                // Write high scores to the file
                using (StreamWriter writer = new StreamWriter(FileName))
                {
                    foreach (ScoreEntry entry in highScores.Take(5))
                    {
                        writer.WriteLine($"{entry.PlayerName}:{entry.Score}");
                    }
                }

                Console.WriteLine("High scores saved successfully to Notepad.");
            }
            catch (Exception ex)
            {
                // Handle file writing errors
                Console.WriteLine($"Error saving high scores to Notepad: {ex.Message}");
            }
        }
    }

}
