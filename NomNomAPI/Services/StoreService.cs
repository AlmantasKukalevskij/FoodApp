using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NomNomAPI.Models;

namespace NomNomAPI.Services
{
    public class StoreService : IStoreService
    {
        private static List<Store> stores = new List<Store>
        {
            new Store
            {
                Id = 1,
                Name = "Crustum (Ukmergės g.)",
                ImageUrl = "https://localhost:7299/images/bakery.jpg",
                LogoUrl = "https://localhost:7299/images/images.png",
                Description = "Delicious pizza, appetizing snacks, sweets, and much more.",
                Distance = "2.5 km",
                Rating = 4.2,
                StartingPrice = "Starting at: €3.99"
            },
            new Store
            {
                Id = 2,
                Name = "Kepykla bubu (Savanoriu g.)",
                ImageUrl = "https://localhost:7299/images/bakery.jpg",
                LogoUrl = "https://localhost:7299/images/images.png",
                Description = "Delicious pizza, appetizing snacks, sweets, and much more.",
                Distance = "5 km",
                Rating = 4.2,
                StartingPrice = "Starting at: €3.99"
            }
        };

        public async Task<List<Store>> GetAllStores()
        {
            return await Task.FromResult(stores);
        }

        public async Task<Store> GetStoreById(int id)
        {
            var store = stores.FirstOrDefault(s => s.Id == id);
            return await Task.FromResult(store);
        }

        public async Task<Store> CreateStore(Store store)
        {
            store.Id = stores.Any() ? stores.Max(s => s.Id) + 1 : 1;
            stores.Add(store);
            return await Task.FromResult(store);
        }

        public async Task<Store> UpdateStore(int id, Store storeUpdate)
        {
            var store = stores.FirstOrDefault(s => s.Id == id);
            if (store != null)
            {
                store.Name = storeUpdate.Name;
                store.ImageUrl = storeUpdate.ImageUrl;
                store.LogoUrl = storeUpdate.LogoUrl;
                store.Description = storeUpdate.Description;
                store.Distance = storeUpdate.Distance;
                store.Rating = storeUpdate.Rating;
                store.StartingPrice = storeUpdate.StartingPrice;
            }
            return await Task.FromResult(store);
        }

        public async Task<bool> DeleteStore(int id)
        {
            var store = stores.FirstOrDefault(s => s.Id == id);
            if (store != null)
            {
                stores.Remove(store);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
