using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Compression
{
    public class CompressionBenchmark
    {
        public static (List<Tuple<int, char>>, string, long) MeasureEncoding(LZ78 algorithm)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long startMemory = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            var encoded = algorithm.Encode();

            stopwatch.Stop();
            long endMemory = GC.GetTotalMemory(false);

            TimeSpan ts = stopwatch.Elapsed;
            long memoryUsed = endMemory - startMemory;

            return (encoded, ts.TotalMilliseconds.ToString(), memoryUsed);
        }

        public static (string, string, long) MeasureDecoding(LZ78 algorithm, List<Tuple<int, char>> compressed)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long startMemory = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            string decompressed = algorithm.Decompress(compressed);

            stopwatch.Stop();
            long endMemory = GC.GetTotalMemory(false);

            TimeSpan ts = stopwatch.Elapsed;
            long memoryUsed = endMemory - startMemory;

            return (decompressed, ts.TotalMilliseconds.ToString(), memoryUsed);
        }

        public static double CalculateCompressionRatio(string originalText, List<Tuple<int, char>> compressed)
        {
            int originalSize = originalText.Length * 8; // размер исходного текста в битах (предполагаем 8 бит на символ)

            // Рассчитываем размер сжатых данных в битах
            int compressedSize = 0;
            foreach (var tuple in compressed)
            {
                int countBitsOffset = Convert.ToString(tuple.Item1, 2).Length;
                int countBitsChar = 8;

                compressedSize += countBitsOffset + countBitsChar;
            }

            return (double)compressedSize / originalSize;
        }

        public static double CalculateBPC(string originalText, List<Tuple<int, char>> compressed)
        {
            int originalSize = originalText.Length; // количество символов в исходном тексте
            int compressedSize = 0;
            foreach (var tuple in compressed)
            {
                int countBitsOffset = Convert.ToString(tuple.Item1, 2).Length;
                int countBitsChar = 8;

                compressedSize += countBitsOffset + countBitsChar;
            }

            return (double)compressedSize / originalSize;
        }

        public static (List<Tuple<int, int, char>>, string, long) MeasureEncoding(LZ77 algorithm)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long startMemory = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            var encoded = algorithm.Encode();

            stopwatch.Stop();
            long endMemory = GC.GetTotalMemory(false);

            TimeSpan ts = stopwatch.Elapsed;
            long memoryUsed = endMemory - startMemory;

            return (encoded, ts.TotalMilliseconds.ToString(), memoryUsed);
        }

        public static (string, string, long) MeasureDecoding(LZ77 algorithm, List<Tuple<int, int, char>> compressed)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long startMemory = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            string decompressed = algorithm.Decode(compressed);

            stopwatch.Stop();
            long endMemory = GC.GetTotalMemory(false);

            TimeSpan ts = stopwatch.Elapsed;
            long memoryUsed = endMemory - startMemory;

            return (decompressed, ts.TotalMilliseconds.ToString(), memoryUsed);
        }

        public static double CalculateCompressionRatio(string originalText, List<Tuple<int, int, char>> compressed)
        {
            int originalSize = originalText.Length * 8; // размер исходного текста в битах (предполагаем 8 бит на символ)

            // Рассчитываем размер сжатых данных в битах
            int compressedSize = 0;
            foreach (var tuple in compressed)
            {
                int countBitsOffset = Convert.ToString(tuple.Item1, 2).Length;
                int countBitsLength = Convert.ToString(tuple.Item2, 2).Length;
                int countBitsChar = 8;

                compressedSize += countBitsOffset + countBitsLength + countBitsChar;
            }

            return (double)compressedSize / originalSize;
        }

        public static double CalculateBPC(string originalText, List<Tuple<int, int, char>> compressed)
        {
            int originalSize = originalText.Length; // количество символов в исходном тексте
            int compressedSize = 0;
            foreach (var tuple in compressed)
            {
                int countBitsOffset = Convert.ToString(tuple.Item1, 2).Length;
                int countBitsLength = Convert.ToString(tuple.Item2, 2).Length;
                int countBitsChar = 8;

                compressedSize += countBitsOffset + +countBitsLength + countBitsChar;
            }

            return (double)compressedSize / originalSize;
        }

        public static (List<Tuple<char, int>>, string, long) MeasureEncoding(RLE algorithm)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long startMemory = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            var encoded = algorithm.Encode();

            stopwatch.Stop();
            long endMemory = GC.GetTotalMemory(false);

            TimeSpan ts = stopwatch.Elapsed;
            long memoryUsed = endMemory - startMemory;

            return (encoded, ts.TotalMilliseconds.ToString(), memoryUsed);
        }

        public static (string, string, long) MeasureDecoding(RLE algorithm, List<Tuple<char, int>> compressed)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            long startMemory = GC.GetTotalMemory(true);
            Stopwatch stopwatch = Stopwatch.StartNew();

            string decompressed = algorithm.Decode(compressed);

            stopwatch.Stop();
            long endMemory = GC.GetTotalMemory(false);

            TimeSpan ts = stopwatch.Elapsed;
            long memoryUsed = endMemory - startMemory;

            return (decompressed, ts.TotalMilliseconds.ToString(), memoryUsed);
        }
        public static double CalculateCompressionRatio(string originalText, List<Tuple<char, int>> compressedText)
        {
            int originalSize = originalText.Length * 8; // размер исходного текста в битах (предполагаем 8 бит на символ)

            // Рассчитываем размер сжатых данных в битах
            int compressedSize = 0;
            foreach (var tuple in compressedText)
            {
                int countBitsChar = 8;
                int countBitsReapeats = Convert.ToString(tuple.Item2, 2).Length;

                compressedSize += countBitsChar + countBitsReapeats;
            }


            return (double)compressedSize / originalSize;
        }

        public static double CalculateBPC(string originalText, List<Tuple<char, int>> compressedText)
        {
            int originalSize = originalText.Length; // количество символов в исходном тексте
                                                    // Рассчитываем размер сжатых данных в битах
            int compressedSize = 0;
            foreach (var tuple in compressedText)
            {
                int countBitsChar = 8;
                int countBitsReapeats = Convert.ToString(tuple.Item2, 2).Length;

                compressedSize += countBitsChar + countBitsReapeats;
            }

            return (double)compressedSize / originalSize;
        }
    }
}

