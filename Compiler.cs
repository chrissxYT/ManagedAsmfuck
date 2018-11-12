using System.IO;
using System.Collections.Generic;

namespace ManagedAsmfuck
{
    static class Compiler
    {
        public static void Compile(string input, string output)
        {
            string[] lines = File.ReadAllLines(input);
            List<byte> bytes = new List<byte>();
            foreach (string l in lines)
            {
                if(l == "nop")
                    bytes.Add(0);
                else if(l == "inc")
                    bytes.Add(1);
                else if(l == "dec")
                    bytes.Add(2);
                else if(l == "tsl")
                    bytes.Add(3);
                else if(l == "tsr")
                    bytes.Add(4);
                else if(l == "sjp")
                    bytes.Add(5);
                else if(l == "jpb")
                    bytes.Add(6);
                else if(l == "rac")
                    bytes.Add(7);
                else if(l == "wac")
                    bytes.Add(8);
                else
                    bytes.Add(0);
            }
            File.WriteAllBytes(output, bytes.ToArray());
        }

        public static void CompileBrainfuck(string input, string output)
        {
            Stream i = File.Open(input, FileMode.Open, FileAccess.Read);
                Stream o = File.Open(output, FileMode.Create, FileAccess.Write);
                int j;
                while ((j = i.ReadByte()) != -1)
                {
                    if (j == 43)
                        i.WriteByte(1);
                    else if (j == 45)
                        i.WriteByte(2);
                    else if (j == 60)
                        i.WriteByte(3);
                    else if (j == 62)
                        i.WriteByte(4);
                    else if (j == 91)
                        i.WriteByte(5);
                    else if (j == 93)
                        i.WriteByte(6);
                    else if (j == 44)
                        i.WriteByte(7);
                    else if (j == 46)
                        i.WriteByte(8);
                    else
                        i.WriteByte(0);
                }
                i.Close();
                o.Close();
        }
    }
}