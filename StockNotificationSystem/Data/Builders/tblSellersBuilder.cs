using Data.Database;
using System;
using System.Collections.Generic;

namespace Data.Builders
{
    public class tblSellersBuilder
    {
        private readonly tblSellers _tblSellers;
        private readonly Random _random;

        public tblSellersBuilder(Random random = null)
        {
            _random = random ?? new Random();
            _tblSellers = new tblSellers
            {
                Id = _random.Next(),
                Name = _random.Next().ToString(),
                Url = _random.Next().ToString(),
            };
        }

        public tblSellers Build()
        {
            return _tblSellers;
        }

        public tblSellersBuilder WithId(int id)
        {
            _tblSellers.Id = id;
            return this;
        }

        public tblSellersBuilder WithName(string name)
        {
            _tblSellers.Name = name;
            return this;
        }

        public tblSellersBuilder WithUrl(string url)
        {
            _tblSellers.Url = url;
            return this;
        }

        public static void Initialize(StockNotificationContext stockNotificationContext)
        {
            var listOfSellers = new List<tblSellers>();
            listOfSellers.Add(new tblSellersBuilder().WithId(1).WithName("Amazon").WithUrl("https://www.amazon.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(2).WithName("Ebay").WithUrl("https://www.ebay.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(3).WithName("Currys PC World").WithUrl("https://www.currys.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(4).WithName("Argos").WithUrl("https://www.argos.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(5).WithName("Smyths").WithUrl("https://www.smythstoys.com").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(6).WithName("Target").WithUrl("https://www.target.com").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(7).WithName("Best Buy").WithUrl("https://www.bestbuy.com").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(8).WithName("Walmart").WithUrl("https://www.walmart.com").Build());

            stockNotificationContext.tblSellers.AddRange(listOfSellers);
            stockNotificationContext.SaveChanges();
        }
    }
}
