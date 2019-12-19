using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShareTheSpot.Models;
using Xamarin.Forms;

namespace ShareTheSpot.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>();

            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", image = GetImageAccordingToDevicePlatform("AUEB.jpg"), Description ="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", image = GetImageAccordingToDevicePlatform("AUEB.jpg"), Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", image = GetImageAccordingToDevicePlatform("AUEB.jpg"), Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", image = GetImageAccordingToDevicePlatform("AUEB.jpg"), Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", image = GetImageAccordingToDevicePlatform("AUEB.jpg"), Description="This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", image = GetImageAccordingToDevicePlatform("AUEB.jpg"), Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        private ImageSource GetImageAccordingToDevicePlatform(string imgUrl)
        {
            return Device.RuntimePlatform == Device.Android ? ImageSource.FromFile(imgUrl) :
                ImageSource.FromFile("Images/" + imgUrl);
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}