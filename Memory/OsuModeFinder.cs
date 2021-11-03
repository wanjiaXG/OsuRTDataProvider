﻿using OsuRTDataProviderLibrary.Listen;
using System;
using System.Diagnostics;

namespace OsuRTDataProviderLibrary.Memory
{
    internal class OsuPlayModeFinder : OsuFinderBase
    {
        //85 ff 74 57? a1 ?? ?? ?? ?? 89 45 e4           

        private static readonly string s_mode_pattern = "\xEC\x57\x56\x53\x3B\x0D\x00\x00\x00\x00\x74\x60\x89\x0D";

        private static readonly string s_mode_mask = "xxxxxx????xxxx";

        private IntPtr m_mode_address;

        public OsuPlayModeFinder(Process process) : base(process)
        {
        }

        public override bool TryInit()
        {
            bool success = false;

            SigScan.Reload();
            {
                m_mode_address = SigScan.FindPattern(StringToByte(s_mode_pattern), s_mode_mask,6);
                Console.WriteLine($"Mode Address (0):0x{(int)m_mode_address:X8}");

                success = TryReadIntPtrFromMemory(m_mode_address, out m_mode_address);
                Console.WriteLine($"Mode Address (1):0x{(int)m_mode_address:X8}");
            }
            SigScan.ResetRegion();

            if (m_mode_address == IntPtr.Zero)
                success = false;

            return success;
        }

        public OsuPlayMode GetMode()
        {
            TryReadIntFromMemory(m_mode_address, out int val);
            return (OsuPlayMode)val;
        }
    }
}