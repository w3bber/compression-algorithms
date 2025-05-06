using System;
using System.Collections.Generic;
using System.Text;

public class RLE
{
    private string input;
    public RLE(string input)
    {
        this.input = input;
    }
    public List<Tuple<char, int>> Encode()
    {
        if (input == null || input.Length == 0)
        {
            throw new ArgumentException("String must have at least one letter.");
        }
        List<Tuple<char, int>> encoded = new List<Tuple<char, int>>();
        int counter = 0;
        char prev = input[0];

        for (var index = 1; index <= input.Length; index++)
        {
            var ch = index == input.Length ? '\0' : input[index];
            counter++;
            if (ch == prev)
                continue;

            encoded.Add(new Tuple<char, int>(prev, counter));
            counter = 0;
            prev = ch;
        }

        return encoded;
    }
    public string Decode(List<Tuple<char, int>> inp)
    {
        if (inp == null)
        {
            throw new ArgumentException("Input string cannot be null or empty.");
        }

        StringBuilder decoded = new StringBuilder();

        foreach (var tuple in inp)
        {
            char ch = tuple.Item1;
            int length = tuple.Item2;

            decoded.Append(ch, length);
        }
        return decoded.ToString();
    }
}