using OsuRTDataProviderLibrary;
using OsuRTDataProviderLibrary.BeatmapInfo;
using OsuRTDataProviderLibrary.Listen;
using OsuRTDataProviderLibrary.Memory;
using OsuRTDataProviderLibrary.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            OsuListenerManager manager = new OsuListenerManager();
            manager.OnBeatmapChanged += OnBeatmapChanged;
            manager.OnAccuracyChanged += OnAccuracyChanged;
            manager.OnComboChanged += OnComboChanged;
            manager.OnCount100Changed += OnCount100Changed;
            manager.OnCount300Changed += OnCount300Changed;
            manager.OnCount50Changed += OnCount50Changed;
            manager.OnCountGekiChanged += OnCountGekiChanged;
            manager.OnCountKatuChanged += OnCountKatuChanged;
            manager.OnCountMissChanged += OnCountMissChanged;
            manager.OnErrorStatisticsChanged += OnErrorStatisticsChanged;
            manager.OnHealthPointChanged += OnHealthPointChanged;
            manager.OnHitEventsChanged += OnHitEventsChanged;
            manager.OnModsChanged += OnModsChanged;
            manager.OnPlayerChanged += OnPlayerChanged;
            manager.OnPlayingTimeChanged += OnPlayingTimeChanged;
            manager.OnPlayModeChanged += OnPlayModeChanged;
            manager.OnScoreChanged += OnScoreChanged;
            manager.OnOsuInternalStatusChanged += OnOsuInternalStatusChanged;
            manager.Start();
            Console.ReadLine();
        }

        private static void OnOsuInternalStatusChanged(OsuInternalStatus status)
        {
            Console.WriteLine("OsuInternalStatus: " + status);
        }

        private static void OnScoreChanged(int obj)
        {
            //Console.WriteLine("Score: obj");
        }

        private static void OnPlayModeChanged(OsuPlayMode last, OsuPlayMode mode)
        {
            //Console.WriteLine($"PlayMode: last: {last} => current: {mode}");
        }

        private static void OnPlayingTimeChanged(int ms)
        {
            //Console.WriteLine($"PlayingTime: {ms}ms");
        }

        private static void OnPlayerChanged(string player)
        {
            //Console.WriteLine($"Player: {player}");
        }

        private static void OnModsChanged(ModsInfo mods)
        {
            //Console.WriteLine($"Mods: {mods}");
        }

        private static void OnHitEventsChanged(PlayType playType, List<HitEvent> hitEvents)
        {
            //Console.WriteLine($"PlayType: {playType}, HitEvents Count: {hitEvents.Count}");
        }

        private static void OnHealthPointChanged(double hp)
        {
            //Console.WriteLine($"HealthPoint: {hp}");
        }

        private static void OnErrorStatisticsChanged(ErrorStatisticsResult result)
        {
            //Console.WriteLine($"ErrorStatistics: {result}");
        }

        private static void OnCountMissChanged(int hit)
        {
            //Console.WriteLine($"Miss: {hit}");
        }

        private static void OnCountKatuChanged(int hit)
        {
            //Console.WriteLine($"Katu: {hit}");
        }

        private static void OnCountGekiChanged(int hit)
        {
            //Console.WriteLine($"Geki: {hit}");
        }

        private static void OnCount50Changed(int hit)
        {
            //Console.WriteLine($"50: {hit}");
        }

        private static void OnCount300Changed(int hit)
        {
            //Console.WriteLine($"300: {hit}");
        }

        private static void OnCount100Changed(int hit)
        {
            //Console.WriteLine($"100: {hit}");
        }

        private static void OnComboChanged(int combo)
        {
            //Console.WriteLine($"Combo: {combo}");
        }

        private static void OnAccuracyChanged(double acc)
        {
            //Console.WriteLine($"Accuracy: {acc}");
        }

        private static void OnBeatmapChanged(Beatmap map)
        {
            Console.WriteLine("Beatmap Path: " + map.FilenameFull);
        }
    }
}
