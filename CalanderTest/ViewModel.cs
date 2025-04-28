using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace CalanderTest
{
    public class ViewModel : BindableBase
    {
        private Model m;
        private DateTime _selectedDate;
        private Visibility _panelVisibility;
        private Visibility _buttonVisibility;
        private ObservableCollection<string> _dateText;
        private ObservableCollection<ImageSource> _photos;
        private string _dateTextIn;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public Visibility PanelVisibility
        {
            get => _panelVisibility;
            set => SetProperty(ref _panelVisibility, value);
        }

        public Visibility ButtonVisibility
        {
            get => _buttonVisibility;
            set => SetProperty(ref _buttonVisibility, value);
        }

        public ObservableCollection<string> DateText
        {
            get => _dateText;
            set => SetProperty(ref _dateText, value);
        }

        public ObservableCollection<ImageSource> Photos
        {
            get => _photos;
            set => SetProperty(ref _photos, value);
        }

        public string DateTextIn
        {
            get => _dateTextIn;
            set => SetProperty(ref _dateTextIn, value);
        }

        public ICommand EnterComm { get; }
        public ICommand LeaveComm { get; }
        public ICommand AddPhotoComm { get; }

        public ViewModel()
        {
            m = new Model();
            _photos = new ObservableCollection<ImageSource>();
            SelectedDate = DateTime.Now;
            PanelVisibility = Visibility.Collapsed;
            ButtonVisibility = Visibility.Visible;
            EnterComm = new DelegateCommand(Enter);
            LeaveComm = new DelegateCommand(Leave);
            AddPhotoComm = new DelegateCommand(AddPhoto);
        }

        public void Enter()
        {
            PanelVisibility = Visibility.Visible;
            ButtonVisibility = Visibility.Collapsed;
            DateText = m.MemoryDictionary.ContainsKey(SelectedDate)
                ? m.MemoryDictionary[SelectedDate]
                : new ObservableCollection<string> { "No data available" };
        }

        public void Leave()
        {
            PanelVisibility = Visibility.Collapsed;
            ButtonVisibility = Visibility.Visible;
            if (DateText.Contains("No data available") && !String.IsNullOrWhiteSpace(DateTextIn))
            {
                m.MemoryDictionary.Add(SelectedDate, new ObservableCollection<string> { DateTextIn });
            }
            else if (DateTextIn == "")
            {
                MessageBox.Show("You did not enter any text");
            }
            else if (m.MemoryDictionary.ContainsKey(SelectedDate))
            {
                m.MemoryDictionary[SelectedDate].Add(DateTextIn);
            }

            DateTextIn = "";
        }

        public void AddPhoto()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Photos",
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    var bitmap = new BitmapImage(new Uri(fileName));
                    Photos.Add(bitmap);
                }
            }
        }
    }
}