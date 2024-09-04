using System.Collections.Generic;
using System.Threading.Tasks;
using NomNomAPI.Models;

namespace NomNomAPI.Services
{
    public interface IStoreService
    {
        Task<List<Store>> GetAllStores();
        Task<Store> GetStoreById(int id);
        Task<Store> CreateStore(Store store);
        Task<Store> UpdateStore(int id, Store store);
        Task<bool> DeleteStore(int id);
    }
}