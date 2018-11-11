using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ManagedAsmfuck
{
    static class ASMVM
    {
        public static void Run(string file, int tapelen)
        {
            byte[] bytes = File.ReadAllBytes(file);
            sbyte[] t = new sbyte[tapelen];
            int tp = tapelen / 2;
            int jp = -1;
            for (int ip = 0; ip < bytes.Length; ip++)
            {
                byte b = bytes[ip];
                if (b == 1)
                    unchecked { t[tp]++; }
                else if (b == 2)
                    unchecked { t[tp]--; }
                else if (b == 3)
                    unchecked { tp--; }
                else if (b == 4)
                    unchecked { tp++; }
                else if (b == 5)
                    jp = ip;
                else if (b == 6 && t[tp] != 0)
                    ip = jp;
                else if (b == 7)
                    t[tp] = (sbyte)Console.ReadKey(false).KeyChar;
                else if (b == 8)
                    Console.Write((char)t[tp]);
            }
        }
    }
}
