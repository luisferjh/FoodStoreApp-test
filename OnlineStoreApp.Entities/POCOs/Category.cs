namespace OnlineStoreApp.Entities.POCOs
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Food> Foods { get; set; } = new List<Food>();
    }
}
