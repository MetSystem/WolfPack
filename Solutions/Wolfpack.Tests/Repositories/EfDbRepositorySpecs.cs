using System;
using NUnit.Framework;
using StoryQ;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Core.Testing.Bdd;

namespace Wolfpack.Tests.Repositories
{
    [TestFixture]
    public class EfDbRepositorySpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Ensure that the EF Wolfpack repository works correctly")
                .InOrderTo("interact with EF based storage")
                .AsA("component that performs reading or writing of wolfpack notification data")
                .IWant("these tests to prove that the repository hanldes this")
                .Tag("EntityFramework")
                .Tag("Database");
        }

        [Test]
        public void AddItemToRepository()
        {
            var id = Guid.NewGuid();

            using (var domain = new EfDbRepositoryDomain())
            {
                Feature.WithScenario("Add a new item to the database")
                    .Given(domain.TheWolfpackDatabaseIsClearedOfNotifications)
                    .When(domain.NotificationWithId_IsAddedToTheRepository, id)
                    .Then(domain.TheNotificationWithId_ShouldBeInTheNotificationTable, id)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void ChainedQuery()
        {
            var id = Guid.NewGuid();

            using (var domain = new EfDbRepositoryDomain())
            {
                Feature.WithScenario("chaining multiple queries together")
                    .Given(domain.TheWolfpackDatabaseIsClearedOfNotifications)
                        .And(domain.NotificationWithId_IsAddedToTheRepository, id)
                    .When(domain.TheQueryForId_AndState_IsExecuted, id, MessageStateTypes.NotSet)
                    .Then(domain.TheQueryResultsShouldHave_Items, 1)
                    .ExecuteWithReport();
            }
        }
    }
}