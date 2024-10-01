using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Alpha program entry point
/// </summary>
public class ProgramAlpha
{
    /// <summary>
    /// Main method - Alpha
    /// </summary>
    /// <param name="args">Command line parameters "inputFilePath outputFilePath"</param>
    public static void Main(string[] args)
    {

        //Performance review
        Stopwatch sw = new Stopwatch();

        sw.Start();
        Console.WriteLine("Started={0}", sw.Elapsed);

        // Validate input arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: WordFrequencyCounter <inputFilePath> <outputFilePath>");
            return;
        }

        string inputFilePath = args[0];
        string outputFilePath = args[1];

        // Validate input file
        if (!File.Exists(inputFilePath))
        {
            Console.WriteLine($"Error: Input file '{inputFilePath}' not found.");
            return;
        }

        // Process file and get word frequencies
        var wordFrequencies = GetWordFrequencies(inputFilePath);

        Console.WriteLine("File Read={0}", sw.Elapsed);

        // Sort by frequency and then alphabetically
        var sortedWords = wordFrequencies
            .OrderByDescending(w => w.Value)
            .ThenBy(w => w.Key)
            .ToList();

        // Write the results to the output file
        WriteFrequenciesToFile(sortedWords, outputFilePath);

        Console.WriteLine("Processing complete. Output written to: " + outputFilePath);

        sw.Stop();
        Console.WriteLine("Elapsed={0}", sw.Elapsed);
    }

    /// <summary>
    /// Method to get word frequencies from the input file
    /// </summary>
    /// <param name="filePath">Input file location</param>
    /// <returns>Dictionary with words and respective frequency</returns>
    public static ConcurrentDictionary<string, int> GetWordFrequencies(string filePath)
    {
        var wordFrequencies = new ConcurrentDictionary<string, int>();

        //Words and numbers
        var delimiters = new Regex(@"\W+");

        // Read file in parallel for scalability
        Parallel.ForEach(File.ReadLines(filePath), line =>
        {
            var words = delimiters.Split(line);
            foreach (var word in words)
            {
                if (string.IsNullOrWhiteSpace(word)) continue;

                wordFrequencies.AddOrUpdate(word, 1, (key, count) => count + 1);
            }
        });

        return wordFrequencies;
    }

    /// <summary>
    /// Method to write the sorted word frequencies to a file
    /// </summary>
    /// <param name="sortedWords">sorted word dicionary</param>
    /// <param name="filePath">output file location</param>
    public static void WriteFrequenciesToFile(List<KeyValuePair<string, int>> sortedWords, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var kvWord in sortedWords)
            {
                writer.WriteLine($"{kvWord.Key},{kvWord.Value}");
            }
        }
    }
}
