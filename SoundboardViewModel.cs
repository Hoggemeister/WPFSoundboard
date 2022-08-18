﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace WPFSoundboard
{
    public class SoundboardViewModel : ViewModelBase
    {
        public static readonly string CONFIG_NAME = "_Name";
        public static readonly string CONFIG_REPEAT = "_Repeat";
        public static readonly string CONFIG_PLAYCOUNT = "_PlayCount";

        private string soundDirectory;
        private string soundDirectoryName;

        public string SoundDirectoryName
        {
            get { return soundDirectoryName; }
            set { soundDirectoryName = value; NotifyPropertyChanged(); }
        }

        public string SoundDirectory
        {
            get { return soundDirectory; }
            set { soundDirectory = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<SoundItem> SoundItems { get; set; } = new ObservableCollection<SoundItem>();


        public SoundboardViewModel(string directory, int randomCount)
        {
            this.SoundDirectory = directory;
          

            List<SoundItem> tempItems = new List<SoundItem>();

            string[] files = Directory.GetFiles(directory, "*.mp3", randomCount > 0 ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            this.SoundDirectoryName = $"{new DirectoryInfo(directory).Name} ({files.Length})";

            if (randomCount > 0)
            {
                Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                int minVal = Math.Min(files.Length, randomCount);

                List<int> randoms = new List<int>();

                while (randoms.Count < minVal)
                {
                    int nextRand = rand.Next(0, files.Length);

                    if (randoms.Contains(nextRand) == false)
                    {
                        randoms.Add(nextRand);
                    }
                }

                foreach (int index in randoms)
                {
                    tempItems.Add(getNewSoundItemFromFile(files[index], true));
                }
            }
            else
            {
                foreach (string file in files)
                {
                    tempItems.Add(getNewSoundItemFromFile(file, false));
                }
            }

            foreach (SoundItem item in tempItems/*.OrderByDescending(item => item.Playcount)*/)
            {
                SoundItems.Add(item);
            }
        }

        private SoundItem getNewSoundItemFromFile(string file, bool random)
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            string id = filename.Replace(' ', '#');

            bool repeat;
            try { repeat = Convert.ToBoolean(ConfigurationManager.AppSettings.Get($"{id}{CONFIG_REPEAT}")); }
            catch (Exception) { repeat = false; }

            string name = "";
            try { name = ConfigurationManager.AppSettings.Get($"{id}{CONFIG_NAME}"); }
            catch (Exception) { }
            finally
            {
                if (string.IsNullOrEmpty(name))
                    name = filename;
            }

            int count;
            try { count = Convert.ToInt32(ConfigurationManager.AppSettings.Get($"{id}{CONFIG_PLAYCOUNT}")); }
            catch (Exception) { count = 0; }

            return new SoundItem(id, name, file, repeat, count, random);
        }

        public SoundboardViewModel()
        {
            this.SoundDirectoryName = "[NoName]";
        }
    }
}
