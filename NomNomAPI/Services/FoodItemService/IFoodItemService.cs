using Microsoft.AspNetCore.Mvc;

namespace NomNomAPI.Services.FoodItemsService
{
    public interface IFoodItemService
    {
        Task<List<FoodItem>> GetAllFoods();
        Task<List<FoodItem>> GetFoodsByName(string name);
        Task<FoodItem?> GetSingleFood(int id);
        Task<List<FoodItem>> AddFood(FoodItem food);
        Task<List<FoodItem>?> UpdateFood(int id, FoodItem request);
        Task<List<FoodItem>?> DeleteFood(int id);
        Task<List<FoodItem>> GetAllFoodByStoreId(int storeId);
        Task<List<FoodItem>> GetFoodItemsByPriceRange(double minPrice, double maxPrice);


    }
}
