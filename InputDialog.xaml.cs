using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFSoundboard
{
    /// <summary>
    /// Interaktionslogik für InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
		public InputDialog(string name, string file, bool Repeat)
		{
			InitializeComponent();
			txtName.Text = name;
			txtFile.Text = file;
			chkRepeat.IsChecked = Repeat;
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			txtName.SelectAll();
			txtName.Focus();
		}
		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "Sound Files|*.mp3;*.wav;*.ogg;*.flac";
			if (openFileDialog.ShowDialog() == true)
				txtFile.Text = openFileDialog.FileName;
		}

		public string newName { get => txtName.Text; }
		public string newFile { get => txtFile.Text; }
	}
}
