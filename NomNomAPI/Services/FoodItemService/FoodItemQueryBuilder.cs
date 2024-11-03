public class FoodItemQueryBuilder
{
    private IQueryable<FoodItem> _query;

    public FoodItemQueryBuilder(IQueryable<FoodItem> initialQuery)
    {
        _query = initialQuery;
    }

    public FoodItemQueryBuilder WithStoreId(int? storeId)
    {
        if (storeId.HasValue)
            _query = _query.Where(f => f.StoreId == storeId.Value);
        return this;
    }

    public FoodItemQueryBuilder WithCategory(string? category)
    {
        if (!string.IsNullOrEmpty(category))
            _query = _query.Where(f => f.Category.Equals(category, StringComparison.CurrentCultureIgnoreCase));
        return this;
    }

    public FoodItemQueryBuilder WithExpirationDate(DateTime? expirationDate)
    {
        if (expirationDate.HasValue)
            _query = _query.Where(f => f.ExpirationDate.Date <= expirationDate.Value.Date);
        return this;
    }

    public FoodItemQueryBuilder WithMinPrice(double? minPrice)
    {
        if (minPrice.HasValue)
            _query = _query.Where(f => f.Price >= minPrice.Value);
        return this;
    }

    public FoodItemQueryBuilder WithMaxPrice(double? maxPrice)
    {
        if (maxPrice.HasValue)
            _query = _query.Where(f => f.Price <= maxPrice.Value);
        return this;
    }

    public FoodItemQueryBuilder WithName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
            _query = _query.Where(f => f.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        return this;
    }

    public FoodItemQueryBuilder WithVeganFilter(bool? isVegan)
    {
        if (isVegan.HasValue)
            _query = _query.Where(f => f.IsVegan == isVegan.Value);
        return this;
    }

    public FoodItemQueryBuilder WithDescription(string? description)
    {
        if (!string.IsNullOrEmpty(description))
            _query = _query.Where(f => f.Description.Contains(description, StringComparison.CurrentCultureIgnoreCase));
        return this;
    }

    public FoodItemQueryBuilder WithMinDiscount(double? minDiscount)
    {
        if (minDiscount.HasValue)
            _query = _query.Where(f => (f.Price - f.DiscountedPrice) / f.Price >= minDiscount.Value);
        return this;
    }

    public IQueryable<FoodItem> Build()
    {
        return _query;
    }
}