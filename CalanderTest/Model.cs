using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalanderTest
{
    internal class Model
    {
        public Dictionary<DateTime, ObservableCollection<string>> MemoryDictionary { get; set; }
        public Dictionary<DateTime, ObservableCollection<ImageSource>> ImageDictionary { get; set; }

        public Model()
        {
            MemoryDictionary = new Dictionary<DateTime, ObservableCollection<string>>();
            ImageDictionary = new Dictionary<DateTime, ObservableCollection<ImageSource>>();
            LoadGalleryData();
        }

        public void SaveGalleryData()
        {
            var galleryData = new List<MemoryImage>();

            var allData = ImageDictionary.Keys.Union(MemoryDictionary.Keys).Distinct();

            foreach (var data in allData)
            {
                
                        galleryData.Add(new MemoryImage
                        {
                            Date = data.Date,
                            Text = new List<string> { MemoryDictionary.ContainsKey(data) ? string.Join(", ", MemoryDictionary[data]) : null},
                            ImagePath = ImageDictionary.ContainsKey(data)
                                ? ImageDictionary[data]
                                    .OfType<BitmapImage>()
                                    .Select(b => b.UriSource?.LocalPath ?? "")
                                    .ToList()
                                : new List<string>()
                        });
                    
            }

            string json = JsonSerializer.Serialize(galleryData, new JsonSerializerOptions { WriteIndented = true });
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


                foreach (var image in item.ImagePath)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(image, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    ImageDictionary[item.Date].Add(bitmap);
                }


                foreach (var text in item.Text)
                {
                    MemoryDictionary[item.Date].Add(text);
                }

            }
        }

    }
}
