namespace OnlineStoreApp.Entities.POCOs
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int State { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<UserClaims> UserClaims { get; set; }
        public List<Order> Orders { get; set; }
    }
}
