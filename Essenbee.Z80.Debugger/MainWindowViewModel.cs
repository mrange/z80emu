﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using static Essenbee.Z80.Z80;

namespace Essenbee.Z80.Debugger
{
    public partial class MainWindowViewModel
    {
        private Z80 _cpu;
        private IBus _basicBus;
        private ushort _disassembleFrom;
        private ushort _disassembleTo;

        // ================== Construction Event ==================
        partial void Constructed()
        {
            _basicBus = new BasicBus(64);
            _cpu = new Z80 { PC = 0x8000 }; //Default start location
            _cpu.ConnectToBus(_basicBus);
            ProgramCounter = _cpu.PC.ToString("X4");
            StackPointer = _cpu.SP.ToString("X4");
            IndexX = _cpu.IX.ToString("X4");
            IndexY = _cpu.IY.ToString("X4");
            InterruptVector = _cpu.I.ToString("X2");
            Refresh = _cpu.R.ToString("X2");
            QRegister = ((byte)_cpu.Q).ToString("X2");
            MemPointer = _cpu.MEMPTR.ToString("X4");
            SetRegisterPairs();
            SetFlags();
            Mode = GetInterruptMode();

            Memory = BuildMemoryMap();
            MemoryMapRow = GetMemoryMapRow(_cpu.PC);
        }

        // ================== Property Events ==================
        partial void Changed_ProgramCounter(string prev, string current)
        {
            _cpu.PC = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber) ;
        }

        partial void Changed_AccuFlags(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.A = (byte)((temp & 0xFF00) >> 8);
            _cpu.F = (Flags)(temp & 0x00FF);
        }

        partial void Changed_AccuFlagsPrime(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.A1 = (byte)((temp & 0xFF00) >> 8);
            _cpu.F1 = (Flags)(temp & 0x00FF);
        }

        partial void Changed_HLPair(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.H = (byte)((temp & 0xFF00) >> 8);
            _cpu.L = (byte)(temp & 0x00FF);
        }

        partial void Changed_HLPairPrime(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.H1 = (byte)((temp & 0xFF00) >> 8);
            _cpu.L1 = (byte)(temp & 0x00FF);
        }

        partial void Changed_BCPair(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.B = (byte)((temp & 0xFF00) >> 8);
            _cpu.C = (byte)(temp & 0x00FF);
        }

        partial void Changed_BCPairPrime(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.B1 = (byte)((temp & 0xFF00) >> 8);
            _cpu.C1 = (byte)(temp & 0x00FF);
        }

        partial void Changed_DEPair(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.D = (byte)((temp & 0xFF00) >> 8);
            _cpu.E = (byte)(temp & 0x00FF);
        }

        partial void Changed_DEPairPrime(string prev, string current)
        {
            var temp = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            _cpu.D1 = (byte)((temp & 0xFF00) >> 8);
            _cpu.E1 = (byte)(temp & 0x00FF);
        }

        partial void Changed_StackPointer(string prev, string current)
        {
            _cpu.SP = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
            MemoryMapRow = GetMemoryMapRow(_cpu.PC);
        }

        partial void Changed_IndexX(string prev, string current)
        {
            _cpu.IX = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
        }

        partial void Changed_IndexY(string prev, string current)
        {
            _cpu.IY = ushort.Parse(current, System.Globalization.NumberStyles.HexNumber);
        }

        partial void Changed_InterruptVector(string prev, string current)
        {
            _cpu.I = byte.Parse(current, System.Globalization.NumberStyles.HexNumber);
        }

        partial void Changed_Refresh(string prev, string current)
        {
            _cpu.R = byte.Parse(current, System.Globalization.NumberStyles.HexNumber);
        }

        partial void Changed_SignBit(bool prev, bool current)
        {
            SetFlag(Flags.S, current);
        }

        partial void Changed_ZeroBit(bool prev, bool current)
        {
            SetFlag(Flags.Z, current);
        }

        partial void Changed_UBit(bool prev, bool current)
        {
            SetFlag(Flags.U, current);
        }

        partial void Changed_HalfCarryBit(bool prev, bool current)
        {
            SetFlag(Flags.H, current);
        }

        partial void Changed_XBit(bool prev, bool current)
        {
            SetFlag(Flags.X, current);
        }

        partial void Changed_ParityOverflowBit(bool prev, bool current)
        {
            SetFlag(Flags.P, current);
        }

        partial void Changed_NegationBit(bool prev, bool current)
        {
            SetFlag(Flags.N, current);
        }

        partial void Changed_CarryBit(bool prev, bool current)
        {
            SetFlag(Flags.C, current);
        }

        // ================== Command Events ==================
        partial void CanExecute_StepCommand(ref bool result)
        {
            result = _cpu != null;
        }

        partial void Execute_StepCommand()
        {
            _cpu.Step();
            SelectedRow = _cpu.PC.ToString("X4");
            Memory = BuildMemoryMap();
            ProgramCounter = _cpu.PC.ToString("X4");
            MemoryMapRow = GetMemoryMapRow(_cpu.PC);
            StackPointer = _cpu.SP.ToString("X4");
            IndexX = _cpu.IX.ToString("X4");
            IndexY = _cpu.IY.ToString("X4");
            InterruptVector = _cpu.I.ToString("X2");
            Refresh = _cpu.R.ToString("X2");
            QRegister = ((byte)_cpu.Q).ToString("X2");
            MemPointer = _cpu.MEMPTR.ToString("X4");
            Mode = GetInterruptMode();
            SetRegisterPairs();
            SetFlags();
        }

        partial void CanExecute_LoadCommand(ref bool result)
        {
            result = _cpu != null;
        }

        partial void Execute_LoadCommand()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "HEX file (*.hex)|*.hex"
            };

            var result = openFileDialog.ShowDialog();

            if (result ?? false)
            {
                var fileName = openFileDialog.FileName;
                var (RAM, startAddr, endAddr) = HexFileLoader.Read(fileName, new byte[64 * 1024]);
                _basicBus = new BasicBus(RAM);
                _cpu.ConnectToBus(_basicBus);
                Memory = BuildMemoryMap();
                _cpu.PC = startAddr;
                ProgramCounter = _cpu.PC.ToString("X4");
                MemoryMapRow = GetMemoryMapRow(startAddr);
                var disassembly = _cpu.Disassemble(startAddr, endAddr);
                DisAsm = GetDisassembedProgram(disassembly);
                SelectedRow = startAddr.ToString("X4");
            }
        }

        partial void CanExecute_LoadRomCommand(ref bool result)
        {
            result = _cpu != null;
        }

        partial void Execute_LoadRomCommand()
        {
            ushort startAddr = 0x000;
            
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "ROM file (*.rom)|*.rom"
            };

            var result = openFileDialog.ShowDialog();

            if (result ?? false)
            {
                var RAM = new byte[65536];
                Array.Clear(RAM, 0, RAM.Length);
                var fileName = openFileDialog.FileName;
                var rom = File.ReadAllBytes(fileName);
                var dataBlocks = new List<(ushort, ushort)>(); 

                if (File.Exists($"{fileName}.data"))
                {
                    var romData = File.ReadAllLines($"{fileName}.data");

                    foreach (var line in romData)
                    {
                        var address = line.Split(',');
                        dataBlocks.Add((Convert.ToUInt16(address[0], 16), Convert.ToUInt16(address[1], 16)));
                    }
                }

                var endAddr = (ushort)rom.Length;
                Array.Copy(rom, RAM, rom.Length);

                _basicBus = new BasicBus(RAM);
                _cpu.ConnectToBus(_basicBus);
                Memory = BuildMemoryMap();
                _cpu.PC = startAddr;
                ProgramCounter = _cpu.PC.ToString("X4");

                MemoryMapRow = GetMemoryMapRow(startAddr);
                var disassembly = _cpu.Disassemble(startAddr, endAddr, dataBlocks);
                DisAsm = GetDisassembedProgram(disassembly);

                SelectedRow = startAddr.ToString("X4");
            }
        }

        private Dictionary<string, string> GetDisassembedProgram(Dictionary<ushort, string> disassembly)
        {
            var retVal = new Dictionary<string, string>();

            foreach (var line in disassembly)
            {
                var addr = line.Key.ToString("X4");
                retVal.Add(addr, line.Value);
            }

            return retVal;
        }

        private Dictionary<string, string> BuildMemoryMap()
        {
            var memory = _basicBus.RAM.Select((a, b) => new { a, b })
                                  .ToDictionary(mem => mem.b, mem => mem.a);
            var memoryMap = new Dictionary<string, string>();
            for (int i = 0; i < memory.Count; i += 16)
            {
                memoryMap[i.ToString("X4")] = memory[i].ToString("X2") + " ";

                for (int x = 1; x < 15; x++)
                {
                    memoryMap[i.ToString("X4")] += memory[i + x].ToString("X2") + " ";
                }

                memoryMap[i.ToString("X4")] += memory[i + 15].ToString("X2");
            }

            return memoryMap;
        }

        private int GetMemoryMapRow(int programCounter)
        {
            var row = programCounter / 16;
            return row;
        }

        private void SetFlags()
        {
            SignBit = CheckFlag(Flags.S);
            ZeroBit = CheckFlag(Flags.Z);
            UBit = CheckFlag(Flags.U);
            HalfCarryBit = CheckFlag(Flags.H);
            XBit = CheckFlag(Flags.X);
            ParityOverflowBit = CheckFlag(Flags.P);
            NegationBit = CheckFlag(Flags.N);
            CarryBit = CheckFlag(Flags.C);
        }

        private void SetRegisterPairs()
        {
            AccuFlags = _cpu.AF.ToString("X4");
            AccuFlagsPrime = _cpu.AF1.ToString("X4");
            HLPair = _cpu.HL.ToString("X4");
            HLPairPrime = _cpu.HL1.ToString("X4");
            BCPair = _cpu.BC.ToString("X4");
            BCPairPrime = _cpu.BC1.ToString("X4");
            DEPair = _cpu.DE.ToString("X4");
            DEPairPrime = _cpu.DE1.ToString("X4");
        }

        private int GetInterruptMode() => _cpu.InterruptMode switch
            {
                InterruptMode.Mode0 => 0,
                InterruptMode.Mode1 => 1,
                InterruptMode.Mode2 => 2,
                _ => 0
            };

        private bool CheckFlag(Flags flag)
        {
                if ((_cpu.F & flag) == flag)
                {
                    return true;
                }

            return false;
        }

        private void SetFlag(Flags flag, bool value)
        {
                if (value)
                {
                    _cpu.F |= flag;
                }
                else
                {
                    _cpu.F &= ~flag;
                }
        }
    }
}
