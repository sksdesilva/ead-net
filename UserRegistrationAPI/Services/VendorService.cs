using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;


public class VendorService : UserRegistrationAPI.Services.IVendorService
{
    private readonly IMongoCollection<Vendor> _vendors;

    public VendorService(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("VendorManagementDB");
        _vendors = database.GetCollection<Vendor>("Vendors");
    }

    public async Task CreateVendorAsync(Vendor vendor)
    {

        if (vendor.Reviews == null)
    {
        vendor.Reviews = new List<VendorReview>();
    }

        await _vendors.InsertOneAsync(vendor);
    }

    public async Task<Vendor> GetVendorByIdAsync(string id)
    {
        return await _vendors.Find(v => v.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddVendorRankingAsync(string vendorId, VendorReview review)
    {
        var vendor = await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
    if (vendor != null && vendor.Reviews == null)
    {
        vendor.Reviews = new List<VendorReview>();
        var update = Builders<Vendor>.Update.Set(v => v.Reviews, vendor.Reviews);
        await _vendors.UpdateOneAsync(v => v.Id == vendorId, update);
    }

    var filter = Builders<Vendor>.Filter.Eq(v => v.Id, vendorId);
    var updateReview = Builders<Vendor>.Update.Push(v => v.Reviews, review);
    await _vendors.UpdateOneAsync(filter, updateReview);

    // Recalculate average ranking
    await RecalculateAverageRanking(vendorId);
    }

   private async Task RecalculateAverageRanking(string vendorId)
{
    var vendor = await _vendors.Find(v => v.Id == vendorId).FirstOrDefaultAsync();
    if (vendor != null && vendor.Reviews.Any())
    {
        decimal average = vendor.Reviews.Average(r => r.Rating);
        // Correct use of Builders<Vendor>.Update
        var update = Builders<Vendor>.Update.Set(v => v.AverageRanking, average);
        await _vendors.UpdateOneAsync(v => v.Id == vendorId, update);
    }
}

    public async Task UpdateCustomerCommentAsync(string vendorId, string customerId, string newComment)
    {
        var filter = Builders<Vendor>.Filter.And(
            Builders<Vendor>.Filter.Eq(v => v.Id, vendorId),
            Builders<Vendor>.Filter.ElemMatch(v => v.Reviews, r => r.CustomerId == customerId)
        );
        var update = Builders<Vendor>.Update.Set("Reviews.$.Comment", newComment);
        await _vendors.UpdateOneAsync(filter, update);
    }
}
