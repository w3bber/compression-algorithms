using System;
using System.Collections.Generic;
using System.Text;

namespace Compression
{
    public class LZ78
    {
        private string text;
        public LZ78(string text)
        {
            this.text = text;
        }
        public List<Tuple<int, char>> Encode()
        {
            var dictionary = new Dictionary<string, int>();
            var compressed = new List<Tuple<int, char>>();
            int nextCode = 1;
            string currentSubstring = "";

            foreach (char c in text)
            {
                string temp = currentSubstring + c;
                if (dictionary.ContainsKey(temp))
                {
                    currentSubstring = temp;
                }
                else
                {
                    if (currentSubstring != "")
                    {
                        compressed.Add(new Tuple<int, char>(dictionary[currentSubstring], c));
                    }
                    else
                    {
                        compressed.Add(new Tuple<int, char>(0, c));
                    }

                    dictionary[temp] = nextCode;
                    nextCode++;
                    currentSubstring = "";
                }
            }

            if (currentSubstring != "")
            {
                compressed.Add(new Tuple<int, char>(dictionary[currentSubstring], '\0'));
            }

            return compressed;
        }
        public string Decompress(List<Tuple<int, char>> compressed)
        {
            var dictionary = new List<string>();
            StringBuilder decompressed = new StringBuilder();

            foreach (var item in compressed)
            {
                if (item.Item1 == 0)
                {
                    decompressed.Append(item.Item2);
                    dictionary.Add(item.Item2.ToString());
                }
                else
                {
                    string entry = dictionary[item.Item1 - 1] + item.Item2;
                    decompressed.Append(entry);
                    dictionary.Add(entry);
                }
            }

            return decompressed.ToString();
        }
    }
}
