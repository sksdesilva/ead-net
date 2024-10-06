using System.Threading.Tasks;
using UserRegistrationAPI.Models;

namespace UserRegistrationAPI.Services
{
   public interface IVendorService
{
    Task CreateVendorAsync(Vendor vendor);
    Task<Vendor> GetVendorByIdAsync(string id);
    Task AddVendorRankingAsync(string vendorId, VendorReview review);
    Task UpdateCustomerCommentAsync(string vendorId, string customerId, string newComment);
}

}
