namespace UserRegistrationAPI.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // Make the Id field nullable

        public string VendorId { get; set; }  // Owner of the product
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
