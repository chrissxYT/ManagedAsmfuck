//instruction|description          |brainfuck operator|binary code
//nop        |no operation         |                  |0
//inc        |increment at tape pos|+                 |1
//dec        |decrement at tape pos|-                 |2
//tsl        |tape shift left      |<                 |3
//tsr        |tape shift right     |>                 |4
//sjp        |set jump position    |[                 |5
//jpb        |jmp to jp if bool    |]                 |6
//rac        |read ascii char      |,                 |7
//wac        |write ascii char     |.                 |8
//
//type   |var |name               |description
//sbyte[]|t   |tape               |the tape that's your memory
//int    |ip  |instruction pointer|is equal to x86's eip and AMD64's rip; points to the current instruction in the loaded asmfuck binary
//int    |tp  |tape pointer       |points to your position on the tape
//int    |jp  |jump pointer       |the instruction to jump after on jpb

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ManagedAsmfuck
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || (args.Length < 3 && args[0] != "runabin"))
            {
                Console.WriteLine("Usage: dotnet run ManagedAsmfuck.dll [operation] [inputfile] [outputfile]");
                Console.WriteLine("Operations:");
                Console.WriteLine("    asm2bin");
                Console.WriteLine("    bin2asm");
                Console.WriteLine("    bin2elf");
                Console.WriteLine("    runabin");
                Console.WriteLine("    asm2bfk");
                Console.WriteLine("    bin2bfk");
                Console.WriteLine("[outputfile] can be left out if the operation is runabin");
                Environment.Exit(1);
            }
            const string nop = "nop";
            const string inc = "inc";
            const string dec = "dec";
            const string tsl = "tsl";
            const string tsr = "tsr";
            const string sjp = "sjp";
            const string jpb = "jpb";
            const string rac = "rac";
            const string wac = "wac";
            const int tapelen = 50 * 1024 * 1024; // 50 MiB of tape length should be enough
            string input = args[1];
            string output = args.Length > 2 ? args[2] : "nope this is not here";
            if (args[0] == "asm2bin")
            {
                string[] lines = File.ReadAllLines(input);
                List<byte> bytes = new List<byte>();
                foreach (string l in lines)
                {
                    if(l == "")
                        continue;
                    else if(l == nop)
                        bytes.Add(0);
                    else if(l == inc)
                        bytes.Add(1);
                    else if(l == dec)
                        bytes.Add(2);
                    else if(l == tsl)
                        bytes.Add(3);
                    else if(l == tsr)
                        bytes.Add(4);
                    else if(l == sjp)
                        bytes.Add(5);
                    else if(l == jpb)
                        bytes.Add(6);
                    else if(l == rac)
                        bytes.Add(7);
                    else if(l == wac)
                        bytes.Add(8);
                    else
                    {
                        Console.WriteLine($"Did not recognize instruction \"{l}\", replaced it with a nop.");
                        bytes.Add(0);
                    }
                }
                File.WriteAllBytes(output, bytes.ToArray());
            }
            else if (args[0] == "bin2asm")
            {
                byte[] bytes = File.ReadAllBytes(input);
                List<string> lines = new List<string>();
                foreach(byte b in bytes)
                {
                    if(b == 0)
                        lines.Add(nop);
                    else if(b == 1)
                        lines.Add(inc);
                    else if(b == 2)
                        lines.Add(dec);
                    else if(b == 3)
                        lines.Add(tsl);
                    else if(b == 4)
                        lines.Add(tsr);
                    else if(b == 5)
                        lines.Add(sjp);
                    else if(b == 6)
                        lines.Add(jpb);
                    else if(b == 7)
                        lines.Add(rac);
                    else if(b == 8)
                        lines.Add(wac);
                    else
                    {
                        Console.WriteLine($"Did not recognize binary code {b.ToString("B8")}, replaced it with a nop.");
                        lines.Add(nop);
                    }
                }
                File.WriteAllLines(output, lines);
            }
            else if(args[0] == "bin2elf")
            {
                Console.WriteLine("Not implemented yet, because we haven't figured out how to compile an ELF in C#.");
            }
            else if(args[0] == "runabin")
            {
                byte[] bytes = File.ReadAllBytes(input);
                sbyte[] t = new sbyte[tapelen];
                int tp = tapelen / 2;
                int jp = -1;
                for(int ip = 0; ip < bytes.Length; ip++)
                {
                    byte b = bytes[ip];
                    if(b == 1)
                        unchecked{t[tp]++;}
                    else if(b == 2)
                        unchecked{t[tp]--;}
                    else if(b == 3)
                        unchecked{tp--;}
                    else if(b == 4)
                        unchecked{tp++;}
                    else if(b == 5)
                        jp = ip;
                    else if(b == 6 && t[tp] != 0)
                        ip = jp;
                    else if(b == 7)
                        t[tp] = (sbyte)Console.ReadKey(true).KeyChar;
                    else if(b == 8)
                        Console.Write((char)t[tp]);
                }
            }
            else if(args[0] == "asm2bfk")
            {
                string[] lines = File.ReadAllLines(input);
                List<byte> bytes = new List<byte>();
                foreach (string l in lines)
                {
                    if(l == "" || l == nop)
                        continue;
                    else if(l == inc)
                        bytes.Add(43);
                    else if(l == dec)
                        bytes.Add(45);
                    else if(l == tsl)
                        bytes.Add(60);
                    else if(l == tsr)
                        bytes.Add(62);
                    else if(l == sjp)
                        bytes.Add(91);
                    else if(l == jpb)
                        bytes.Add(93);
                    else if(l == rac)
                        bytes.Add(44);
                    else if(l == wac)
                        bytes.Add(46);
                    else
                        Console.WriteLine($"Did not recognize instruction \"{l}\", replaced it with a nop.");
                }
                File.WriteAllBytes(output, bytes.ToArray());
            }
            else if(args[0] == "bin2bfk")
            {
                byte[] bytes = File.ReadAllBytes(input);
                List<byte> bts = new List<byte>();
                foreach(byte b in bytes)
                {
                    if(b == 0)
                        continue;
                    else if(b == 1)
                        bts.Add(43);
                    else if(b == 2)
                        bts.Add(45);
                    else if(b == 3)
                        bts.Add(60);
                    else if(b == 4)
                        bts.Add(62);
                    else if(b == 5)
                        bts.Add(91);
                    else if(b == 6)
                        bts.Add(93);
                    else if(b == 7)
                        bts.Add(44);
                    else if(b == 8)
                        bts.Add(46);
                    else
                        Console.WriteLine($"Did not recognize binary code {b.ToString("B8")}, replaced it with a nop.");
                }
                File.WriteAllBytes(output, bts);
            }
            else
            {
                Console.WriteLine("Unrecognized operation.");
                Environment.Exit(1);
            }
        }
    }
}
