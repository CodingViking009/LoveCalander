using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
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

        public void SaveGalleryData()
        {
            var galleryData = new List<MemoryImage>();

            var allData = ImageDictionary.Keys.Union(MemoryDictionary.Keys).Distinct();

            foreach (var data in allData)
            {
                foreach (var imageSource in ImageDictionary[data])
                {
                    if (imageSource is BitmapImage bitmapImage && bitmapImage.UriSource != null)
                    {
                        galleryData.Add(new MemoryImage
                        {
                            Date = data.Date,
                            Text = MemoryDictionary.ContainsKey(data) ? string.Join(", ", MemoryDictionary[data]) : "",
                            ImagePath = bitmapImage.UriSource.ToString()
                        });
                    }
                }
            }

            string json = JsonSerializer.Serialize(galleryData, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText("galleryData.json", json);
        }

        public void LoadGalleryData()
        {
            if (!File.Exists("galleryData.json"))
                return;

            var json = File.ReadAllText("galleryData.json");
            var galleryData = JsonSerializer.Deserialize<List<MemoryImage>>(json);

            if (galleryData == null)
                return;

            foreach (var item in galleryData)
            {
                if (!ImageDictionary.ContainsKey(item.Date))
                {
                    ImageDictionary[item.Date] = new ObservableCollection<ImageSource>();
                }
                if (!MemoryDictionary.ContainsKey(item.Date))
                {
                    MemoryDictionary[item.Date] = new ObservableCollection<string>();
                }

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(item.ImagePath, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                ImageDictionary[item.Date].Add(bitmap);
            }
        }

    }
}
