﻿using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFSoundboard
{
    public class SoundItem : ViewModelBase
    {
        private string file;
        private string name;
        private string id;
        private string playinfo;
        private int playcount;
        private bool repeat;
        private bool isPlaying;
        private DispatcherTimer timer;
        private WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

        public SoundItem(string id, string name, string file, bool repeat, int playcount)
        {
            this.id = id;
            this.Name = name;
            this.file = file;
            this.Repeat = repeat;
            this.Playcount = playcount;

            this.IsPlaying = false;
            _playSoundCommand = new DelegateCommand(OnPlaySound);
            _changeDataCommand = new DelegateCommand(OnChangeData);
            wplayer.PlayStateChange += Wplayer_PlayStateChange;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Playinfo = $"{wplayer.controls.currentPositionString}";
        }

        private void Wplayer_PlayStateChange(int NewState)
        {
            IsPlaying = NewState == (int)WMPLib.WMPPlayState.wmppsPlaying;
            OnPlayStateChanged(new PlayStateChangedEventArgs(IsPlaying, Name));
        }

        public void Play()
        {
            if (wplayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                timer.Stop();
                wplayer.controls.stop();
                Playinfo = "";
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    wplayer.URL = FilePath;
                    wplayer.settings.setMode("loop", Repeat);
                    wplayer.controls.play();
                    timer.Start();
                    Playcount++;
                    AddUpdateAppSettings($"{id}{Soundboard.CONFIG_PLAYCOUNT}", Playcount.ToString());
                }
            }
        }

        public string FilePath 
        {
            get { return file; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string ID
        {
            get { return id; }
        }

        public string Playinfo
        {
            get { return playinfo; }
            set
            {
                if (value != playinfo)
                {
                    playinfo = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool Repeat
        {
            get { return repeat; }
            set
            {
                if (value != repeat)
                {
                    repeat = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Playcount
        {
            get { return playcount; }
            set
            {
                if (value != playcount)
                {
                    playcount = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public bool IsPlaying
        {
            get { return isPlaying; }
            private set
            {
                if (value != isPlaying)
                {
                    isPlaying = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;

        #region Commands
        private readonly DelegateCommand _playSoundCommand;
        public ICommand PlaySoundCommand => _playSoundCommand;
        private void OnPlaySound(object commandParameter)
        {
            Play();
        }

        private readonly DelegateCommand _changeDataCommand;
        public ICommand ChangeDataCommand => _changeDataCommand;
        public void OnChangeData(object commandParameter)
        {
            InputDialog inputDialog = new InputDialog(Name, System.IO.Path.GetFileName(FilePath), Repeat);
            inputDialog.Owner = Application.Current.MainWindow;
            if (inputDialog.ShowDialog() == true)
            {
                bool check = inputDialog.chkRepeat.IsChecked ?? false;
                Name = inputDialog.newName;
                Repeat = check;

                AddUpdateAppSettings($"{id}{Soundboard.CONFIG_NAME}", inputDialog.newName);
                AddUpdateAppSettings($"{id}{Soundboard.CONFIG_REPEAT}", check.ToString());
            }
        }
        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        protected void OnPlayStateChanged(PlayStateChangedEventArgs e)
        {
            EventHandler<PlayStateChangedEventArgs> handler = PlayStateChanged;
            handler?.Invoke(this, e);
        }
        #endregion
    }
    public class PlayStateChangedEventArgs : EventArgs
    {
        public PlayStateChangedEventArgs(bool playState, string name)
        {
            PlayStateActive = playState;
        }
        public bool PlayStateActive { get; set; }
    }
}
