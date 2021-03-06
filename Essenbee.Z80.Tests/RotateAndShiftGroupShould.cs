﻿using FakeItEasy;
using System.Collections.Generic;
using Xunit;


namespace Essenbee.Z80.Tests
{
    public class RotateAndShiftGroupShould
    {
        [Fact]
        private void UpdateAccumulatorCorrectlyForRLCA()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0x07 }, // RLCA
                { 0x0081, 0x00 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b10001000, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00010001, cpu.A);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateAccumulatorCorrectlyForRLA()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0x17 }, // RLA
                { 0x0081, 0x00 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b01110110, F = Z80.Flags.C, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b11101101, cpu.A);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 0
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateAccumulatorCorrectlyForRRCA()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0x0F }, // RRCA
                { 0x0081, 0x00 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b00010001, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10001000, cpu.A);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateAccumulatorCorrectlyForRRA()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0x1F }, // RRA
                { 0x0081, 0x00 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b11100001, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b01110000, cpu.A);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateRegisterCorrectlyForRLCC()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RLC C
                { 0x0081, 0x01 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { C = 0b10001000, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00010001, cpu.C);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRLCHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RLC (HL)
                { 0x0081, 0x06 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b10001000 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00010001, program[cpu.HL]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRLCIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // RLC (IX+3)
                { 0x0081, 0xCB },
                { 0x0082, 0x03 },
                { 0x0083, 0x06 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 }, // <- (IX)
                { 0x0192, 0x00 },
                { 0x0193, 0x00 },
                { 0x0194, 0b10001000 },
                { 0x0195, 0x00 },
                { 0x0196, 0x00 },
                { 0x0197, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x0191, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00010001, program[0x0194]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRLCIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // RLC (IY+3)
                { 0x0081, 0xCB },
                { 0x0082, 0x03 },
                { 0x0083, 0x06 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 }, // <- (IX)
                { 0x0192, 0x00 },
                { 0x0193, 0x00 },
                { 0x0194, 0b10001000 },
                { 0x0195, 0x00 },
                { 0x0196, 0x00 },
                { 0x0197, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x0191, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00010001, program[0x0194]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateRegisterCorrectlyForRLB()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RL B
                { 0x0081, 0x10 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { B = 0b10001111, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00011110, cpu.B);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRLHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RL (HL)
                { 0x0081, 0x16 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b10001111 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00011110, program[cpu.HL]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdatesRegisterCorrectlyWhenSLAR()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SLA L
                { 0x0081, 0x25 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { L = 0b10110001, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b01100010, cpu.L);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is set
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
        }

        private void UpdatesRegisterCorrectlyWhenSLAR2()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SLA E
                { 0x0081, 0x23 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { E = 0b01011001, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10110010, cpu.E);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is not set
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
        }

        [Fact]
        private void UpdatesLocationPointedToByHLCorrectlyWhenSLAHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SLA (HL)
                { 0x0081, 0x26 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b01011001 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10110010, program[cpu.HL]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is not set
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdatesRegisterCorrectlyWhenSRLR()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SRL D
                { 0x0081, 0x3A },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0x00 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { D = 0b10110001, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b01011000, cpu.D);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is set
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
        }

        [Fact]
        private void UpdatesLocationPointedToByHLCorrectlyWhenSRLHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SRL (HL)
                { 0x0081, 0x3E },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b01011001 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00101100, program[cpu.HL]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is set
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRLIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // RL (IX+2)
                { 0x0081, 0xCB },
                { 0x0082, 0x02 },
                { 0x0083, 0x16 },
                { 0x0084, 0x00 },

                { 0x018F, 0x00 }, // IX
                { 0x0190, 0x00 },
                { 0x0191, 0b10001111 }, // <- (IX+2)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x018F, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00011110, program[(ushort)(cpu.IX + 2)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRLIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // RL (IY+2)
                { 0x0081, 0xCB },
                { 0x0082, 0x02 },
                { 0x0083, 0x16 },
                { 0x0084, 0x00 },

                { 0x018F, 0x00 }, // IY
                { 0x0190, 0x00 },
                { 0x0191, 0b10001111 }, // <- (IY+2)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x018F, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00011110, program[(ushort)(cpu.IY + 2)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateRegisterCorrectlyForRRCR()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RRC A
                { 0x0081, 0x0F },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b00110001, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10011000, cpu.A);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateLocationCorrectlyForRRCHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RRC (HL)
                { 0x0081, 0x0E },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b00110001 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10011000, program[cpu.HL]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRRCIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // RRC (IX+2)
                { 0x0081, 0xCB },
                { 0x0082, 0x02 },
                { 0x0083, 0x0E },
                { 0x0084, 0x00 },

                { 0x018F, 0x00 }, // IX
                { 0x0190, 0x00 },
                { 0x0191, 0b00110001 }, // <- (IX+2)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x018F, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10011000, program[(ushort)(cpu.IX + 2)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRRCIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // RRC (IY+2)
                { 0x0081, 0xCB },
                { 0x0082, 0x02 },
                { 0x0083, 0x0E },
                { 0x0084, 0x00 },

                { 0x018F, 0x00 }, // IY
                { 0x0190, 0x00 },
                { 0x0191, 0b00110001 }, // <- (IY+2)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x018F, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10011000, program[(ushort)(cpu.IY + 2)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S); ;

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateRegisterCorrectlyForRRR()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // RR A
                { 0x0081, 0x1F },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b11011101, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b01101110, cpu.A);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateLocationCorrectlyForRRHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0x37 }, // SCF
                { 0x0081, 0xCB }, // RR (HL)
                { 0x0082, 0x1E },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b11011101 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();
            cpu.Step();

            Assert.Equal(0b11101110, program[cpu.HL]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRRIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // RR (IX+2)
                { 0x0081, 0xCB },
                { 0x0082, 0x02 },
                { 0x0083, 0x1E },
                { 0x0084, 0x00 },

                { 0x018F, 0x00 }, // IX
                { 0x0190, 0x00 },
                { 0x0191, 0b11011101 }, // <- (IX+2)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x018F, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b01101110, program[(ushort)(cpu.IX + 2)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationContentsCorrectlyForRRIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // RR (IY+2)
                { 0x0081, 0xCB },
                { 0x0082, 0x02 },
                { 0x0083, 0x1E },
                { 0x0084, 0x00 },

                { 0x018F, 0x00 }, // IY
                { 0x0190, 0x00 },
                { 0x0191, 0b11011101 }, // <- (IY+2)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x018F, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b01101110, program[(ushort)(cpu.IY + 2)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 1
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdatesLocationCorrectlyWhenSLAIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // SLA (IX+1)
                { 0x0081, 0xCB },
                { 0x0082, 0x01 },
                { 0x0083, 0x26 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 }, // <- (IX)
                { 0x0191, 0b01011001 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x0190, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10110010, program[(ushort)(cpu.IX + 1)]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is not set
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdatesLocationCorrectlyWhenSLAIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // SLA (IY+1)
                { 0x0081, 0xCB },
                { 0x0082, 0x01 },
                { 0x0083, 0x26 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 }, // <- (IY)
                { 0x0191, 0b01011001 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x0190, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b10110010, program[(ushort)(cpu.IY + 1)]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is not set
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdatesLocationCorrectlyWhenSRLIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // SRL (IX+1)
                { 0x0081, 0xCB },
                { 0x0082, 0x01 },
                { 0x0083, 0x3E },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 }, // <- (IX)
                { 0x0191, 0b01011001 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x0190, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00101100, program[(ushort)(cpu.IX + 1)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is set
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdatesLocationCorrectlyWhenSRLIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // SRL (IY+1)
                { 0x0081, 0xCB },
                { 0x0082, 0x01 },
                { 0x0083, 0x3E },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 }, // <- (IY)
                { 0x0191, 0b01011001 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x0190, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b00101100, program[(ushort)(cpu.IY + 1)]);
            Assert.True((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag is set
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateRegisterCorrectlyForSRAR()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SRA A
                { 0x0081, 0x2F },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);

            var cpu = new Z80() { A = 0b10111000, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b11011100, cpu.A);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 0
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);
        }

        [Fact]
        private void UpdateLocationCorrectlyForSRAHL()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xCB }, // SRA (HL)
                { 0x0081, 0x2E }, 
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b10111000 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b11011100, program[cpu.HL]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 0
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationCorrectlyForSRAIXD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xDD }, // SRA (IX+1)
                { 0x0081, 0xCB },
                { 0x0082, 0x01 },
                { 0x0083, 0x2E },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 }, // <- (IX)
                { 0x0191, 0b10111000 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IX = 0x0190, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b11011100, program[(ushort)(cpu.IX + 1)]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 0
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void UpdateLocationCorrectlyForSRAIYD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xFD }, // SRA (IY+1)
                { 0x0081, 0xCB },
                { 0x0082, 0x01 },
                { 0x0083, 0x2E },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 }, // <- (IY)
                { 0x0191, 0b10111000 },
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { IY = 0x0190, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b11011100, program[(ushort)(cpu.IY + 1)]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C); // Carry flag contains 0
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.True((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void AccumulatorAndLocationCorrectAfterRLD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xED }, // RLD
                { 0x0081, 0x6F },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b0011_0001 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { A = 0b0111_1010, H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b0111_0011, cpu.A);
            Assert.Equal(0b0001_1010, program[cpu.HL]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.True((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.False((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }

        [Fact]
        private void AccumulatorAndLocationCorrectAfterRRD()
        {
            var fakeBus = A.Fake<IBus>();

            var program = new Dictionary<ushort, byte>
            {
                // Program Code
                { 0x0080, 0xED }, // RRD
                { 0x0081, 0x67 },
                { 0x0082, 0x00 },
                { 0x0083, 0x00 },
                { 0x0084, 0x00 },

                { 0x0190, 0x00 },
                { 0x0191, 0b0010_0000 }, // <- (HL)
                { 0x0192, 0x00 },
            };

            A.CallTo(() => fakeBus.Read(A<ushort>._, A<bool>._))
                .ReturnsLazily((ushort addr, bool ro) => program[addr]);
            A.CallTo(() => fakeBus.Write(A<ushort>._, A<byte>._))
                .Invokes((ushort addr, byte data) => UpdateMemory(addr, data));

            var cpu = new Z80() { A = 0b1000_0100, H = 0x01, L = 0x91, PC = 0x0080 };
            cpu.ConnectToBus(fakeBus);

            cpu.Step();

            Assert.Equal(0b1000_0000, cpu.A);
            Assert.Equal(0b0100_0010, program[cpu.HL]);
            Assert.False((cpu.F & Z80.Flags.C) == Z80.Flags.C);
            Assert.False((cpu.F & Z80.Flags.N) == Z80.Flags.N);
            Assert.False((cpu.F & Z80.Flags.P) == Z80.Flags.P);
            Assert.False((cpu.F & Z80.Flags.X) == Z80.Flags.X);
            Assert.False((cpu.F & Z80.Flags.H) == Z80.Flags.H);
            Assert.False((cpu.F & Z80.Flags.U) == Z80.Flags.U);
            Assert.False((cpu.F & Z80.Flags.Z) == Z80.Flags.Z);
            Assert.True((cpu.F & Z80.Flags.S) == Z80.Flags.S);

            void UpdateMemory(ushort addr, byte data)
            {
                program[addr] = data;
            }
        }
    }
}
