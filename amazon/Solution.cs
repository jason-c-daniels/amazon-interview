using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace amazon
{
   public class Solution
    {
        /** This was from my phone interview with Amazon... 
           Parse a roman numeral in an extensible manner. 
           (i.e. keep the standard subtraction, decade, and 5's rules, 
            but allow for extending it further, arbitrarily, in a data driven manner.)


            Symbol	I	V	 X	 L	 C	 D	  M
            Value	1	5	10	50	100	500	1,000

            Rules
            Numerals (I,X,C,M) can repeat up to 3 times, except multiples of 5, which can only occur isolation, or subtractive notation. (V,L,D)

            For values that would have been a succession of 4 characters, use subtractive notation.
            (Subtractive notation from wikipedia)
            I placed before V or X indicates one less, so four is IV (one less than five) and nine is IX (one less than ten)
            X placed before L or C indicates ten less, so forty is XL (ten less than fifty) and ninety is XC (ten less than a hundred)
            C placed before D or M indicates a hundred less, so four hundred is CD (a hundred less than five hundred) and nine hundred is CM (a hundred less than a thousand)[5]

        */
        public int FromRoman(string rn)
        {
            int sum = 0;
            // this implementation assumes the 5's-10s pattern will repeat when/if the alphabet changes.
            int[] vals=       { 1,    5,  10,  50,  100, 500, 1000 };
            char[] numerals = { 'I', 'V', 'X', 'L', 'C', 'D', 'M' };
            var lt = new Dictionary<char, int>();
            // build the base value lookup table lt
            for (int i = 0; i < vals.Length; i++) lt.Add(numerals[i], vals[i]);

            string re = MakeRegex(numerals);
            Regex regex = new Regex(re, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (!regex.IsMatch(rn)) throw new Exception("Invalid Roman Numeral"); //TODO: localize this text.

            MatchCollection mc = regex.Matches(rn);
            foreach (Match m in mc)
            {
                for (int gIdx = 1; gIdx < m.Groups.Count; gIdx++)
                {
                    var v = m.Groups[gIdx].Value;
                    switch(v.Length)
                    {
                        case 0: /*skip the item*/break;
                        case 1: sum += lt[v[0]]; break;
                        case 2:
                            if (lt[v[0]] < lt[v[1]]) sum += (lt[v[1]] - lt[v[0]]);
                            else sum += 2 * lt[v[0]];
                            break;
                        default:
                            if (v[0] == v[1] && v[1] == v[2]) sum += 3 * lt[v[0]];
                            else if (lt[v[0]] > lt[v[1]])  {
                                // MCM type numbers
                                sum += lt[v[0]] + lt[v[2]] - lt[v[1]];
                            }
                            else {
                                // CMM type numbers (rarer roman numeral form, but apparently legitimate)
                                sum += lt[v[1]] + lt[v[2]] - lt[v[0]];
                            }
                            break;
                    }
                }
            }
            return sum;
        }

        public string MakeRegex(char[] N)
        {
            string re = "";
            for (int i= N.Length-1; i>=0;i--)
            {
                //TODO: find out if the intent was to alter max repetitions.
                if (i==0) re += string.Format("({0}{{1,3}})?", N[0]);
                else if (i % 2 == 0) re += string.Format("({1}?{0}{1}|{1}{{1,3}})?", N[i - 2], N[i]); 
                else if (i % 2 == 1) re += string.Format("({0}{1}|{1}{{1,1}})?", N[i - 1], N[i]);
            }

            return "^" +re+"$";
        }
    }
}
