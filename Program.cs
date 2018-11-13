﻿//instruction|description          |brainfuck operator|binary code
//nop        |no operation         |                  |0
//inc        |increment at tp      |+                 |1
//dec        |decrement at tp      |-                 |2
//tsl        |tape shift left      |<                 |3
//tsr        |tape shift right     |>                 |4
//sjp        |set jump position    |[                 |5
//jpb        |jmp to jp if bool    |]                 |6
//rac        |read ascii char      |,                 |7
//wac        |write ascii char     |.                 |8
//
//type   |var|name               |description
//sbyte[]|t  |tape               |the tape that's your memory
//int    |ip |instruction pointer|is equal to x86's eip and AMD64's rip; points to the current instruction in the loaded asmfuck binary
//int    |tp |tape pointer       |points to your position on the tape
//int    |jp |jump pointer       |the instruction to jump after on jpb

using System;
using System.IO;
using System.Collections.Generic;
using static ManagedAsmfuck.Consts;

namespace ManagedAsmfuck
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 ||
                (args.Length < 3 && args[0] != "runabin"))
            {
                Console.WriteLine("Usage: dotnet ManagedAsmfuck.dll [operation] [inputfile] {outputfile}");
                Console.WriteLine("Operations:");
                Console.WriteLine("    asm2bin");
                Console.WriteLine("    bin2asm");
                Console.WriteLine("    bin2elf");
                Console.WriteLine("    runabin");
                Console.WriteLine("    asm2bfk");
                Console.WriteLine("    bin2bfk");
                Console.WriteLine("    bfk2asm");
                Console.WriteLine("    bfk2bin");
                Console.WriteLine("    optimiz");
                Console.WriteLine("{outputfile} can be left out if the operation is runabin");
                Environment.Exit(1);
            }
            string input = args[1];
            string output = args.Length > 2 ? args[2] : "nope this is not here";
            if (args[0] == "asm2bin")
            {
                Compiler.Compile(input, output);
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
                        lines.Add(nop);
                }
                File.WriteAllLines(output, lines);
            }
            else if(args[0] == "bin2elf")
            {
                Console.WriteLine("Not implemented yet, because we haven't figured out how to compile an ELF in C#.");
            }
            else if(args[0] == "runabin")
            {
                ASMVM.Run(input, tapelen);
            }
            else if(args[0] == "asm2bfk")
            {
                string[] lines = File.ReadAllLines(input);
                List<byte> bytes = new List<byte>();
                foreach (string l in lines)
                {
                    if (l == "" || l == nop)
                        continue;
                    else if (l[0] == 'i' && l[1] == 'n' && l[2] == 'c')
                        bytes.Add(43);
                    else if(l[0] == 'd' && l[1] == 'e' && l[2] == 'c')
                        bytes.Add(45);
                    else if(l.StartsWith(tsl))
                        bytes.Add(60);
                    else if(l.StartsWith(tsr))
                        bytes.Add(62);
                    else if(l.StartsWith(sjp))
                        bytes.Add(91);
                    else if(l.StartsWith(jpb))
                        bytes.Add(93);
                    else if(l.StartsWith(rac))
                        bytes.Add(44);
                    else if(l.StartsWith(wac))
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
                File.WriteAllBytes(output, bts.ToArray());
            }
            else if(args[0] == "bfk2asm")
            {
                byte[] bytes = File.ReadAllBytes(input);
                List<string> s = new List<string>();
                foreach(byte b in bytes)
                {
                    if(b == 43)
                        s.Add(inc);
                    else if(b == 45)
                        s.Add(dec);
                    else if(b == 60)
                        s.Add(tsl);
                    else if(b == 62)
                        s.Add(tsr);
                    else if(b == 91)
                        s.Add(sjp);
                    else if(b == 93)
                        s.Add(jpb);
                    else if(b == 44)
                        s.Add(rac);
                    else if(b == 46)
                        s.Add(wac);
                    else
                        s.Add(nop);
                }
                File.WriteAllLines(output, s.ToArray());
            }
            else if(args[0] == "bfk2bin")
            {
                Compiler.CompileBrainfuck(input, output);
            }
            else if(args[0] == "optimiz")
            {
                Optimizer.Optimize(input, output);
            }
            else
            {
                Console.WriteLine("Unrecognized operation.");
                Environment.Exit(1);
            }
        }
    }
}
