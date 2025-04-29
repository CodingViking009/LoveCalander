using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;

namespace CalanderTest
{
    public class ViewModel : BindableBase, IApplicationLifecycleAware
    {
        private Model m;
        private DateTime _selectedDate;
        private Visibility _panelVisibility;
        private Visibility _buttonVisibility;
        private Visibility _imageVisibility;
        private Visibility _altTextVisibility;
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

        public Visibility ImageVisibility
        {
            get => _imageVisibility;
            set => SetProperty(ref _imageVisibility, value);
        }

        public Visibility AltTextVisibility
        {
            get => _altTextVisibility;
            set => SetProperty(ref _altTextVisibility, value);
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
            m.LoadGalleryData();
            _photos = new ObservableCollection<ImageSource>();
            SelectedDate = DateTime.Now;
            PanelVisibility = Visibility.Collapsed;
            ButtonVisibility = Visibility.Visible;
            ImageVisibility = Visibility.Collapsed;
            AltTextVisibility = Visibility.Visible;
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
            if (m.ImageDictionary.ContainsKey(SelectedDate))
            {
                AltTextVisibility = Visibility.Collapsed;
                ImageVisibility = Visibility.Visible;
                Photos = m.ImageDictionary[SelectedDate];
            }
            else
            {
                ImageVisibility = Visibility.Collapsed;
                AltTextVisibility = Visibility.Visible;
            }
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
                if (!m.ImageDictionary.ContainsKey(SelectedDate))
                {
                    m.ImageDictionary[SelectedDate] = new ObservableCollection<ImageSource>();
                }

                foreach (string fileName in openFileDialog.FileNames)
                {
                    var bitmap = new BitmapImage(new Uri(fileName));
                    m.ImageDictionary[SelectedDate].Add(bitmap);
                }
                AltTextVisibility = Visibility.Collapsed;
                ImageVisibility = Visibility.Visible;
                Photos = m.ImageDictionary[SelectedDate];
            }
        }

        public void OnResume()
        {
            throw new NotImplementedException();
        }

        public void OnSleep()
        {
            m.SaveGalleryData();
        }
    }
}