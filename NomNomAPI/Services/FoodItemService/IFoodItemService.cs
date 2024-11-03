using Microsoft.AspNetCore.Mvc;

namespace NomNomAPI.Services.FoodItemsService
{
    public interface IFoodItemService
    {
        Task<IEnumerable<FoodItem>> GetAllFoods();
        Task<List<FoodItem>> GetFoodsByName(string name);
        Task<FoodItem?> GetSingleFood(int id);
        Task<List<FoodItem>> AddFood(FoodItem food);
        Task<List<FoodItem>?> UpdateFood(int id, FoodItem request);
        Task<List<FoodItem>?> DeleteFood(int id);
        Task<List<FoodItem>> GetAllFoodByStoreId(int storeId);
        Task<List<FoodItem>> GetFoodItemsByPriceRange(double minPrice, double maxPrice);

        Task<IEnumerable<FoodItem>> GetFoodItemsAsync(
            int? storeId = null,
            string? category = null,
            DateTime? expirationDate = null,
            double? minPrice = null,
            double? maxPrice = null,
            double? minDiscount = null,
            string? name = null,
            bool? isVegan = null,
            string? description = null);  // Add this line
    }
}
