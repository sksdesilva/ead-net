using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace UserRegistrationAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  // MongoDB will auto-generate the Id

        [BsonElement("Username")]
        public string Username { get; set; } = null!;

        // Temporary field to accept password from user (not stored directly)
        [BsonIgnore]
        public string Password { get; set; } = null!;  // Plain password (not stored)

        [BsonElement("PasswordHash")]
        public string PasswordHash { get; set; } = null!;  // Hashed password stored

        private List<string> _roles = new(); // Backing field for roles

        [BsonElement("Roles")]
        public List<string> Roles
        {
            get => _roles;
            set
            {
                foreach (var role in value)
                {
                    if (!IsValidRole(role))
                    {
                        throw new ArgumentException($"Invalid role: {role}. Allowed roles are: Administrator, Vendor, and Customer Service Representative.");
                    }
                }
                _roles = value;
            }
        }

        // Method to check if the role is valid
        private bool IsValidRole(string role)
        {
            var allowedRoles = new List<string>
            {
                "Administrator",
                "Vendor",
                "Customer Service Representative"
            };

            return allowedRoles.Contains(role);
        }
    }
}
