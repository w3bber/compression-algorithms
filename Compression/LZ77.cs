using System;
using System.Collections.Generic;
using System.Text;

namespace Compression
{
    public class LZ77
    {
        private string input;
        private int windowSize;
        private int lookAheadBuffer;

        public LZ77(string input)
        {
            this.input = input;
            this.windowSize = DetermineWindowSize(input.Length);
            this.lookAheadBuffer = windowSize / 2;
        }
        private int DetermineWindowSize(int length)
        {
            if (length < 50)
            {
                return 4; // Очень маленькое окно для очень коротких строк
            }
            else if (length < 200)
            {
                return 16; // Маленькое окно для коротких строк
            }
            else if (length < 500)
            {
                return 32; // Среднее окно для коротких-средних строк
            }
            else if (length < 1000)
            {
                return 128; // Среднее окно для средних строк
            }
            else if (length < 5000)
            {
                return 512; // Большое окно для длинных строк
            }
            else
            {
                return 1024; // Очень большое окно для очень длинных строк
            }
        }
        public List<Tuple<int, int, char>> Encode()
        {
            List<Tuple<int, int, char>> encoded = new List<Tuple<int, int, char>>();

            int inputLength = input.Length;
            int currentIndex = 0;

            while (currentIndex < inputLength)
            {
                int matchLength = 0;
                int matchPosition = 0;

                int searchWindowStart = Math.Max(0, currentIndex - windowSize);
                int searchWindowEnd = currentIndex;

                for (int i = searchWindowStart; i < searchWindowEnd; i++)
                {
                    int len = 0;
                    while (currentIndex + len < inputLength && len < lookAheadBuffer && input[i + len] == input[currentIndex + len])
                        len++;

                    if (len > matchLength)
                    {
                        matchLength = len;
                        matchPosition = currentIndex - i;
                    }
                }

                if (matchLength == 0)
                {
                    encoded.Add(new Tuple<int, int, char>(0, 0, input[currentIndex]));
                    currentIndex++;
                }
                else
                {
                    char symbol = (currentIndex + matchLength < inputLength) ? input[currentIndex + matchLength] : '\0';
                    encoded.Add(new Tuple<int, int, char>(matchPosition, matchLength, symbol));
                    currentIndex += matchLength + 1;
                }
            }

            return encoded;
        }

        public string Decode(List<Tuple<int, int, char>> encoded)
        {
            StringBuilder decoded = new StringBuilder();
            foreach (var tuple in encoded)
            {
                int position = tuple.Item1;
                int length = tuple.Item2;
                char nextChar = tuple.Item3;

                if (position == 0 && length == 0)
                {
                    decoded.Append(nextChar);
                }
                else
                {
                    int startIndex = decoded.Length - position;
                    for (int i = 0; i < length; i++)
                    {
                        decoded.Append(decoded[startIndex + i]);
                    }
                    decoded.Append(nextChar);
                }
            }
            return decoded.ToString();
        }
    }
}
