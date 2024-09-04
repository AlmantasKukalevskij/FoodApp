namespace NomNomAPI.Models
{
    public class FoodItem
    {//properties
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; }= string.Empty;
        public double Price { get; set; }
        public string ImageUrl {  get; set; }= "test";
    }
}
