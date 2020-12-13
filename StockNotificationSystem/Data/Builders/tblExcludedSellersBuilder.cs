using Data.Database;
using System;
using System.Collections.Generic;

namespace Data.Builders
{
    public class tblExcludedSellersBuilder
    {
        private readonly tblExcludedSellers _tblExcludedSellers;
        private readonly Random _random;

        public tblExcludedSellersBuilder(Random random = null)
        {
            _random = random ?? new Random();
            _tblExcludedSellers = new tblExcludedSellers
            {
                //Id = _random.Next(),
                Username = _random.Next().ToString(),
                SellerId = _random.Next(),
            };
        }

        public tblExcludedSellers Build()
        {
            return _tblExcludedSellers;
        }

        public tblExcludedSellersBuilder WithUsername(string username)
        {
            _tblExcludedSellers.Username = username;
            return this;
        }

        public tblExcludedSellersBuilder WithSellerId(int sellerId)
        {
            _tblExcludedSellers.SellerId = sellerId;
            return this;
        }

        public static void Initialize(StockNotificationContext stockNotificationContext)
        {
            var listOfExcludedSellers = new List<tblExcludedSellers>();
            listOfExcludedSellers.Add(new tblExcludedSellersBuilder().WithUsername("bjorn").WithSellerId(7).Build());
            listOfExcludedSellers.Add(new tblExcludedSellersBuilder().WithUsername("bjorn").WithSellerId(8).Build());

            stockNotificationContext.tblExcludedSellers.AddRange(listOfExcludedSellers);
            stockNotificationContext.SaveChanges();
        }
    }
}
