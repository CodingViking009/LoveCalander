using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalanderTest
{
    internal class Model
    {
        public Dictionary<DateTime,ObservableCollection<string>> MemoryDictionary { get; set; }
        public Dictionary<DateTime, ObservableCollection<ImageSource>> ImageDictionary { get; set; }

        public Model()
        {
            MemoryDictionary = new Dictionary<DateTime, ObservableCollection<string>>
            {
                // Initialize the dictionary with some sample data
                { new DateTime(2024, 12, 9), new ObservableCollection<string> { "Became Couple" } },
                { new DateTime(2025, 10, 2), new ObservableCollection<string> { "Sample text for 2nd October 2025" } },
                { new DateTime(2025, 10, 3), new ObservableCollection<string> { "Sample text for 3rd October 2025" } },
                { new DateTime(2025, 10, 4), new ObservableCollection<string> { "Sample text for 4th October 2025" } },
                { new DateTime(2025, 10, 5), new ObservableCollection<string> { "Sample text for 5th October 2025" } }
            };

            ImageDictionary = new Dictionary<DateTime, ObservableCollection<ImageSource>>
            {
                { new DateTime(2024, 12, 9), new ObservableCollection<ImageSource> { new BitmapImage(new Uri("C:/Users/Jannik/source/repos/CalanderTest/CalanderTest/bin/Debug/SampleImages/Sample5.png")) } },
                { new DateTime(2025, 10, 2), new ObservableCollection<ImageSource> { new BitmapImage(new Uri("C:/Users/Jannik/source/repos/CalanderTest/CalanderTest/bin/Debug/SampleImages/Sample1.png")) } },
                { new DateTime(2025, 10, 3), new ObservableCollection<ImageSource> { new BitmapImage(new Uri("C:/Users/Jannik/source/repos/CalanderTest/CalanderTest/bin/Debug/SampleImages/Sample2.png")) } },
                { new DateTime(2025, 10, 4), new ObservableCollection<ImageSource> { new BitmapImage(new Uri("C:/Users/Jannik/source/repos/CalanderTest/CalanderTest/bin/Debug/SampleImages/Sample3.png")) } },
                { new DateTime(2025, 10, 5), new ObservableCollection<ImageSource> { new BitmapImage(new Uri("C:/Users/Jannik/source/repos/CalanderTest/CalanderTest/bin/Debug/SampleImages/Sample4.png")) } }
            };

        }
    }
}
