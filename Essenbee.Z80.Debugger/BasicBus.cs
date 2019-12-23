﻿using System;
using System.Collections.Generic;

namespace Essenbee.Z80.Debugger
{
    public class BasicBus : IBus
    {
        public bool Interrupt { get; set; }
        public bool NonMaskableInterrupt { get; set; }
        public IEnumerable<byte> Data { get; set; } = new List<byte>();

        private byte[] _memory;
        public BasicBus(int RAMSize)
        {
            _memory = new byte[RAMSize * 1024];
        }

        public BasicBus(byte[] ram)
        {
            _memory = ram;
        }

        public IReadOnlyCollection<byte> RAM
        {
            get => _memory;
        }

        public byte Read(ushort addr, bool ro = false)
        {
            return _memory[addr];
        }

        public byte ReadPeripheral(ushort port)
        {
            throw new NotImplementedException();
        }

        public void Write(ushort addr, byte data)
        {
            _memory[addr] = data;
        }

        public void WritePeripheral(ushort port, byte data)
        {
            throw new NotImplementedException();
        }
    }
}
