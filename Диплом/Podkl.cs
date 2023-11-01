using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Диплом
{
    internal class Podkl
    {
        public RelayCommand OpenReportCommand { get; set; }
        public RelayCommand DeleteReportCommand { get; set; }
        private string FolderPath = @"TextReports/";
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        private void UpdateFilter(string s)
        {
            _listViewItemsFiltered.Clear();
            Task.Factory.StartNew(() =>
            {
                var requiredFiles = TextFiles.Where(p => p.Name.ToString().Contains(s)).ToList();
                return requiredFiles;
            }).ContinueWith(task =>
            {
                foreach (var result in task.Result)
                    _listViewItemsFiltered.Add(result);
                if (string.IsNullOrEmpty(s))
                {
                    UpdateView();
                }

            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void UpdateView()
        {
            ListViewItemsFiltered.Clear();
            var dinfo = new DirectoryInfo(FolderPath);

            var listOfFiles = dinfo.GetFiles(".", SearchOption.TopDirectoryOnly).ToList();
            foreach (var directory in listOfFiles)
            {
                _listViewItemsFiltered.Add(new ListViewItem() { Name = directory.Name, PathName = directory.FullName });
            }
        }
        public class ListViewItem
        {
            public string Name { get; set; }
            public string PathName { get; set; }
        }

        private readonly ObservableCollection<ListViewItem> _listViewItemsFiltered = new ObservableCollection<ListViewItem>();
        public ObservableCollection<ListViewItem> ListViewItemsFiltered => _listViewItemsFiltered;
        private readonly ObservableCollection<ListViewItem> _textFiles = new ObservableCollection<ListViewItem>();
        public ObservableCollection<ListViewItem> TextFiles => _textFiles;

        private ListViewItem _selectedFile;
        public ListViewItem SelectedFile
        {
            get => _selectedFile;
            set
            {
                _selectedFile = value;
                OnPropertyChanged();
            }
        }
        private string _filter = "";
        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged();
                UpdateFilter(_filter);
            }
        }
        public Podkl()
        {
            OpenReportCommand = new RelayCommand(o =>
            {
                var selectedPath = @"" + "TextReports/" + SelectedFile.Name;
                Process.Start("notepad.exe", selectedPath);
            }, o => ListViewItemsFiltered.Count > 0);
            DeleteReportCommand = new RelayCommand(o =>
            {
                try
                {
                    var selectedPath = @"" + "TextReports/" + SelectedFile.Name;
                    var result = MessageBox.Show("Удалить выбранный отчёт?", "Предупреждение",
                    MessageBoxButton.OKCancel);
                    if (result != MessageBoxResult.OK) return;
                    File.Delete(selectedPath);
                    foreach (var file in TextFiles)
                    {
                        if (file.Name == SelectedFile.Name)
                        {
                            TextFiles.Remove(file);
                            break;
                        }
                    }
                    MessageBox.Show("Отчёт удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
                UpdateFilter("");
            }, o => SelectedFile != null);
            var dinfo = new DirectoryInfo(FolderPath);
            var listOfFiles = dinfo.GetFiles(".", SearchOption.TopDirectoryOnly).ToList();
            foreach (var directory in listOfFiles)
            {
                TextFiles.Add(new ListViewItem() { Name = directory.Name, PathName = directory.FullName });
            }
            UpdateView();
        }
    }
}
