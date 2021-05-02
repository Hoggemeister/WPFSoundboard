using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;

namespace WPFSoundboard
{
    public class Soundboard : ViewModelBase
    {
        public static readonly string CONFIG_NAME = "_Name";
        public static readonly string CONFIG_REPEAT = "_Repeat";
        public static readonly string CONFIG_PLAYCOUNT = "_PlayCount";

        public ObservableCollection<SoundItem> itemList { get; set; } = new ObservableCollection<SoundItem>();

        public Soundboard()
        {
            List<SoundItem> tempItems = new List<SoundItem>();
            NameValueCollection sAll = ConfigurationManager.AppSettings;

            string[] files = System.IO.Directory.GetFiles(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Sounds"), "*.mp3");

            foreach (string file in files)
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                string id = filename.Replace(' ', '#');
                
                bool repeat;
                try { repeat = Convert.ToBoolean(sAll.Get($"{id}{CONFIG_REPEAT}")); }
                catch (Exception) { repeat = false; }
                
                string name = "";
                try { name = sAll.Get($"{id}{CONFIG_NAME}"); }
                catch (Exception) { }
                finally
                {
                    if (string.IsNullOrEmpty(name))
                        name = filename;
                }

                int count;
                try { count = Convert.ToInt32(sAll.Get($"{id}{CONFIG_PLAYCOUNT}")); }
                catch (Exception) { count = 0; }

                tempItems.Add(new SoundItem(id, name, file, repeat, count));
            }

            foreach (SoundItem item in tempItems)
                itemList.Add(item);
        }
    }
}
