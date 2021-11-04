using OsuRTDataProviderLibrary.BeatmapInfo;
using OsuRTDataProviderLibrary.Listen;
using OsuRTDataProviderLibrary.Mods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuiDemo
{
    public partial class MainForm : Form
    {
        private SynchronizationContext Context;

        public MainForm()
        {
            InitializeComponent();
            Context = SynchronizationContext.Current;
            if(Context == null)
            {
                Context = new SynchronizationContext();
            }
        }

        protected override void OnLoad(EventArgs e)
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
            manager.Start();
        }


        private void OnScoreChanged(int score)
        {
            Context.Post(delegate (object obj)
            {
                Score.Text = score.ToString();
            }, null);
        }

        private void OnPlayModeChanged(OsuPlayMode last, OsuPlayMode mode)
        {
            Console.WriteLine($"PlayMode: last: {last} => current: {mode}");
            Context.Post(delegate (object obj)
            {
                PlayModeLast.Text = last.ToString();
                PlayModeCurrent.Text = mode.ToString();
            }, null);
        }

        private void OnPlayingTimeChanged(int ms)
        {
            Context.Post(delegate (object obj)
            {
                PlayingTime.Text = ms + "ms";
            }, null);
        }

        private void OnPlayerChanged(string player)
        {
            Context.Post(delegate (object obj)
            {
                Player.Text = player;
            }, null);
        }

        private void OnModsChanged(ModsInfo mods)
        {
            Context.Post(delegate (object obj)
            {
                Mods.Text = mods.ToString();
            }, null);
        }

        private void OnHitEventsChanged(PlayType playType, List<HitEvent> hitEvents)
        {
            Context.Post(delegate (object obj)
            {
                PlayType.Text = playType.ToString();
                HitEventsCount.Text = hitEvents.Count.ToString();
            }, null);
        }

        private void OnHealthPointChanged(double hp)
        {
            Context.Post(delegate (object obj)
            {
                HealthPoint.Text = hp.ToString();
            }, null);
        }

        private void OnErrorStatisticsChanged(ErrorStatisticsResult result)
        {
            Context.Post(delegate (object obj)
            {
                ErrorStatistics.Text = "UR:" + result.UnstableRate.ToString() + ", Min: " + result.ErrorMin + "Max: " + result.ErrorMax;
            }, null);
        }

        private void OnCountMissChanged(int hit)
        {
            Context.Post(delegate (object obj)
            {
                CountMiss.Text = hit.ToString();
            }, null);
        }

        private void OnCountKatuChanged(int hit)
        {
            Context.Post(delegate (object obj)
            {
                CountKatu.Text = hit.ToString();
            }, null);
        }

        private void OnCountGekiChanged(int hit)
        {
            Context.Post(delegate (object obj)
            {
                CountGeki.Text = hit.ToString();
            }, null);
        }

        private void OnCount50Changed(int hit)
        {
            Context.Post(delegate (object obj)
            {
                Count50.Text = hit.ToString();
            }, null);
        }

        private void OnCount300Changed(int hit)
        {
            Context.Post(delegate (object obj)
            {
                Count300.Text = hit.ToString();
            }, null);
        }

        private void OnCount100Changed(int hit)
        {
            Context.Post(delegate (object obj)
            {
                Count100.Text = hit.ToString();
            }, null);
        }

        private void OnComboChanged(int combo)
        {
            Context.Post(delegate (object obj)
            {
                Combo.Text = combo.ToString();
            }, null);
        }

        private void OnAccuracyChanged(double acc)
        {
            Context.Post(delegate (object obj)
            {
                Accuracy.Text = acc.ToString();
            }, null);
        }

        private void OnBeatmapChanged(Beatmap map)
        {
            Context.Post(delegate (object obj)
            {
                BeatmapPath.Text = map.FilenameFull;
            }, null);
        }
    }
}
