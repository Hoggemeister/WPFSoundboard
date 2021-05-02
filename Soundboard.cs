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

            foreach (string file in files)
            {
                string filename = System.IO.Path.GetFileNameWithoutExtension(file);
                
                bool repeat = false;
                //try { repeat = Convert.ToBoolean(sAll.Get($"{filename}_Repeat")); }
                //catch (Exception) { repeat = false; }
                
                string name = filename;
                //try { name = sAll.Get($"{filename}_Name"); }
                //catch (Exception) { name = filename; }

                itemList.Add(new SoundItem(name, file, repeat));
            }
        }
    }
}
