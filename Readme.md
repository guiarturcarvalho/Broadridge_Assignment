# Word Frequency Counter

## Overview

The **Word Frequency Counter** is a C# console application designed to process large text files and count the frequency of each word. The program handles input files that may exceed several gigabytes in size, using efficient chunk-based processing to minimize memory usage and support scalability. The output is a sorted list of words and their frequencies, which is written to a file in CSV format (`WORD, FREQUENCY`).

## Features

- **Scalable Processing**: Processes large text files in chunks to handle files greater than 2GB.
- **Multithreading Support**: Uses multi-threading to optimize performance for large-scale data.
- **Case-Insensitive Word Counting**: Words are compared without case sensitivity.
- **Customizable Output**: Outputs word frequencies sorted first by frequency and then alphabetically.
- **Error Handling**: Gracefully handles invalid inputs and files.
- **Unit Tests**: Ensures code quality and functionality through unit tests.

## Architecture

The application (Beta) follows a modular architecture (Layered Architecture) with clear separation of concerns, ensuring maintainability and extensibility.

### Directory Structure

```
WordFrequencyCounter
│
├── WordFrequencyCalcAlpha			// Alpha Version (Monolithic Architecture & Simplistic Approach)
│   └── Program.cs				// Monolith
│   │
├── WordFrequencyCalcBeta			// Beta Version (Layered Architecture)
│   └── Program.cs				// Presentation layer | Entry point
│   │
│   ├── Services
│   │   └── FileProcessor.cs			// File chunk reading and processing logic
│   │
│   ├── Infrastructure
│   │   └── FrequencyWriter.cs			// File writing logic
Tests
    └── WordFrequencyTests.cs			// Unit tests
```

### Key Components

- **Program.cs**: This is the entry point of the application. It handles parsing of command-line arguments (input and output file paths), validates the arguments, and invokes the services responsible for file processing and writing the results.
  
- **FileProcessor.cs**: This service is responsible for reading the input file in chunks, processing each chunk to count word occurrences, and aggregating the results. It ensures that the file is processed efficiently, even if it is very large.
  
- **FileWriter.cs**: This service writes the word frequency results to an output file. It supports asynchronous writing to ensure that large datasets are written to disk efficiently.

- **WordFrequencyTests.cs**: Contains unit tests to validate the behavior of the FileProcessor and FileWriter components. These tests ensure the correct counting of word frequencies and the correct output format.

## Prerequisites

- .NET SDK (Core or Framework, depending on your setup)
- Command-line interface (CLI)

## Installation and Setup

1. Clone the repository to your local machine.   
      
2. Open the solution in Visual Studio or your preferred .NET IDE.

3. Build the solution to restore dependencies.

## Usage

WordFrequencyCalcBeta.exe <input-file-path> <output-file-path>

### Example

WordFrequencyCounter.exe "C:\InputFiles\large_text.txt" "C:\OutputFiles\word_frequencies.txt"

## Example Output
GIT,18<br />
is,8<br />
the,5<br />
best,4<br />
in,4<br />
the,4<br />
world,3<br />

## Future Enhancements

1. Cloud Storage Integration: Add support for cloud storage providers (e.g., AWS S3, Azure Blob Storage) to handle distributed file processing.
2. Real-Time Stream Processing: Extend the application to handle real-time text streams for live word frequency analysis.
3. Logging and Monitoring: Add comprehensive logging and monitoring for better debugging and performance tracking.

## Performance Tests

The files are randomly generated. Different files will make for different performance.

### Processing 100MB File

- **8MB Chunks**: 5.4851443s
- **4MB Chunks**: 5.7430506s
- **2MB Chunks**: 5.6665995s
- **1MB Chunks**: 5.8565369s
- **512KB Chunks**: 4.4296505s
- **256KB Chunks**: 4.5430570s
- **128KB Chunks**: 4.5958082s
- **64KB Chunks**: 5.7313746s
- **32KB Chunks**: 4.9154403s
- **16KB Chunks**: 5.7488035s

### Processing 500MB File

- **8MB Chunks**: 24.9320648s
- **4MB Chunks**: 25.3443994s
- **2MB Chunks**: 24.0818753s
- **1MB Chunks**: 23.3326036s
- **512KB Chunks**: 18.1613185s
- **256KB Chunks**: 19.4683624s
- **128KB Chunks**: 18.8758379s
- **64KB Chunks**: 21.2940609s
- **32KB Chunks**: 20.7861753s
- **16KB Chunks**: 24.3048824s

### Processing 1GB File

- **8MB Chunks**: 49.5605301s
- **4MB Chunks**: 50.8340914s
- **2MB Chunks**: 48.1396851s
- **1MB Chunks**: 46.3129412s
- **512KB Chunks**: 36.2043223s
- **256KB Chunks**: 37.4762583s
- **128KB Chunks**: 37.5498954s
- **64KB Chunks**: 41.0212207s
- **32KB Chunks**: 40.3137619s
- **16KB Chunks**: 46.5555143s

### Processing 2GB File

- **8MB Chunks**: 1m37.3064436s
- **4MB Chunks**: 1m38.6957405s
- **2MB Chunks**: 1m32.0818573s
- **1MB Chunks**: 1m28.3735174s
- **512KB Chunks**: 1m10.1388382s
- **256KB Chunks**: 1m14.9966188
- **128KB Chunks**: 1m14.7490725s
- **64KB Chunks**: 1m26.3711728s
- **32KB Chunks**: 1m18.3401208s
- **16KB Chunks**: 1m30.8140600s

### Conclusion

While there is no one-size-fits-all answer to the optimal chunk size, in this case it seems that using 512KB chunks is more effective. Be prepared to adjust based on performance tests and the specific characteristics of your application's workload. Always test with representative data and monitor resource usage to find the best configuration. We must always havee in mind File Size, Memory, I/O Performance, Network or even Processing Overhead.



