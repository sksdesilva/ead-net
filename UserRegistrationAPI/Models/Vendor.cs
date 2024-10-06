using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


public class Vendor

    {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
      public List<VendorReview> Reviews { get; set; } = new List<VendorReview>();
    
    // AverageRanking is nullable as it might not be available at creation
    public decimal? AverageRanking { get; set; } 
}

