using Data.Builders;
using Data.Database;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class UserSettingsServiceTests
    {
        private readonly DbContextOptions<StockNotificationContext> _options;
        private readonly StockNotificationContext _dbContext;
        private readonly UserSettingsService _userSettingsService;

        private const string _username = "UnitTestUsername";

        public UserSettingsServiceTests()
        {
            _options = new DbContextOptionsBuilder<StockNotificationContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _dbContext = new StockNotificationContext(_options);

            _userSettingsService = new UserSettingsService(_dbContext);
        }

        [Fact]
        public async Task GetUserExcludedSellerPreferences_Should_Return_List_Of_Sellers_User_Has_Excluded()
        {
            // Arrange
            await _dbContext.Database.EnsureDeletedAsync();
            var listOfSellers = new List<tblSellers>();
            listOfSellers.Add(new tblSellersBuilder().WithId(1).WithName("Amazon").WithUrl("https://www.amazon.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(2).WithName("Ebay").WithUrl("https://www.ebay.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(3).WithName("Currys PC World").WithUrl("https://www.currys.co.uk").Build());
            _dbContext.tblSellers.AddRange(listOfSellers);

            var listOfExclSellers = new List<tblExcludedSellers>();
            listOfExclSellers.Add(new tblExcludedSellersBuilder().WithUsername(_username).WithSellerId(2).Build());
            listOfExclSellers.Add(new tblExcludedSellersBuilder().WithUsername(_username).WithSellerId(3).Build());
            _dbContext.tblExcludedSellers.AddRange(listOfExclSellers);

            await _dbContext.SaveChangesAsync();

            // Act
            var task = _userSettingsService.GetUserExcludedSellerPreferences(_username);
            var result = task.Result;

            Assert.IsAssignableFrom<IEnumerable<UserSeller>>(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetSellers_Should_Return_List_Of_Sellers()
        {
            // Arrange
            await _dbContext.Database.EnsureDeletedAsync();
            var listOfSellers = new List<tblSellers>();
            listOfSellers.Add(new tblSellersBuilder().WithId(1).WithName("Amazon").WithUrl("https://www.amazon.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(2).WithName("Ebay").WithUrl("https://www.ebay.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(3).WithName("Currys PC World").WithUrl("https://www.currys.co.uk").Build());
            _dbContext.tblSellers.AddRange(listOfSellers);

            await _dbContext.SaveChangesAsync();

            // Act
            var task = _userSettingsService.GetSellers();
            var result = task.Result;

            Assert.IsAssignableFrom<IEnumerable<Seller>>(result);
            Assert.Equal(3, result.Count());
        }

        // THIS UNIT TEST WILL THROW THE EXCEPTION
        [Fact]
        public async Task UpdateUserSellerPreferences_Updates_User_Settings()
        {
            // Arrange
            await _dbContext.Database.EnsureDeletedAsync();
            var listOfSellers = new List<tblSellers>();
            listOfSellers.Add(new tblSellersBuilder().WithId(1).WithName("Amazon").WithUrl("https://www.amazon.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(2).WithName("Ebay").WithUrl("https://www.ebay.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(3).WithName("Currys PC World").WithUrl("https://www.currys.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(4).WithName("Argos").WithUrl("https://www.argos.co.uk").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(5).WithName("Smyths").WithUrl("https://www.smythstoys.com").Build());
            listOfSellers.Add(new tblSellersBuilder().WithId(6).WithName("Target").WithUrl("https://www.target.com").Build());
            _dbContext.tblSellers.AddRange(listOfSellers);

            var listOfExclSellers = new List<tblExcludedSellers>();
            listOfExclSellers.Add(new tblExcludedSellersBuilder().WithUsername(_username).WithSellerId(5).Build());
            listOfExclSellers.Add(new tblExcludedSellersBuilder().WithUsername(_username).WithSellerId(6).Build());
            _dbContext.tblExcludedSellers.AddRange(listOfExclSellers);

            await _dbContext.SaveChangesAsync();

            var newListOfSellers = new List<int>() { 1, 2, 3, 5 };

            // Act
            var task = _userSettingsService.UpdateUserSellerPreferences(_username, newListOfSellers);
            await task;

            // Assert
            var updatedList = _dbContext.tblExcludedSellers.Where(x => x.Username == _username).Select(x => x.SellerId).ToList();
            Assert.Equal(2, updatedList.Count);
            Assert.Contains(4, updatedList);
            Assert.Contains(6, updatedList);
        }

        [Fact]
        public async Task UpdateUserSellerPreferences_Updates_User_Settings_Two()
        {
            // Arrange
            await _dbContext.Database.EnsureDeletedAsync();

            using (var seedingContext = new StockNotificationContext(_options))
            {
                var listOfSellers = new List<tblSellers>();
                listOfSellers.Add(new tblSellersBuilder().WithId(1).WithName("Amazon").WithUrl("https://www.amazon.co.uk").Build());
                listOfSellers.Add(new tblSellersBuilder().WithId(2).WithName("Ebay").WithUrl("https://www.ebay.co.uk").Build());
                listOfSellers.Add(new tblSellersBuilder().WithId(3).WithName("Currys PC World").WithUrl("https://www.currys.co.uk").Build());
                listOfSellers.Add(new tblSellersBuilder().WithId(4).WithName("Argos").WithUrl("https://www.argos.co.uk").Build());
                listOfSellers.Add(new tblSellersBuilder().WithId(5).WithName("Smyths").WithUrl("https://www.smythstoys.com").Build());
                listOfSellers.Add(new tblSellersBuilder().WithId(6).WithName("Target").WithUrl("https://www.target.com").Build());
                seedingContext.tblSellers.AddRange(listOfSellers);

                var listOfExclSellers = new List<tblExcludedSellers>();
                listOfExclSellers.Add(new tblExcludedSellersBuilder().WithUsername(_username).WithSellerId(5).Build());
                listOfExclSellers.Add(new tblExcludedSellersBuilder().WithUsername(_username).WithSellerId(6).Build());
                seedingContext.tblExcludedSellers.AddRange(listOfExclSellers);

                await seedingContext.SaveChangesAsync();
            }

            var newListOfSellers = new List<int>() { 1, 2, 3, 5 };

            // Act
            var task = _userSettingsService.UpdateUserSellerPreferences(_username, newListOfSellers);
            await task;

            // Assert
            var updatedList = _dbContext.tblExcludedSellers.Where(x => x.Username == _username).Select(x => x.SellerId).ToList();
            Assert.Equal(2, updatedList.Count);
            Assert.Contains(4, updatedList);
            Assert.Contains(6, updatedList);
        }
    }
}


//----------Starting test run ----------
//[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.4.3+1b45f5407b (64-bit .NET Core 3.1.8)
//[xUnit.net 00:00:00.35]   Starting: Test
//[xUnit.net 00:00:00.95]     Test.UserSettingsServiceTests.UpdateUserSellerPreferences_Updates_User_Settings[FAIL]
//[xUnit.net 00:00:00.95]       System.InvalidOperationException : The instance of entity type 'tblExcludedSellers' cannot be tracked because another instance with the same key value for {'SellerId', 'Username'} is already being tracked.When attaching existing entities, ensure that only one entity instance with a given key value is attached. Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' to see the conflicting key values.
//[xUnit.net 00:00:00.95]       Stack Trace:
//[xUnit.net 00:00:00.95]
//at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.ThrowIdentityConflict(InternalEntityEntry entry)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.Add(TKey key, InternalEntityEntry entry, Boolean updateDuplicate)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.IdentityMap`1.Add(TKey key, InternalEntityEntry entry)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.NullableKeyIdentityMap`1.Add(InternalEntityEntry entry)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.StartTracking(InternalEntityEntry entry)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry.SetEntityState(EntityState oldState, EntityState newState, Boolean acceptChanges, Boolean modifyProperties)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.InternalEntityEntry.SetEntityState(EntityState entityState, Boolean acceptChanges, Boolean modifyProperties, Nullable`1 forceStateWhenUnknownKey)
//[xUnit.net 00:00:00.95]
//at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.EntityGraphAttacher.PaintAction(EntityEntryGraphNode`1 node)
//[xUnit.net 00:00:00.95]
//at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.EntityEntryGraphIterator.TraverseGraph[TState] (EntityEntryGraphNode`1 node, Func`2 handleNode)
//[xUnit.net 00:00:00.95]
//at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.EntityGraphAttacher.AttachGraph(InternalEntityEntry rootEntry, EntityState targetState, EntityState storeGeneratedWithKeySetTargetState, Boolean forceStateWhenUnknownKey)
//[xUnit.net 00:00:00.95]
//at Microsoft.EntityFrameworkCore.Internal.InternalDbSet`1.SetEntityState(InternalEntityEntry entry, EntityState entityState)
//[xUnit.net 00:00:00.95] at Microsoft.EntityFrameworkCore.Internal.InternalDbSet`1.RemoveRange(IEnumerable`1 entities)
//[xUnit.net 00:00:00.95]         C:\test\StockNotificationSystem\Services\UserSettingsService.cs(84, 0): at Services.UserSettingsService.UpdateUserSellerPreferences(String username, IEnumerable`1 sellerIds)
//[xUnit.net 00:00:00.95]         C:\test\StockNotificationSystem\Test\UserSettingsServiceTests.cs(104, 0): at Test.UserSettingsServiceTests.UpdateUserSellerPreferences_Updates_User_Settings()
//[xUnit.net 00:00:00.95]         ---End of stack trace from previous location where exception was thrown ---


//----------Starting test run ----------
//[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v2.4.3+1b45f5407b (64-bit .NET Core 3.1.8)
//[xUnit.net 00:00:00.37]   Starting: Test
//[xUnit.net 00:00:01.03]     Test.UserSettingsServiceTests.UpdateUserSellerPreferences_Updates_User_Settings_Two[FAIL]
//[xUnit.net 00:00:01.03]       Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException : Attempted to update or delete an entity that does not exist in the store.
//[xUnit.net 00:00:01.03]       Stack Trace:
//[xUnit.net 00:00:01.03]
//at Microsoft.EntityFrameworkCore.InMemory.Storage.Internal.InMemoryTable`1.Delete(IUpdateEntry entry)
//[xUnit.net 00:00:01.03] at Microsoft.EntityFrameworkCore.InMemory.Storage.Internal.InMemoryStore.ExecuteTransaction(IList`1 entries, IDiagnosticsLogger`1 updateLogger)
//[xUnit.net 00:00:01.03]
//at Microsoft.EntityFrameworkCore.InMemory.Storage.Internal.InMemoryDatabase.SaveChangesAsync(IList`1 entries, CancellationToken cancellationToken)
//[xUnit.net 00:00:01.03]
//at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(IList`1 entriesToSave, CancellationToken cancellationToken)
//[xUnit.net 00:00:01.03]
//at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChangesAsync(DbContext _, Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
//[xUnit.net 00:00:01.03]
//at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
//[xUnit.net 00:00:01.03]
//at Microsoft.EntityFrameworkCore.DbContext.SaveChangesAsync(Boolean acceptAllChangesOnSuccess, CancellationToken cancellationToken)
//[xUnit.net 00:00:01.03]         C:\test\StockNotificationSystem\Services\UserSettingsService.cs(97, 0): at Services.UserSettingsService.UpdateUserSellerPreferencesTwo(String username, IEnumerable`1 sellerIds)
//[xUnit.net 00:00:01.03]         C:\test\StockNotificationSystem\Test\UserSettingsServiceTests.cs(131, 0): at Test.UserSettingsServiceTests.UpdateUserSellerPreferences_Updates_User_Settings_Two()
//[xUnit.net 00:00:01.03]         ---End of stack trace from previous location where exception was thrown ---
//[xUnit.net 00:00:01.04]   Finished: Test
//========== Test run finished: 1 Tests run in 1.5 sec (0 Passed, 1 Failed, 0 Skipped) ==========