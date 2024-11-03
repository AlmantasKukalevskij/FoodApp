namespace NomNomAPI.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTimeOffset ExpirationDate { get; set; }
        public string ImageUrl { get; set; } = "test";

        private double? _discountedPrice;

        public double DiscountedPrice
        {
            get
            {
                if (_discountedPrice.HasValue)
                    return _discountedPrice.Value;


                var daysUntilExpiration = (ExpirationDate - DateTimeOffset.Now).Days;

                _discountedPrice = daysUntilExpiration switch
                {
                    <= 1 => Price * 0.5,
                    <= 3 => Price * 0.7,
                    <= 7 => Price * 0.9,
                    _ => Price
                };

                return _discountedPrice.Value;
            }
        }

        public bool IsVegan { get; set; }
    }
}
