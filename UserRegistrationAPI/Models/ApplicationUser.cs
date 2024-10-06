using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UserRegistrationAPI.Models
{
    public class ApplicationUser
    {
        private static readonly HashSet<string> AllowedRoles = new HashSet<string> { "Administrator", "Vendor", "CSR","Customer" };

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // MongoDB will auto-generate the Id

        [BsonElement("Username")]
        public string Username { get; set; } = null!;

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; } = null!;  // Hashed password stored

        [BsonIgnore]
        public string Password { get; set; } = null!;  // Plain password (not stored)

        private List<string> roles = new();
        
        [BsonElement("Roles")]
        public List<string> Roles
        {
            get => roles;
            set
            {
                if (value.Any(role => !AllowedRoles.Contains(role)))
                {
                    throw new ArgumentException("Roles can only be 'Administrator', 'Vendor', 'Customer , 'or 'CSR'.");
                }
                roles = value;
            }
        }
    }
}
