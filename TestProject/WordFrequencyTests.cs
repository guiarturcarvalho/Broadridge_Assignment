using Microsoft.VisualStudio.TestPlatform.TestHost;
using WordFrequencyCalcBeta.Infrastructure;
using WordFrequencyCalcBeta.Services;

namespace TestProject
{
    /// <summary>
    /// Unit tests for the Word Frequency Counter application (alpha version).
    /// Ensures that the independ methos are working
    /// </summary>
    public class AlphaTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Tests the alpha word processing method
        /// </summary>
        [Test]
        public void TestWordFrequencies()
        {
            //var input = new[] { "This is a test.", "This test is simple." };

            var file = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\input_text_alpha.txt";
            var wordFrequencies = ProgramAlpha.GetWordFrequencies(file);

            Assert.That(wordFrequencies["This"], Is.AtLeast(1));
            Assert.That(wordFrequencies["is"], Is.AtLeast(1));
            Assert.That(wordFrequencies["test"], Is.AtLeast(1));
            Assert.That(wordFrequencies["a"], Is.EqualTo(3));
            Assert.That(wordFrequencies["simple"], Is.EqualTo(1));
        }

        /// <summary>
        /// Tests the full monolithic alpha word counter
        /// </summary>
        [Test]
        public void CompleteTest()
        {            
            var inputFile = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\input_text_alpha.txt";
            var outputFile = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\output_text_alpha.txt";
            var wordFrequencies = ProgramAlpha.GetWordFrequencies(inputFile);

            // Sort by frequency and then alphabetically
            var sortedWords = wordFrequencies
                .OrderByDescending(w => w.Value)
                .ThenBy(w => w.Key)
                .ToList();

            ProgramAlpha.WriteFrequenciesToFile(sortedWords, outputFile);

            Assert.That(outputFile, Does.Exist);

        }        
    }

    /// <summary>
    /// Unit tests for the Word Frequency Counter application.
    /// Ensures that the FileProcessor and FileWriter behave correctly under different conditions.
    /// </summary>
    public class BetaTests
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Creates a testing file with random words and punctuation
        /// </summary>
        [Test, Order(1)]
        public void CreateTestFile()
        {
            var filePath = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\input_text_beta.txt";
            ///Create file for testing            
            FileWriter.GenerateRandomTextFile(filePath, 2000);

            Assert.That(filePath, Does.Exist);

        }

        /// <summary>
        /// Tests the file processing method
        /// </summary>
        [Test]
        public async Task TestWordFrequencies()
        {
            //var input = new[] { "This is a test.", "This test is simple." };

            var file = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\input_text_beta.txt";
            var wordFrequencies = await FileProcessor.ProcessFileByChunksAsync(file);

            Assert.That(wordFrequencies["Boris"], Is.AtLeast(10));
            Assert.That(wordFrequencies["BANANA"], Is.AtLeast(50));
            Assert.That(wordFrequencies["BROADRIDGE"], Is.AtLeast(50));
            Assert.That(wordFrequencies["broadridge"], Is.AtLeast(50));
            Assert.That(wordFrequencies["banana"], Is.AtLeast(50));
        }

        /// <summary>
        /// Tests everything: 
        /// 1. File reading and processing;
        /// 2. Sorting
        /// 3. Writing the results
        /// </summary>
        [Test]
        public async Task CompleteTest()
        {
            var inputFile = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\input_text_beta.txt";
            var outputFile = "C:\\Users\\guiar\\Documents\\GitHub\\Broadridge_Assignment\\output_text_beta.txt";
            var wordFrequencies = await FileProcessor.ProcessFileByChunksAsync(inputFile);

            // Sort by frequency and then alphabetically
            var sortedWords = wordFrequencies
                .OrderByDescending(w => w.Value)
                .ThenBy(w => w.Key)
                .ToList();

            await FileWriter.WriteFrequenciesToFileAsync(sortedWords, outputFile);

            Assert.That(outputFile, Does.Exist);

        }
        
    }
}