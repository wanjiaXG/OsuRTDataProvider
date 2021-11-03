using OsuRTDataProviderLibrary.BeatmapInfo;
using OsuRTDataProviderLibrary.Listen;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace OsuRTDataProviderLibrary.Memory
{
    internal class OsuBeatmapFinder : OsuFinderBase
    {
        private static readonly string s_beatmap_pattern = "\x74\x24\x8B\x0D\x0\x0\x0\x0\x85\xC9\x74\x1A";

        private static readonly string s_beatmap_mask = "xxxx????xxxx";

        private static readonly int s_beatmap_offset = 0xc4;
        private static readonly int s_beatmap_set_offset = 0xc8;

        private static readonly int s_beatmap_folder_offset = 0x74;
        private static readonly int s_beatmap_filename_offset = 0x8c;

        private int BeatmapAddressOffset { get; }
        private int BeatmapSetAddressOffset { get; }
        private int BeatmapFolderAddressOffset { get; }
        private int BeatmapFileNameAddressOffset { get; }

        private const int MAX_RETRY_COUNT = 10;

        private IntPtr m_beatmap_address;

        public OsuBeatmapFinder(Process osu) : base(osu)
        {
            BeatmapAddressOffset = s_beatmap_offset;
            BeatmapSetAddressOffset = s_beatmap_set_offset;
            BeatmapFolderAddressOffset = s_beatmap_folder_offset;
            BeatmapFileNameAddressOffset = s_beatmap_filename_offset;

            //兼容20190816以前的屙屎
            var cmp_ver20190816 = Utils.ConvertVersionStringToValue("20190816");

            //兼容20211014的屙屎（暂时不知道下个版本能否正常使用）
            var cmp_ver20211014 = Utils.ConvertVersionStringToValue("20211014");

            var currentVersion = Utils.ConvertVersionStringToValue(OsuListenerManager.OsuVersion);

            if (currentVersion < cmp_ver20190816)
            {
                Console.WriteLine("BeatmapAddressOffset and others -= 4 for osu!ver < 20190816");
                BeatmapAddressOffset -= 4;
                BeatmapSetAddressOffset -= 4;
                BeatmapFolderAddressOffset -= 4;
                BeatmapFileNameAddressOffset -= 4;
            }
            else if(currentVersion >= cmp_ver20211014)
            {
                Console.WriteLine("BeatmapFileNameAddressOffset += 4 for osu!ver >= 20211014");
                BeatmapFileNameAddressOffset += 4;
            }
        }

        public override bool TryInit()
        {
            bool success = false;
            SigScan.Reload();
            {
                //Find Beatmap ID Address
                m_beatmap_address = SigScan.FindPattern(StringToByte(s_beatmap_pattern), s_beatmap_mask, 4);
                Console.WriteLine($"Beatmap Base Address (0):0x{(int)m_beatmap_address:X8}");

                success = TryReadIntPtrFromMemory(m_beatmap_address, out m_beatmap_address);
                Console.WriteLine($"Beatmap Base Address (1):0x{(int)m_beatmap_address:X8}");
            }
            SigScan.ResetRegion();

            if (m_beatmap_address == IntPtr.Zero)
                success = false;

            return success;
        }

        public Beatmap GetCurrentBeatmap(int osu_id)
        {
            TryReadIntPtrFromMemory(m_beatmap_address, out IntPtr cur_beatmap_address);
            TryReadIntFromMemory(cur_beatmap_address + BeatmapAddressOffset, out int id);
            TryReadIntFromMemory(cur_beatmap_address + BeatmapSetAddressOffset, out int set_id);

            string filename = GetCurrentBeatmapFilename();
            string folder = GetCurrentBeatmapFolder();

            Beatmap beatmap = Beatmap.Empty;

            try
            {
                if (!(string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(folder)))
                {
                    string folder_full = Path.Combine(OsuListenerManager.SongsPath, folder);
                    string filename_full = Path.Combine(folder_full, filename);
                    using (var fs = File.OpenRead(filename_full))
                    {
                        beatmap = new Beatmap(osu_id, set_id, id, fs);
                    }
                }
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("------------- ORTDP(Exception)--------------- ");
                sb.AppendLine(e.ToString());
                sb.AppendLine("--------------ORTDP(Detail)-----------------");
                sb.AppendLine($"Songs Path:{OsuListenerManager.SongsPath}");
                sb.AppendLine($"Filename:{filename}");
                sb.AppendLine($"Folder:{folder}");
                sb.AppendLine($"BeatmapID:{id}");
                sb.AppendLine($"BeatmapSetID:{set_id}");
                sb.AppendLine("--------------------------------------------");

                Console.WriteLine(sb.ToString());
            }

            return beatmap;
        }

        #region Beatmap Info

        private string GetCurrentBeatmapFolder()
        {
            TryReadIntPtrFromMemory(m_beatmap_address, out var cur_beatmap_address);
            bool success = TryReadStringFromMemory(cur_beatmap_address + BeatmapFolderAddressOffset, out string str);
            if (!success) return "";
            return str;
        }

        private string GetCurrentBeatmapFilename()
        {
            TryReadIntPtrFromMemory(m_beatmap_address, out var cur_beatmap_address);
            bool success = TryReadStringFromMemory(cur_beatmap_address + BeatmapFileNameAddressOffset, out string str);
            if (!success) return "";
            return str;
        }

        #endregion Beatmap Info
    }
}
