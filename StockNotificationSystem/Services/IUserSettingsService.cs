using Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserSettingsService
    {
        Task<IEnumerable<UserSeller>> GetUserExcludedSellerPreferences(string username);
        Task<IEnumerable<Seller>> GetSellers();
        Task UpdateUserSellerPreferences(string username, IEnumerable<int> sellerIds);
    }
}
