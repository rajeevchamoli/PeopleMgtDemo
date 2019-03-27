using Microsoft.EntityFrameworkCore;
using PeopleMgt.Data;
using PeopleMgt.Models;
using PeopleMgt.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;

namespace PeopleMgtTest
{
    public class UserRepositoryTests
    {
        /// <summary>
        /// Create In-Memory DBContext to have test data.
        /// </summary>
        /// <returns></returns>
        private UserDbContext GetContextWithData(DbContextOptions dbOptions= null)
        {
            var options = dbOptions ?? new DbContextOptionsBuilder<UserDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).Options;

            var context = new UserDbContext(options);
            

            // add some test data to in-memory instance.
            context.UserTable.Add(new User { Id = 1, FirstName = "FName1", LastName = "LName1", Email = "testEmail1@test.com", Age = 21, Address = "Address1", Interests = "Coding1", Picture = "Beryllium_pic" });
            context.UserTable.Add(new User { Id = 2, FirstName = "FName2", LastName = "LName2", Email = "testEmail2@test.com", Age = 22, Address = "Address2", Interests = "Cooking2", Picture = "Nitrogen_pic" });
            context.UserTable.Add(new User { Id = 3, FirstName = "FName3", LastName = "LName3", Email = "testEmail3@test.com", Age = 23, Address = "Address3", Interests = "Movies3", Picture = "Oxygen_pic" });
            context.UserTable.Add(new User { Id = 4, FirstName = "FName4", LastName = "LName4", Email = "testEmail4@test.com", Age = 24, Address = "Address4", Interests = "Dancing4", Picture = "Lithium_pic" });
            context.UserTable.Add(new User { Id = 5, FirstName = "Eva", LastName = "Corets", Email = "testEmail5@test.com", Age = 25, Address = "Address5", Interests = "Painting5", Picture = "Helium_pic" });
            context.UserTable.Add(new User { Id = 6, FirstName = "Susan", LastName = "Burk", Email = "testEmail6@test.com", Age = 26, Address = "Address6", Interests = "Sports6", Picture = "Neon_pic" });
            context.UserTable.Add(new User { Id = 7, FirstName = "Patrick", LastName = "Steiner", Email = "testEmail7@test.com", Age = 27, Address = "Address7", Interests = "Reading7", Picture = "Sodium_pic" });
            context.UserTable.Add(new User { Id = 8, FirstName = "Gabriele", LastName = "Cannata", Email = "testEmail8@test.com", Age = 58, Address = "Address8", Interests = "Movies8", Picture = "Aluminum_pic" });
            context.UserTable.Add(new User { Id = 9, FirstName = "George", LastName = "Sullivan", Email = "testEmail9@test.com", Age = 22, Address = "Address9", Interests = "Sports9", Picture = "Neon_pic" });
            context.UserTable.Add(new User { Id = 10, FirstName = "Tamer", LastName = "Salah", Email = "testEmail10@test.com", Age = 37, Address = "Address10", Interests = "Reading10", Picture = "Sodium_pic" });
            context.UserTable.Add(new User { Id = 11, FirstName = "Andrew", LastName = "Dixon", Email = "testEmail11@test.com", Age = 28, Address = "Address11", Interests = "Movies11", Picture = "Aluminum_pic" });

            // commit 
            context.SaveChanges();
            
            return context;
        }

        [Fact(DisplayName = "GetUsersAsync should return all the results for empty page param")]
        public async Task GetUsersAsync_should_return_all_the_results_for_empty_page_param()
        {
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);
                var pageParameters = new PageParameters
                {
                    Filter = string.Empty,
                    PageNumber = 1,
                    PageSize = 5,
                    SortColumn = string.Empty,
                    SortOrder = string.Empty
                };
                var result = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.NotNull(userList);
                var pageMetadata = result.Item2;
                Assert.NotNull(pageMetadata);

                Assert.True(userList.Count == pageParameters.PageSize);
                Assert.True(pageMetadata.RecordCount == 11);

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "GetUsersAsync should return all the results for empty filter")]
        public async Task GetUsersAsync_should_return_all_the_results_for_empty_filter()
        {
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);
                var pageParameters = new PageParameters
                {
                    Filter = string.Empty,
                    PageNumber = 1,
                    PageSize = 5,
                    SortColumn = "FirstName",
                    SortOrder = "asc"
                };
                var result = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.NotNull(userList);
                var pageMetadata = result.Item2;
                Assert.NotNull(pageMetadata);

                Assert.True(userList.Count == pageParameters.PageSize);
                Assert.True(pageMetadata.RecordCount == 11);

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "GetUsersAsync should return result for 'interests' matching 'Movies' when orderby valid column name and missing sort order")]
        public async Task GetUsersAsync_should_return_results_in_asc_order_when_sortColumnIsPresent_But_sortOrder_IsEmpty()
        {
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);
                var pageParameters = new PageParameters { Filter = "Movies", PageNumber = 1, PageSize = 5, SortColumn = "FirstName", SortOrder = "" };
                var result = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.NotNull(userList);
                Assert.True(userList[0].FirstName == "Andrew");
                Assert.True(userList[1].FirstName == "FName3"); // test result are in asc sorted order after filter
                var pageMetadata = result.Item2;
                Assert.NotNull(pageMetadata);
                Assert.True(pageMetadata.RecordCount == 3); // only 3 records are expected from test data

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "GetUsersAsync should return 'interests' matching 'Movies' when orderby firstname and decending order")]
        public async Task GetUsersAsync_should_return_Results_and_sorted_decending()
        {
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);
                var pageParameters = new PageParameters { Filter = "Movies", PageNumber = 1, PageSize = 5, SortColumn = "FirstName", SortOrder = "desc" };
                var result = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.NotNull(userList);
                Assert.True(userList[0].FirstName == "Gabriele");
                Assert.True(userList[1].FirstName == "FName3"); // test result are in desc sorted order after filter
                var pageMetadata = result.Item2;
                Assert.NotNull(pageMetadata);
                Assert.True(pageMetadata.RecordCount == 3); // only 3 records are expected from test data

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "GetUsersAsync should return 2nd page for page no 2, when filtered by address")]
        public async Task GetUsersAsync_should_return_2_page_filtred_by_address_and_pageNumber_2()
        {
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);
                var pageParameters = new PageParameters { Filter = "Address", PageNumber = 2, PageSize = 5, SortColumn = "Address", SortOrder = "asc" };
                var result = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.NotNull(userList);
                var pageMetadata = result.Item2;
                Assert.NotNull(pageMetadata);
                Assert.Equal("Address4", userList[0].Address, ignoreCase: true); // address 1,10,11, 2, 3 will in first page.
                Assert.True(pageMetadata.Current == 2);

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "GetUsersAsync should return same set of records when filtered by 'interests' = 'Movies' or 'interests' = 'MOviEs'")]
        public async Task GetUsersAsync_should_return_same_results_using_case_insensitive_filter()
        {
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);
                var pageParameters = new PageParameters { Filter = "Movies" };
                var result1 = await repository.GetUsersAsync(new PageParameters { Filter = "Movies" });
                var result2 = await repository.GetUsersAsync(new PageParameters { Filter = "MOviEs" });

                Assert.NotNull(result1);
                Assert.NotNull(result2);

                var userList1 = new List<User>(result1.Item1);
                var userList2 = new List<User>(result2.Item1);

                Assert.True(userList1.Count > 0 && userList1.Count == userList2.Count);
                Assert.Equal(userList1[0].FirstName, userList2[0].FirstName);

                var pageMetadata = result1.Item2;
                Assert.NotNull(pageMetadata);
                Assert.True(result1.Item2.RecordCount == result2.Item2.RecordCount); // same count in metadata

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "CreateUserAsync should create new record")]
        public async Task CreateUserAsync_should_create_new_record()
        {
            var testDataUsed = new List<User>();

            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);

                var newRecord = new User { Id = 25, FirstName = "TestFname", LastName = "TestLName", Age = 23, Address = "Some Drive" };
                testDataUsed.Add(newRecord);
                HandleInMemoryContextForCrudOperation(context, testDataUsed);

                await repository.CreateUserAsync(newRecord);

                var pageParameters = new PageParameters { Filter = "Some Drive", PageNumber = 1, PageSize = 5, SortColumn = "FirstName", SortOrder = "asc" };
                var result = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.True(userList.Count == 1);
                Assert.NotNull(userList[0]);
                Assert.True(userList[0].FirstName.Equals(newRecord.FirstName, StringComparison.OrdinalIgnoreCase) && userList[0].Id > 0);

                context.Database.EnsureDeleted();
            }

        }

        [Fact(DisplayName = "UpdateUserAsync should update existing record")]
        public async Task UpdateUserAsync_should_Update_Existing_record()
        {
            var pageParameters = new PageParameters
            {
                Filter = "FooFirstName",
                PageNumber = 1,
                PageSize = 5,
                SortColumn = "FirstName",
                SortOrder = "asc"
            };

            var id = 12;

            var testDataUsed = new List<User>();

            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);

                var newRecord = new User
                {
                    Id = id,
                    FirstName = "FooFirstName",
                    LastName = "FooLastName",
                    Age = 26,
                    Address = "New Drive"
                };
                testDataUsed.Add(newRecord);
                HandleInMemoryContextForCrudOperation(context, testDataUsed);

                await repository.CreateUserAsync(newRecord);

                var result = await repository.GetUsersAsync(pageParameters);

                // validate new records creation part
                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);

                Assert.True(userList.Count == 1);
                Assert.NotNull(userList[0]);
                Assert.Equal(userList[0].FirstName, newRecord.FirstName, ignoreCase: true);

                id = userList[0].Id;

                var updatedRecord = new User()
                {
                    Id = id,
                    FirstName = "FooFirstName",
                    LastName = "FooLastName",
                    Age = 26,
                    Address = "New Drive"
                };

                updatedRecord.Interests = "Testing";
                testDataUsed.Add(updatedRecord);

                repository = new UserRepository(context);
                HandleInMemoryContextForCrudOperation(context, testDataUsed);

                await repository.UpdateUserAsync(updatedRecord);

                var updatedResult = await repository.GetUsersAsync(pageParameters);

                Assert.NotNull(updatedResult);
                userList = new List<User>(updatedResult.Item1);
                Assert.True(userList.Count == 1);
                Assert.NotNull(userList[0]);
                Assert.True(userList[0].FirstName.Equals(updatedRecord.FirstName, StringComparison.OrdinalIgnoreCase) 
                    && "Testing".Equals(userList[0].Interests, StringComparison.OrdinalIgnoreCase));

                context.Database.EnsureDeleted();

            }
        }

        [Fact(DisplayName = "DeleteUserAsync should create new record")]
        public async Task DeleteUserAsync_should_delete_existing_record()
        {
            var testDataUsed = new List<User>();
            using (var context = GetContextWithData())
            {
                var repository = new UserRepository(context);

                var newRecord = new User
                {
                    Id = 32,
                    FirstName = "BarFirstFname",
                    LastName = "BarLastName",
                    Age = 23,
                    Address = "Delete Drive"
                };

                testDataUsed.Add(newRecord);

                await repository.CreateUserAsync(newRecord);

                var pageParameters = new PageParameters
                {
                    Filter = "Delete Drive",
                    PageNumber = 1,
                    PageSize = 5,
                    SortColumn = "FirstName",
                    SortOrder = "asc"
                };

                var result = await repository.GetUsersAsync(pageParameters);
                HandleInMemoryContextForCrudOperation(context, testDataUsed);

                // validate new records creation part
                Assert.NotNull(result);
                var userList = new List<User>(result.Item1);
                Assert.True(userList.Count == 1);
                Assert.NotNull(userList[0]);
                Assert.True(userList[0].FirstName.Equals(newRecord.FirstName, StringComparison.OrdinalIgnoreCase) && userList[0].Id > 0);

                // validate deleting the records after creation.

                var recordToDelete = new User { Id = 0, FirstName = "BarFirstFname", LastName = "BarLastName" };
                testDataUsed.Add(newRecord);
                HandleInMemoryContextForCrudOperation(context, testDataUsed);

                await repository.DeleteUserAsync(userList[0]);

                var updatedResult = await repository.GetUsersAsync(pageParameters);
                Assert.NotNull(updatedResult);
                var updatedList = new List<User>(updatedResult.Item1);
                Assert.True(updatedList.Count == 0);

                context.Database.EnsureDeleted();
            }

        }

        private void HandleInMemoryContextForCrudOperation(UserDbContext cntx, List<User> testDataUsed)
        {
            // this is needed to issue mentioned in https://github.com/aspnet/EntityFrameworkCore/issues/12459
            foreach (var itm in testDataUsed)
            {
                cntx.Entry(itm).State = EntityState.Detached;
            }
        }

    }
}
