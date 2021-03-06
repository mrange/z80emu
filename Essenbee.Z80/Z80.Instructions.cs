﻿using System;

namespace Essenbee.Z80
{
    public partial class Z80
    {
        // Z80 Instructions
        //
        // Note: The Z80 uses 252 out of the available 256 codes as single byte opcodes ("root instructions").
        // The four remaining codes are used extensively as opcode "prefixes":
        // 
        // - CB and ED enable extra instructions
        // - DD or FD selects IX+d or IY+d respectively (in some cases without the displacement d) in place of HL
        //
        // This means that we have to read another byte in order to determine the operation in these four cases.

        // ========================================
        // General Purpose Arithmetic/CPU Control
        // ========================================

        // Instruction   : DAA
        // Operation     : Conditionally adjusts A for BCD arithmetic.
        // Flags Affected: S,Z,H,P,C
        private byte DAA(byte opCode)
        {
            var t = 0;

            if (CheckFlag(Flags.H) || ((A & 0xF) > 9))
            {
                t++;
            }

            // Set the Carry flag here ...
            if (CheckFlag(Flags.C) || (A > 0x99))
            {
                t += 2;
                SetFlag(Flags.C, true);
            }

            // Determine the Half-carry Flag here...
            if (CheckFlag(Flags.N) && !CheckFlag(Flags.H))
            {
                SetFlag(Flags.H, false);
            }
            else
            {
                if (CheckFlag(Flags.N) && CheckFlag(Flags.H))
                {
                    SetFlag(Flags.H, (A & 0x0F) < 6);
                }
                else
                {
                    SetFlag(Flags.H, (A & 0x0F) >= 0x0A);
                }
            }

            // Add or subtract 6 to/from nibbles as required, to adjust A for BCD correctness...
            switch (t)
            {
                case 1:
                    A += CheckFlag(Flags.N) ? ((byte)0xFA) : ((byte)0x06); // -6:6
                    break;
                case 2:
                    A += CheckFlag(Flags.N) ? ((byte)0xA0) : ((byte)0x60); // -0x60:0x60
                    break;
                case 3:
                    A += CheckFlag(Flags.N) ? ((byte)0x9A) : ((byte)0x66); // -0x66:0x66
                    break;
            }

            // Other Flags
            SetFlag(Flags.S, ((A & 0x80) > 0) ? true : false);
            SetFlag(Flags.Z, A == 0);
            SetFlag(Flags.P, Parity(A));

            // Undocumented Flags
            SetFlag(Flags.X, ((A & 0x08) > 0) ? true : false); //Copy of bit 3
            SetFlag(Flags.U, ((A & 0x20) > 0) ? true : false); //Copy of bit 5
            SetQ();

            return 0;
        }

        // Instruction   : CPL
        // Operation     : A <- Ones Complement of A
        // Flags Affected: H,N
        private byte CPL(byte opCode)
        {
            A = (byte)~A;

            SetFlag(Flags.H, true);
            SetFlag(Flags.N, true);

            // Undocumented Flags
            SetFlag(Flags.X, ((A & 0x08) > 0) ? true : false); //Copy of bit 3
            SetFlag(Flags.U, ((A & 0x20) > 0) ? true : false); //Copy of bit 5
            SetQ();

            return 0;
        }

        // Instruction   : NEG
        // Operation     : A <- Twos Complement of A (negation)
        // Flags Affected: All
        private byte NEG(byte opCode)
        {
            var temp = A;
            A = (byte)~A;
            A++;

            SetFlag(Flags.N, true);
            SetFlag(Flags.S, ((A & 0x80) > 0) ? true : false);
            SetFlag(Flags.C, (temp != 0) ? true : false);
            SetFlag(Flags.P, (temp == 0x80) ? true : false);
            SetFlag(Flags.Z, (A == 0) ? true : false);
            SetFlag(Flags.H, ((temp & 0x0F) + (((~temp) + 1) & 0x0F) > 0xF) ? true : false);

            // Undocumented Flags
            SetFlag(Flags.X, ((A & 0x08) > 0) ? true : false); //Copy of bit 3
            SetFlag(Flags.U, ((A & 0x20) > 0) ? true : false); //Copy of bit 5
            SetQ();

            return 0;
        }

        // Instruction   : CCF
        // Operation     : Invert Carry Flag
        // Flags Affected: H,N,C
        private byte CCF(byte opCode)
        {
            var temp = CheckFlag(Flags.C);
            SetFlag(Flags.C, !temp);
            SetFlag(Flags.N, false); ;
            SetFlag(Flags.H, temp);

            // Undocumented Flags set as per Patrik Rak
            //` https://www.worldofspectrum.org/forums/discussion/41704/redirect/p1
            var x = (byte)((((byte)Q) ^ ((byte)F)) | A);
            SetFlag(Flags.X, ((x & 0x08) > 0) ? true : false);
            SetFlag(Flags.U, ((x & 0x20) > 0) ? true : false);

            SetQ();

            return 0;
        }

        // Instruction   : SCF
        // Operation     : Set Carry Flag
        // Flags Affected: H,N,C
        private byte SCF(byte opCode)
        {
            SetFlag(Flags.C, true);
            SetFlag(Flags.N, false); ;
            SetFlag(Flags.H, false);

            // Undocumented Flags set as per Patrik Rak
            //` https://www.worldofspectrum.org/forums/discussion/41704/redirect/p1
            var x = (byte)((((byte)Q) ^ ((byte)F)) | A);
            SetFlag(Flags.X, ((x & 0x08) > 0) ? true : false);
            SetFlag(Flags.U, ((x & 0x20) > 0) ? true : false);

            SetQ();

            return 0;
        }

        // Instruction    : NOP
        // Operation      : No Operation
        // Flags Affected : None
        private byte NOP(byte opCode) => 0;

        // Instruction    : HALT
        // Operation      : Execute NOPs until a subsequent interrupt or reset is received
        // Flags Affected : None
        // Notes          : The HALT instruction halts the Z80; it does not increase the PC so that the
        //                  instruction is re-executed, until a maskable or non-maskable interrupt is accepted.
        //                  Only then does the Z80 increase the PC again and continues with the next instruction.
        //                  During the HALT state, the HALT line is set. The PC is increased before the interrupt
        //                  routine is called.
        private byte HALT(byte opCode)
        {
            // ToDo: Figure this out!

            ResetQ();
            return 0;
        }

        // Instruction    : DI
        // Operation      : Disable maskable interrupts
        // Flags Affected : None
        private byte DI(byte opCode)
        {
            IFF1 = false;
            IFF2 = false;
            ResetQ();

            return 0;
        }

        // Instruction    : EI
        // Operation      : Enable maskable interrupts
        // Flags Affected : None
        // Notes          : Interrupts are not accepted immediately after an EI, but are accepted
        //                  after the next instruction.
        private byte EI(byte opCode)
        {
            // ToDo: only allow interrupts after the next instruction is executed
            IFF1 = true;
            IFF2 = true;
            ResetQ();

            return 0;
        }


        // Instruction    : IM0
        // Operation      : Set Interrupt Mode 0
        // Flags Affected : None
        // Notes          : In the maskable interrupt mode 0, an interrupting device places
        //                  an instruction on the data bus for execution by the Z80. The
        //                  instruction is normally a Restart (RST) instruction since this is
        //                  an efficient one byte call to any one of eight subroutines located
        //                  in the first 64 bytes of memory (each subroutine is 8 bytes long).
        //                  However, any instruction may be given to the Z80­. The first byte of
        //                  a multi-byte instruction is read during the interrupt acknowledge cycle.
        //                  Subsequent bytes are read in by a normal memory read sequence (the PC,
        //                  however, remains at its pre­-interrupt state and the user must ensure
        //                  that memory will not respond to these read sequences). When the
        //                  interrupt is recognized, further interrupts are automatically disabled
        //                  (IFF1 and IFF2 are false). Any time after the interrupt sequence begins,
        //                  EI can be executed, meaning that this subroutine itself can be interrupted. 
        //                  This process may continue to any level as  long as all pertinent data are 
        //                  saved and restored. A CPU reset will automatically set interrupt mode 0.

        private byte IM0(byte opCode)
        {
            InterruptMode = InterruptMode.Mode0;
            ResetQ();

            return 0;
        }

        // Instruction    : IM1
        // Operation      : Set Interrupt Mode 0
        // Flags Affected : None
        // Notes          : This maskable mode allows peripherals of minimal complexity interrupt 
        //                  access. In this respect, it is similar to the NMI interrupt except that
        //                  the CPU does an automatic CALL to location 0038H instead of 0066H. As in 
        //                  the NMI, the CPU automatically pushes the PC onto the Stack. Note that 
        //                  when doing programmed I/O, the CPU will ignore any data put onto the data 
        //                  bus during the interrupt acknowledge cycle.

        private byte IM1(byte opCode)
        {
            InterruptMode = InterruptMode.Mode1;
            ResetQ();

            return 0;
        }

        // Instruction    : IM2
        // Operation      : Set Interrupt Mode 0
        // Flags Affected : None
        // Notes          : The Z80­ supports an interrupt vectoring structure that allows a peripheral 
        //                  device to identify the starting location of an interrupt service routine. 
        //                  Mode 2 is the most powerful of the three maskable interrupt modes allowing an 
        //                  indirect call to any memory location by a single 8-bit vector supplied from 
        //                  a peripheral. In this mode a peripheral generating the interrupt places 
        //                  the vector on the data bus in response to an interrupt acknowledge. This vector 
        //                  then becomes the least significant 8-bits of the indirect pointer while the I 
        //                  register in the CPU provides the most significant 8 bits. This address in turn
        //                  points to an address in a vector table which is the starting address of the interrupt 
        //                  routine. Interrupt processing thus starts at an arbitrary 16-bit address allowing any 
        //                  location in memory to be the start of the service routine. Notice that since the 
        //                  vector is used to identify two adjacent bytes to form a 16-bit address, only 7 
        //                  bits are required for the vector and the least significant bit is is zero.

        private byte IM2(byte opCode)
        {
            InterruptMode = InterruptMode.Mode2;
            ResetQ();

            return 0;
        }

        // ========================================
        // Exchange, Block Transfer and Search Group
        // ========================================

        // Instruction    : EX DE, HL
        // Operation      : DE <--> HL
        // Flags Affected : None

        private byte EXDEHL(byte opCode)
        {
            var temp = DE;

            D = (byte)((HL & 0xFF00) >> 8);
            E = (byte)(HL & 0x00FF);

            H = (byte)((temp & 0xFF00) >> 8);
            L = (byte)(temp & 0x00FF);

            ResetQ();
            return 0;
        }

        // Instruction    : EX AF, AF'
        // Operation      : AF <--> AF'
        // Flags Affected : None

        private byte EXAFAF(byte opCode)
        {
            var temp = AF;

            A = (byte)((AF1 & 0xFF00) >> 8);
            F = (Flags)(AF1 & 0x00FF);

            A1 = (byte)((temp & 0xFF00) >> 8);
            F1 = (Flags)(temp & 0x00FF);

            ResetQ();
            return 0;
        }

        // Instruction    : EXX
        // Operation      : 
        // Flags Affected : None

        private byte EXX(byte opCode)
        {
            var temp = BC;

            B = (byte)((BC1 & 0xFF00) >> 8);
            C = (byte)(BC1 & 0x00FF);

            B1 = (byte)((temp & 0xFF00) >> 8);
            C1 = (byte)(temp & 0x00FF);

            temp = DE;

            D = (byte)((DE1 & 0xFF00) >> 8);
            E = (byte)(DE1 & 0x00FF);

            D1 = (byte)((temp & 0xFF00) >> 8);
            E1 = (byte)(temp & 0x00FF);

            temp = HL;

            H = (byte)((HL1 & 0xFF00) >> 8);
            L = (byte)(HL1 & 0x00FF);

            H1 = (byte)((temp & 0xFF00) >> 8);
            L1 = (byte)(temp & 0x00FF);

            ResetQ();

            return 0;
        }

        // Instruction    : EXSPHL
        // Operation      : H <--> (SP+1), L <--> (SP)
        // Flags Affected : None

        private byte EXSPHL(byte opCode)
        {
            var tempLo = ReadFromBus(SP);
            var tempHi = ReadFromBus((ushort)(SP + 1));

            WriteToBus(SP, L);
            WriteToBus((ushort)(SP + 1), H);

            H = tempHi;
            L = tempLo;

            MEMPTR = HL;

            return 0;
        }

        // Instruction    : EXSPIX
        // Operation      : IXh <--> (SP+1), IXl <--> (SP)
        // Flags Affected : None

        private byte EXSPIX(byte opCode)
        {
            var tempLo = ReadFromBus(SP);
            var tempHi = ReadFromBus((ushort)(SP + 1));

            var ixL = (byte)(IX & 0x00FF);
            var ixH = (byte)((IX & 0xFF00) >> 8);

            WriteToBus(SP, ixL);
            WriteToBus((ushort)(SP + 1), ixH);

            IX = (ushort)((tempHi << 8) + tempLo);

            MEMPTR = IX;

            return 0;
        }

        // Instruction    : EXSPIY
        // Operation      : IYh <--> (SP+1), IYl <--> (SP)
        // Flags Affected : None

        private byte EXSPIY(byte opCode)
        {
            var tempLo = ReadFromBus(SP);
            var tempHi = ReadFromBus((ushort)(SP + 1));

            var iyL = (byte)(IY & 0x00FF);
            var iyH = (byte)((IY & 0xFF00) >> 8);

            WriteToBus(SP, iyL);
            WriteToBus((ushort)(SP + 1), iyH);

            IY = (ushort)((tempHi << 8) + tempLo);

            MEMPTR = IY;

            return 0;
        }

        // Instruction    : LDI
        // Operation      : (DE) <- (HL); DE++; HL++; BC--
        // Flags Affected : H,P,N,U,X

        private byte LDI(byte opCode)
        {
            var n = ReadFromBus(HL);
            WriteToBus(DE, n);

            var temp = (byte)(n + A);

            IncRegisterPair(HL, 2);
            IncRegisterPair(DE, 1);
            IncRegisterPair(BC, 0, -1);

            SetFlag(Flags.H, false);
            SetFlag(Flags.N, false);
            SetFlag(Flags.P, BC != 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            SetQ();

            return 0;
        }

        // Instruction    : LDIR
        // Operation      : (DE) <- (HL); DE++; HL++; BC--
        // Flags Affected : H,P,N,U,X

        private byte LDIR(byte opCode)
        {
            var n = ReadFromBus(HL);
            WriteToBus(DE, n);

            var temp = (byte)(n + A);

            IncRegisterPair(HL, 2);
            IncRegisterPair(DE, 1);
            IncRegisterPair(BC, 0, -1);

            SetFlag(Flags.H, false);
            SetFlag(Flags.N, false);
            SetFlag(Flags.P, BC != 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            SetQ();

            byte extraTStates = 0;

            if (BC != 0)
            {
                PC--;
                PC--;

                extraTStates = 5;
                MEMPTR = (ushort)(PC + 1);
            }

            return extraTStates;
        }

        // Instruction    : LDD
        // Operation      : (DE) <- (HL); DE--; HL--; BC--
        // Flags Affected : H,P,N,U,X

        private byte LDD(byte opCode)
        {
            var n = ReadFromBus(HL);
            WriteToBus(DE, n);

            var temp = (byte)(n + A);

            IncRegisterPair(HL, 2, -1);
            IncRegisterPair(DE, 1, -1);
            IncRegisterPair(BC, 0, -1);

            SetFlag(Flags.H, false);
            SetFlag(Flags.N, false);
            SetFlag(Flags.P, BC != 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            SetQ();

            return 0;
        }

        // Instruction    : LDDR
        // Operation      : (DE) <- (HL); DE--; HL--; BC--
        // Flags Affected : H,P,N,U,X

        private byte LDDR(byte opCode)
        {
            var n = ReadFromBus(HL);
            WriteToBus(DE, n);

            var temp = (byte)(n + A);

            IncRegisterPair(HL, 2, -1);
            IncRegisterPair(DE, 1, -1);
            IncRegisterPair(BC, 0, -1);

            SetFlag(Flags.H, false);
            SetFlag(Flags.N, false);
            SetFlag(Flags.P, BC != 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            SetQ();

            byte extraTStates = 0;

            if (BC != 0)
            {
                PC--;
                PC--;

                extraTStates = 5;
                MEMPTR = (ushort)(PC + 1);
            }

            return extraTStates;
        }

        // Instruction    : CPI
        // Operation      : A - (HL); HL++; BC--
        // Flags Affected : H,P,N,U,X

        private byte CPI(byte opCode)
        {
            var n = ReadFromBus(HL);
            var diff = (sbyte)(A - n);

            IncRegisterPair(HL, 2);
            IncRegisterPair(BC, 0, -1);

            var carryFlag = CheckFlag(Flags.C);

            // Make use of helper method 
            SetComparisonFlags(n, diff);

            // Carry flag is not changed by CPI...
            SetFlag(Flags.C, carryFlag);

            SetFlag(Flags.N, true);
            SetFlag(Flags.P, BC != 0);
            SetFlag(Flags.Z, diff == 0);
                       
            var temp = A - n - (CheckFlag(Flags.H) ? 1 : 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            MEMPTR++;

            SetQ();

            return 0;
        }

        // Instruction    : CPIR
        // Operation      : A - (HL); HL++; BC--
        // Flags Affected : H,P,N,U,X
        private byte CPIR(byte opCode)
        {
            var n = ReadFromBus(HL);
            var diff = (sbyte)(A - n);

            IncRegisterPair(HL, 2);
            IncRegisterPair(BC, 0, -1);

            var carryFlag = CheckFlag(Flags.C);

            // Make use of helper method 
            SetComparisonFlags(n, diff);

            // Carry flag is not changed by CPI...
            SetFlag(Flags.C, carryFlag);

            SetFlag(Flags.N, true);
            SetFlag(Flags.P, BC != 0);
            SetFlag(Flags.Z, diff == 0);

            var temp = A - n - (CheckFlag(Flags.H) ? 1 : 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            SetQ();

            byte extraTStates = 0;

            if (BC != 0 && diff != 0)
            {
                PC--;
                PC--;

                extraTStates = 5;
                MEMPTR = (ushort)(PC + 1);
            }
            else
            {
                MEMPTR++;
            }

            return extraTStates;
        }

        // Instruction    : CPD
        // Operation      : A - (HL); HL--; BC--
        // Flags Affected : H,P,N,U,X

        private byte CPD(byte opCode)
        {
            var n = ReadFromBus(HL);
            var diff = (sbyte)(A - n);

            IncRegisterPair(HL, 2, -1);
            IncRegisterPair(BC, 0, -1);

            var carryFlag = CheckFlag(Flags.C);

            // Make use of helper method 
            SetComparisonFlags(n, diff);

            // Carry flag is not changed by CPI...
            SetFlag(Flags.C, carryFlag);

            SetFlag(Flags.N, true);
            SetFlag(Flags.P, BC != 0);
            SetFlag(Flags.Z, diff == 0);

            var temp = A - n - (CheckFlag(Flags.H) ? 1 : 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            MEMPTR--;

            SetQ();

            return 0;
        }

        // Instruction    : CPDR
        // Operation      : A - (HL); HL--; BC--
        // Flags Affected : H,P,N,U,X
        private byte CPDR(byte opCode)
        {
            var n = ReadFromBus(HL);
            var diff = (sbyte)(A - n);

            IncRegisterPair(HL, 2, -1);
            IncRegisterPair(BC, 0, -1);

            var carryFlag = CheckFlag(Flags.C);

            // Make use of helper method 
            SetComparisonFlags(n, diff);

            // Carry flag is not changed by CPI...
            SetFlag(Flags.C, carryFlag);

            SetFlag(Flags.N, true);
            SetFlag(Flags.P, BC != 0);
            SetFlag(Flags.Z, diff == 0);

            var temp = A - n - (CheckFlag(Flags.H) ? 1 : 0);

            // Undocumented Flags
            SetFlag(Flags.U, ((temp & 0x02) > 0) ? true : false); //Copy of bit 1 of temp
            SetFlag(Flags.X, ((temp & 0x08) > 0) ? true : false); //Copy of bit 3 of temp

            SetQ();

            byte extraTStates = 0;

            if (BC != 0 && diff != 0)
            {
                PC--;
                PC--;

                extraTStates = 5;
                MEMPTR = (ushort)(PC + 1);
            }
            else
            {
                MEMPTR--;
            }

            return extraTStates;
        }
    }
}
