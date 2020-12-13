using Data.Database;
using Microsoft.EntityFrameworkCore;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly StockNotificationContext _dbContext;

        public UserSettingsService(StockNotificationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserSeller>> GetUserExcludedSellerPreferences(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentOutOfRangeException($"{nameof(username)} is required");

            var userExclSellers = await _dbContext.tblExcludedSellers
                        .Join(_dbContext.tblSellers,
                        x => x.SellerId,
                        y => y.Id,
                        (x, y) => new UserSeller
                        {
                            Username = x.Username,
                            SellerName = y.Name,
                            SellerUrl = y.Url
                        })
                        .Where(x => x.Username == username)
                        .ToListAsync();

            return userExclSellers;
        }

        public async Task<IEnumerable<Seller>> GetSellers()
        {
            return await _dbContext.tblSellers
                            .Select(x => new Seller
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Url = x.Url
                            })
                            .ToListAsync();
        }

        public async Task UpdateUserSellerPreferences(string username, IEnumerable<int> sellerIds)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentOutOfRangeException($"{nameof(username)} is required");

            // get all sellers
            var allSellerIds = await _dbContext.tblSellers.Select(x => x.Id).ToListAsync();

            if (sellerIds.Except(allSellerIds).Any())  // Check if all provided ids map to sellers
                throw new Exception("Seller not found");

            var currentExclusions = await _dbContext.tblExcludedSellers.Where(x => x.Username == username).Select(x => x.SellerId).ToListAsync();

            // Determine insertions and removals based on provided ids and what's currently in db
            var correctExclusions = allSellerIds.Except(sellerIds);

            var removals = currentExclusions.Except(correctExclusions)
                .Select(x => new tblExcludedSellers
                {
                    Username = username,
                    SellerId = x,
                });

            var insertions = correctExclusions.Except(currentExclusions)
                .Select(x => new tblExcludedSellers
                {
                    Username = username,
                    SellerId = x,
                });

            _dbContext.tblExcludedSellers.AddRange(insertions);
            _dbContext.tblExcludedSellers.RemoveRange(removals);
            await _dbContext.SaveChangesAsync();
        }
    }
}
