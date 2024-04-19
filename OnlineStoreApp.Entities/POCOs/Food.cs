﻿namespace OnlineStoreApp.Entities.POCOs
{
    public class Food
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }

        public Category Category { get; set; }
    }
}
