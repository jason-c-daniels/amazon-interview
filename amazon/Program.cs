using System;

namespace amazon
{
    class Program
    {
        static void Main(string[] args)
        {
            Solution s = new Solution();
            bool correct=s.FromRoman("MLXVI")==1066;
            correct = s.FromRoman("MCMLXXI") == 1971;
            correct = s.FromRoman("MmCMLXXI") == 2971;
            correct = s.FromRoman("cmmmLXXI") == 2971;
            correct = s.FromRoman("McmMLXXI") == 2971;
            correct = s.FromRoman("CMMLXXI") == 1971;
            correct = s.FromRoman("CMMLXIX") == 1969;
            correct = s.FromRoman("XIV") == 14;
            correct = s.FromRoman("XV") == 15;
            correct = s.FromRoman("CCVII") == 207;
            correct = s.FromRoman("CCCVII") == 307;
            try { s.FromRoman(""); } catch { int i = 0; }
            try { s.FromRoman("IL"); } catch { int i = 0; }
            try { s.FromRoman("IM"); } catch { int i = 0; }
            try { s.FromRoman("XM"); } catch { int i = 0; }
            try { s.FromRoman("VV"); } catch { int i = 0; }
            try { s.FromRoman("DD"); } catch { int i = 0; }
            try { s.FromRoman("IC"); } catch { int i = 0; }

            Console.WriteLine("Hello World!");
        }
    }
}