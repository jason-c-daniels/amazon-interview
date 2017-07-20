using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

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
            Numerals (I,X,C,M) can repeat up to 3 times. Numerals that are multiples of 5 can only occur isolation, or in subtractive notation. (V,L,D)

            For values that would have been a succession of 4 characters, use subtractive notation.
            (Subtractive notation from wikipedia)
            I placed before V or X indicates one less, so four is IV (one less than five) and nine is IX (one less than ten)
            X placed before L or C indicates ten less, so forty is XL (ten less than fifty) and ninety is XC (ten less than a hundred)
            C placed before D or M indicates a hundred less, so four hundred is CD (a hundred less than five hundred) and nine hundred is CM (a hundred less than a thousand)[5]
            CMM and MCM are equivalent notation for 1900.

        */
        public int FromRoman(string rn)
        {
            rn = rn.ToUpperInvariant();
            int sum = 0;
            char[] baseNumerals;
            var numeralToValueTable =BuildLookupTable(out baseNumerals);

            string expr = MakeNumberParser(baseNumerals);
            Regex regex = new Regex(expr, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            if (string.IsNullOrWhiteSpace(rn) || !regex.IsMatch(rn)) throw new Exception("Invalid Roman Numeral"); //TODO: localize this text.

            MatchCollection mc = regex.Matches(rn);
            foreach (Match m in mc)
            {
                for (int gIdx = 1; gIdx < m.Groups.Count; gIdx++)
                {
                    var v = m.Groups[gIdx].Value;
                    switch (v.Length)
                    {
                        case 0: /*skip the item, group with no matches. safe to ignore.*/ break;
                        case 1: sum += numeralToValueTable [v[0]]; break;
                        default: // subtractive or just stacked, handles an arbitrary number of numerals
                            {
                                var numerals = v.ToCharArray().ToList();
                                // sort acending according to the lookup table.
                                numerals.Sort(new Comparison<char>((l, r) => numeralToValueTable [l] - numeralToValueTable [r]));
                                // subtract/add the first numeral.
                                // if the first item is less than the second it's a subtraction. Otherwise it's an add.
                                sum += (numeralToValueTable [numerals[0]] < numeralToValueTable [numerals[1]]) ? -numeralToValueTable [numerals[0]] : numeralToValueTable [numerals[0]];
                                // add the remaining numerals
                                sum += numeralToValueTable [numerals[1]] * (numerals.Count - 1);
                            }
                            break;
                    }
                }
            }
            return sum;
        }

        private static Dictionary<char, int> BuildLookupTable(out char[] numerals)
        {
            var lookupTable= new Dictionary<char, int>();
            // this implementation assumes the 5's-10s pattern will repeat when/if the alphabet changes.
            int[] vals =         {  1,   5,  10,  50,  100, 500, 1000 };
            numerals = new char[]{ 'I', 'V', 'X', 'L', 'C', 'D', 'M' };
            // build the base value lookup table lt
            for (int i = 0; i < vals.Length; i++) lookupTable.Add(numerals[i], vals[i]);
            return lookupTable;
        }

        private static string MakeNumberParser(char[] N)
        {
            // technically we should use stringbuilder for this... it's better performing.
            // one item you'll notice different in the regex pattern is it's very flat, and only uses alternation within the optional groups.
            string re = "";
            for (int i= N.Length-1; i>=0;i--)
            {
                if (i==0)
                    //< ones only occur in sequence up to 3
                    re += string.Format("({0}{{1,3}})?", N[0]); 
                else if (i % 2 == 0)
                    // captures CMM,MCM, MMCM and other subtractive formats
                    // if we can stack more than 3, we will need to refactor this regex template. 
                    // come up with a way of programmatically come up with the variations.
                    re += string.Format("({0}{1}{1}?{1}?|{1}{0}{1}{1}?|{1}?{1}{0}{1}|{1}{{1,3}})?", N[i - 2], N[i]); 
                else if (i % 2 == 1)
                    // fives based subtractive
                    re += string.Format("({0}?{1}{{1,1}})?", N[i - 1], N[i]);
            }

            return "^" +re+"$";
        }
    }
}
