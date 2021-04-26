using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;

namespace WPFSoundboard
{
    public class Soundboard : ViewModelBase
    {
        public ObservableCollection<SoundItem> itemList { get; set; } = new ObservableCollection<SoundItem>();

        public Soundboard()
        {
            NameValueCollection sAll = ConfigurationManager.AppSettings;

            string[] files = System.IO.Directory.GetFiles(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Sounds"), "*.mp3");

            for (int i = 0; i < files.Length; ++i)
            {
                itemList.Add(new SoundItem(i, sAll.Get($"Name{i}") ?? System.IO.Path.GetFileNameWithoutExtension(files[i]), files[i], Convert.ToBoolean(sAll.Get($"Repeat{i}"))));
            }
        }
    }
}
