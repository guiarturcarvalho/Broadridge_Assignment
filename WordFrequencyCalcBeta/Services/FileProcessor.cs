using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordFrequencyCalcBeta.Services
{

    /// <summary>
    /// The FileProcessor service handles reading large files in chunks or in lines, 
    /// processing each chunk to count word occurrences, and then aggregating the results.
    /// </summary>
    public static class FileProcessor
    {
        #region Class Variables
        // The chunk size for reading files in byte            
        private const int _chunkSize = 512 * 1024 ; // 512KB   

        #endregion

        #region Methods
        /// <summary>
        /// Processes the input file to count word frequencies (line by line).
        /// </summary>
        /// <param name="filePath">Input file location</param>
        /// <returns>Dictionary with words and respective frequency</returns>
        public static async Task<ConcurrentDictionary<string, int>> ProcessFileByLineAsync(string filePath)
        {
            var wordFrequencies = new ConcurrentDictionary<string, int>();
            var delimiters = new Regex(@"\W+");

            // Asynchronously read the file in chunks
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, _chunkSize, true))
            using (StreamReader reader = new StreamReader(fs))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    // Task to process each line in parallel for scalability
                    Parallel.ForEach(delimiters.Split(line), word =>
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            wordFrequencies.AddOrUpdate(word, 1, (key, count) => count + 1);
                        }
                    });
                }
            }

            return wordFrequencies;
        }

        /// <summary>
        ///  Processes the input file to count word frequencies (by chunk).
        /// </summary>
        /// <param name="filePath">Input file location</param>
        /// <returns>Dictionary with words and respective frequency</returns>
        public static async Task<ConcurrentDictionary<string, int>> ProcessFileByChunksAsync(string filePath)
        {
            var wordFrequencies = new ConcurrentDictionary<string, int>();
            var delimiters = new Regex(@"\W+"); // Matches any non-word character

            // Open the file for reading asynchronously
            using (FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, _chunkSize, true))
            {
                byte[] buffer = new byte[_chunkSize];
                int bytesRead;
                StringBuilder leftover = new StringBuilder(); // To store partial words between chunks

                while ((bytesRead = await fs.ReadAsync(buffer)) > 0)
                {
                    // Convert the chunk of bytes to a string
                    string chunkText = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Combine with leftover from the previous chunk (if any)
                    chunkText = leftover + chunkText;

                    // Split into words
                    var words = delimiters.Split(chunkText);

                    // Handle words in the middle of the chunk
                    leftover.Clear();
                    if (!delimiters.IsMatch(chunkText[^1].ToString()))
                    {
                        leftover.Append(words[^1]); // Store the last word if incomplete
                        words[^1] = ""; // Mark the last word as incomplete
                    }

                    // Process the words in parallel
                    Parallel.ForEach(words, word =>
                    {
                        if (!string.IsNullOrWhiteSpace(word))
                        {
                            wordFrequencies.AddOrUpdate(word, 1, (key, count) => count + 1);
                        }
                    });
                }

                // In case the last leftover contains a valid word
                if (leftover.Length > 0)
                {
                    var finalWord = leftover.ToString();
                    if (!string.IsNullOrWhiteSpace(finalWord))
                    {
                        wordFrequencies.AddOrUpdate(finalWord, 1, (key, count) => count + 1);
                    }
                }
            }

            return wordFrequencies;
        } 

        #endregion


    }
}