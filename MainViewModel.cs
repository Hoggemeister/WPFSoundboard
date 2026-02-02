using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace WPFSoundboard
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<SoundboardViewModel> soundGroups = new ObservableCollection<SoundboardViewModel>();
        private SoundboardViewModel selectedSoundboard;

        public SoundboardViewModel SelectedSoundBoard
        {
            get { return selectedSoundboard; }
            set { selectedSoundboard = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<SoundboardViewModel> SoundGroups
        {
            get { return soundGroups; }
            set { soundGroups = value; NotifyPropertyChanged(); }
        }

        public MainViewModel()
        {
            SoundGroups.Add(new SoundboardViewModel());

            var soundDir = Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Sounds");
            Directory.CreateDirectory(soundDir);
            var subDirs = Directory.GetDirectories(soundDir);

            foreach (string dir in subDirs)
            {
                if (!dir.Split(new char[] { '\\' }).Last().StartsWith("."))
                {
                    SoundGroups.Add(new SoundboardViewModel(dir, 0));
                }
            }

            if(File.Exists(Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Random.txt")))
            {
                string[] randFolders = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Random.txt"));

                foreach(string folder in randFolders)
                {
                    string[] cfgSplit = folder.Split(new char[] { ',' });
                    if (Directory.Exists(cfgSplit[1]))
                    {
                        SoundGroups.Add(new SoundboardViewModel(cfgSplit[1], Convert.ToInt32(cfgSplit[0])));
                    }
                }
            }

            SelectedSoundBoard = SoundGroups.Any() ? SoundGroups.First() : null;
        }
    }
}
