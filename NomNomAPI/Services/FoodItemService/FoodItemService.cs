using Microsoft.AspNetCore.Mvc;
using NomNomAPI.Services.FoodItemsService;

namespace NomNomAPI.Services.FoodItemService
{
    public class FoodItemService : IFoodItemService
    {
        //private static List<FoodItem> foodItems = new List<FoodItem>{
        //    new FoodItem
        //    {
        //        Id = 1,
        //        StoreId=1,
        //        Name = "Super paslaptingas maisto krepselis",
        //        Description = "Musu parduotuves delikatesai jusu malonumui uz mazesne kaina",
        //        Price = 5
        //    },
        //    new FoodItem
        //    {
        //        Id = 2,
        //        StoreId=2,
        //        Name = "Paprastas paslaptingas maisto krepselis",
        //        Description = "Musu parduotuves delikatesai jusu malonumui uz mazesne kaina",
        //        Price = 3
        //    }
        //};
        private readonly DataContext _context;

        public FoodItemService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<FoodItem>> AddFood(FoodItem food)
        {
            _context.foodItems.Add(food);
            await _context.SaveChangesAsync();
            return await _context.foodItems.ToListAsync();
        }

        public async Task<List<FoodItem>> GetFoodsByName(string name)
        {
            return await _context.foodItems
                         .Where(foodItem => foodItem.Name.ToLower().Contains(name.ToLower()))
                         .ToListAsync();
        }

        public async Task<List<FoodItem>?> DeleteFood(int id)
        {

            var food = await _context.foodItems.FindAsync(id);
            if (food == null)
                return null;//perdaryti geriau?

            _context.foodItems.Remove(food);
            await _context.SaveChangesAsync();

            return await _context.foodItems.ToListAsync();
        }

        public async Task<IEnumerable<FoodItem>> GetAllFoods()
        {
            var foods = await _context.foodItems.ToListAsync();
            foreach (var food in foods)
            {
                food.ApplyDiscount();
            }
            return foods;
        }

        public async Task<FoodItem?> GetSingleFood(int id)
        {
            var food = await _context.foodItems.FindAsync(id);
            if (food == null)
                return null;
            return food;
        }

        public async Task<List<FoodItem>?> UpdateFood(int id, FoodItem request)
        {
            var food = await _context.foodItems.FindAsync(id);
            if (food == null)
                return null;

            food.Name = request.Name;
            food.Description = request.Description;
            food.Price = request.Price;
            food.StoreId = request.StoreId;
            food.ImageUrl = request.ImageUrl;

            //saving changes
            await _context.SaveChangesAsync();

            return await _context.foodItems.ToListAsync();


        }

        public async Task<List<FoodItem>> GetAllFoodByStoreId(int storeId)
        {
            return await _context.foodItems
                                 .Where(foodItem => foodItem.StoreId == storeId)
                                 .ToListAsync();
        }
        public async Task<List<FoodItem>> GetFoodItemsByPriceRange(double minPrice, double maxPrice)
        {
            return await _context.foodItems
                                 .Where(foodItem => foodItem.Price >= minPrice && foodItem.Price <= maxPrice)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<FoodItem>> GetFoodItemsAsync(
            int? storeId = null,
            string? category = null,
            DateTime? expirationDate = null,
            double? minPrice = null,
            double? maxPrice = null,
            double? minDiscount = null,
            string? name = null,
            bool? isVegan = null,
            string? description = null)  // Add this parameter
        {
            var query = _context.foodItems.AsQueryable();

            if (storeId.HasValue)
                query = query.Where(f => f.StoreId == storeId.Value);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(f => f.Category.ToLower() == category.ToLower());

            if (expirationDate.HasValue)
                query = query.Where(f => f.ExpirationDate.Date <= expirationDate.Value.Date);

            if (minPrice.HasValue)
                query = query.Where(f => f.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(f => f.Price <= maxPrice.Value);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(f => f.Name.ToLower().Contains(name.ToLower()));

            if (isVegan.HasValue)
                query = query.Where(f => f.IsVegan == isVegan.Value);

            if (!string.IsNullOrEmpty(description))
                query = query.Where(f => f.Description.ToLower().Contains(description.ToLower()));

            var foodItems = await query.ToListAsync();

            // Apply discounts
            foreach (var item in foodItems)
            {
                item.ApplyDiscount();
            }

            // Filter by minimum discount if specified
            if (minDiscount.HasValue)
            {
                foodItems = foodItems.Where(f => (f.Price - f.DiscountedPrice) / f.Price >= minDiscount.Value).ToList();
            }

            return foodItems;
        }
    }
}
