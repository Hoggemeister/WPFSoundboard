using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var soundDir = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "Sounds");

            Directory.CreateDirectory(soundDir);

            var subDirs = Directory.GetDirectories(soundDir);

            foreach (string dir in subDirs)
            {
                if (!dir.Split(new char[] { '\\' }).Last().StartsWith("."))
                {
                    SoundGroups.Add(new SoundboardViewModel(dir));
                }
            }

            SelectedSoundBoard = SoundGroups.Any() ? SoundGroups.First() : null;

        }

    }
}
