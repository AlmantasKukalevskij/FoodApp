﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NomNomAPI.Models;
using NomNomAPI.Services.FoodItemsService;

namespace NomNomAPI.Controllers
{

    ////Praso autorizacijos pries naudojant funkcijas(program.cs- addAuthentication)
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodItemService _foodItemService;

        public FoodController(IFoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        //metodas gaut visus food item su tais paciais pavadinimais
        [HttpGet("ByName")]
        public async Task<ActionResult<List<FoodItem>>> GetFoodsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Iveskite pavadinima");
            }

            var foodItems = await _foodItemService.GetFoodsByName(name);

            if (foodItems == null || !foodItems.Any())
            {
                return NotFound($"Nera tokio maisto krepselio pavadinimu: {name}");
            }

            return Ok(foodItems);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetSingleFood(int id)
        {
            var result = await _foodItemService.GetSingleFood(id);
            if (result == null)
                return NotFound("Food not found");
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<List<FoodItem>>> AddFood(FoodItem food)
        {
            var result = await _foodItemService.AddFood(food);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<FoodItem>>> UpdateFood(int id, FoodItem request)
        {
            var result = await _foodItemService.UpdateFood(id, request);
            if (result == null)
                return NotFound("Food not found");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<FoodItem>>> DeleteFood(int id)
        {
            var result = await _foodItemService.DeleteFood(id);
            if (result == null)
                return NotFound("Food not found");
            return Ok(result);
        }

        [HttpGet("ByStore/{storeId}")]
        public async Task<ActionResult<List<FoodItem>>> GetFoodItemsByStoreId(int storeId)
        {
            var foodItems = await _foodItemService.GetAllFoodByStoreId(storeId);

            if (foodItems == null || !foodItems.Any())
            {
                return NotFound($"Produktu nerasta parduotuveje kurios ID: {storeId}");
            }

            return Ok(foodItems);
        }
        [HttpGet("ByPriceRange")]
        public async Task<ActionResult<List<FoodItem>>> GetFoodItemsByPriceRange([FromQuery] double minPrice, [FromQuery] double maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0)
            {
                return BadRequest("Kaina negali buti neigiama");
            }

            if (minPrice > maxPrice)
            {
                return BadRequest("Minimali kaina negali buti didesne uz maksimalia kaina");
            }

            var foodItems = await _foodItemService.GetFoodItemsByPriceRange(minPrice, maxPrice);

            if (foodItems == null || !foodItems.Any())
            {
                return NotFound($"Nerasta produktu nurodytame kainu diapazone nuo {minPrice} iki {maxPrice}");
            }

            return Ok(foodItems);
        }

        [HttpGet]
        public async Task<IActionResult> GetFoodItems(
            [FromQuery] int? storeId = null,
            [FromQuery] string? category = null,
            [FromQuery] DateTime? expirationDate = null,
            [FromQuery] double? minPrice = null,
            [FromQuery] double? maxPrice = null,
            [FromQuery] double? minDiscount = null,
            [FromQuery] string? name = null,
            [FromQuery] bool? isVegan = null,
            [FromQuery] string? description = null)  // Add this parameter
        {
            var foodItems = await _foodItemService.GetFoodItemsAsync(
                storeId, category, expirationDate, minPrice, maxPrice, minDiscount, name, isVegan, description);

            var result = foodItems.Select(f => new
            {
                f.Id,
                f.StoreId,
                f.Name,
                f.Description,
                f.Category,
                OriginalPrice = f.Price,
                DiscountedPrice = f.DiscountedPrice,
                DiscountPercentage = (f.Price - f.DiscountedPrice) / f.Price * 100,
                f.ExpirationDate,
                f.ImageUrl,
                f.IsVegan
            });

            return Ok(result);
        }

    }
}
