using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ManagedAsmfuck.Consts;

namespace ManagedAsmfuck
{
    static class ASMVM
    {
        public static void Run(string file, int tapelen)
        {
            byte[] bin = File.ReadAllBytes(file);
            sbyte[] t = new sbyte[tapelen];
            int tp = startpos;
            int jp = -1;
            for (int ip = 0; ip < bin.Length; ip++)
            {
                byte b = bin[ip];
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

        public static (int, int) MovedTapeRange(List<byte> bin)
        {
            List<int> pos = new List<int>();
            int tp = int.MaxValue / 2;
            foreach (byte b in bin)
            {
                if (b == 1 || b == 2 || b == 7 || b == 8)
                    pos.Add(tp);
                else if (b == 3)
                    tp--;
                else if (b == 4)
                    tp++;
            }
            return (pos.Min(), pos.Max());
        }

        public static (int, int) ReadTapeRange(List<byte> bin)
        {
            List<int> pos = new List<int>();
            sbyte[] t = new sbyte[tapelen];
            int tp = startpos;
            int jp = -1;
            for (int ip = 0; ip < bin.Count; ip++)
            {
                byte b = bin[ip];
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
                {
                    ip = jp;
                    pos.Add(tp);
                }
                else if (b == 7)
                    t[tp] = 64; //the '@' sign is used as a debug symbol
                else if (b == 8)
                    pos.Add(tp);
            }
            return (pos.Min(), pos.Max());
        }

        public static (int, int) WriteTapeRange(List<byte> bin)
        {
            List<int> pos = new List<int>();
            sbyte[] t = new sbyte[tapelen];
            int tp = startpos;
            int jp = -1;
            for (int ip = 0; ip < bin.Count; ip++)
            {
                byte b = bin[ip];
                if (b == 1)
                    unchecked { t[tp]++; pos.Add(tp); }
                else if (b == 2)
                    unchecked { t[tp]--; pos.Add(tp); }
                else if (b == 3)
                    unchecked { tp--; }
                else if (b == 4)
                    unchecked { tp++; }
                else if (b == 5)
                    jp = ip;
                else if (b == 6 && t[tp] != 0)
                    ip = jp;
                else if (b == 7)
                {
                    t[tp] = 64; //the '@' sign is used as a debug symbol
                    pos.Add(tp);
                }
            }
            return (pos.Min(), pos.Max());
        }
    }
}
