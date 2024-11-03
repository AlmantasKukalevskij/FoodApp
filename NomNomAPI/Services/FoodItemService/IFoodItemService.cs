using Microsoft.AspNetCore.Mvc;

namespace NomNomAPI.Services.FoodItemsService
{
    public interface IFoodItemService
    {
        Task<IEnumerable<FoodItem>> GetAllFoods();
        Task<List<FoodItem>> GetFoodsByName(string name);
        Task<List<FoodItem>> AddFood(FoodItem food);
        Task<List<FoodItem>> GetAllFoodByStoreId(int storeId);
        Task<List<FoodItem>> GetFoodItemsByPriceRange(double minPrice, double maxPrice);
    }
}
