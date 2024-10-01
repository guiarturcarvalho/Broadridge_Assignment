using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using WordFrequencyCalcBeta.Infrastructure;
using WordFrequencyCalcBeta.Services;

/// <summary>
/// Beta program entry point
/// </summary>
public class ProgramBeta
{
    /// <summary>
    /// The main entry point of the Word Frequency Counter application.
    /// Handles command-line argument parsing and initiates file processing.
    /// </summary>
    /// <param name="args">Command-line arguments: 
    /// args[0] should be the input file path, and args[1] should be the output file path.</param>
    public static async Task Main(string[] args)
    {
        //Performance review
        Stopwatch sw = new();

        sw.Start();
        Console.WriteLine("Started={0}", sw.Elapsed);

        if (args.Length < 2)
        {
            Console.WriteLine("Usage: WordFrequencyCalcBeta.exe <inputFilePath> <outputFilePath>");
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

        // Process the input file asynchronously
        try
        {

            //var wordFrequencies = await FileProcessor.ProcessFileByLineAsync(inputFilePath); // too slow for big files (specially with a lot of lines)
            var wordFrequencies = await FileProcessor.ProcessFileByChunksAsync(inputFilePath);

            Console.WriteLine("File Read={0}", sw.Elapsed);

            // Sort by frequency and word
            var sortedWords = wordFrequencies
                .OrderByDescending(w => w.Value)
                .ThenBy(w => w.Key)
                .ToList();

            // Write the results asynchronously
            await FileWriter.WriteFrequenciesToFileAsync(sortedWords, outputFilePath);

            Console.WriteLine($"Processing complete. Output written to: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        sw.Stop();

        Console.WriteLine("Elapsed={0}", sw.Elapsed);
    }
}
