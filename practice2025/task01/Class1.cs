using System;
using System.Linq;

namespace task01
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string input)
        {
            if (string.IsNullOrEmpty(input))
               return false;
        
            char[] downcased = new char[input.Length];
            for (int i = 0; i < input.Length; i++) 
            {
                downcased[i] = char.ToLower(input[i]);
            }

            int CharCount = 0;
            for (int i = 0; i < downcased.Length; i++)
            {
                if (!char.IsWhiteSpace(downcased[i]) && !char.IsPunctuation(downcased[i]))
                    CharCount++;
            }

            char[] newString = new char[CharCount];
            int index = 0;
            for (int i = 0; i < downcased.Length; i++)
            {
                if (!char.IsWhiteSpace(downcased[i]) && !char.IsPunctuation(downcased[i]))
                {
                    newString[index] = downcased[i];
                    index++;
                }
            }

            char[] newString2 = new char[newString.Length];
            for (int i = 0; i < newString.Length; i++)
            {
                newString2[i] = newString[newString.Length - 1 - i];
            }

            for (int i = 0; i < newString.Length; i++)
            {
                if (newString[i] != newString2[i])
                    return false;
            }

            return true;
        }
    }
}