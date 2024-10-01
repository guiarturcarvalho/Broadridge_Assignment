using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordFrequencyCalcBeta.Infrastructure
{
    /// <summary>
    /// The FileWriter is responsible for:
    /// Writing the word frequencies to the output file;
    /// Creating a file with the desired size for testing;
    /// It supports asynchronous writing to handle large datasets efficiently.
    /// </summary>
    public class FileWriter
    {
        #region Class Variables
        private static readonly string[] RandomWords =
        [
            "apple", "banana", "cat", "broadridge", "elephant", "frog", "GRAPe", "house", "ice", "jungle",
        "kite", "lion", "mountain", "night", "orange", "pencil", "queen", "river", "snake", "tree",
        "umbrella", "violet", "whale", "xylophone", "yacht", "zebra", "file", "input", "application",
        "output", "input", "Boris","grape", "Engineer", "BANANA", "BROADRIDGE"
        ];

        private static readonly char[] PunctuationMarks = [' ', ',', '.', ';', '!', '?', '\n'];
        #endregion

        #region Methods
        /// <summary>
        /// Async method write words and frequency to file
        /// </summary>
        /// <param name="sortedWords">Words and respective frequency (key value par)</param>
        /// <param name="filePath">The Output filepath</param>
        /// <returns></returns>
        public static async Task WriteFrequenciesToFileAsync(List<KeyValuePair<string, int>> sortedWords, string filePath)
        {
            using (StreamWriter writer = new(filePath, false))
            {
                foreach (var kvWord in sortedWords)
                {
                    await writer.WriteLineAsync($"{kvWord.Key},{kvWord.Value}");
                }
            }
        }

        /// <summary>
        /// Method to generate testing file
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <param name="fileSizeInMB">FIle Size</param>
        public static void GenerateRandomTextFile(string filePath, long fileSizeInMB)
        {
            long fileSizeInBytes = fileSizeInMB * 1024 * 1024;
            Random random = new Random();

            using (StreamWriter writer = new(filePath))
            {
                long writtenBytes = 0;
                StringBuilder buffer = new();

                // Generate random words until the file reaches the desired size
                while (writtenBytes < fileSizeInBytes)
                {
                    // Get a random word from the predefined list
                    string word = RandomWords[random.Next(RandomWords.Length)];
                    buffer.Append(word);

                    // Append a random delimiter (space, punctuation, or newline)
                    char delimiter = PunctuationMarks[random.Next(PunctuationMarks.Length)];
                    buffer.Append(delimiter);

                    // Write the buffer to the file if it reaches a certain size (to avoid excessive memory usage)
                    if (buffer.Length > 4096)
                    {
                        writer.Write(buffer.ToString());
                        writtenBytes += Encoding.UTF8.GetByteCount(buffer.ToString());
                        buffer.Clear();
                    }
                }

                // Write any remaining text in the buffer
                if (buffer.Length > 0)
                {
                    writer.Write(buffer.ToString());
                }
            }

            Console.WriteLine($"File '{filePath}' has been created with approximate size {fileSizeInMB} MB.");
        }

        #endregion
    }
}