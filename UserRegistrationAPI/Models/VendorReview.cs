using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class VendorReview
{
     [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string CustomerId { get; set; }  // User who posted the review
    public string Comment { get; set; }
    public decimal Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
