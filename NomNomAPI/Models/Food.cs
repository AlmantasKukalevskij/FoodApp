namespace NomNomAPI.Models
{
    public class FoodItem
    {//properties
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTimeOffset ExpirationDate { get; set; }
        public string ImageUrl { get; set; } = "test";

        public double DiscountedPrice { get; set; }

        public bool IsVegan { get; set; }

        public void ApplyDiscount()
        {
            var daysUntilExpiration = (ExpirationDate - DateTimeOffset.Now).Days;

            if (daysUntilExpiration <= 1)
            {
                DiscountedPrice = Price * 0.5; // 50% discount if expiring within a day
            }
            else if (daysUntilExpiration <= 3)
            {
                DiscountedPrice = Price * 0.7; // 30% discount if expiring within 3 days
            }
            else if (daysUntilExpiration <= 7)
            {
                DiscountedPrice = Price * 0.9; // 10% discount if expiring within a week
            }
            else
            {
                DiscountedPrice = Price; // No discount
            }
        }
    }
}
