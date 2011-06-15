using NUnit.Framework;
using StoryQ;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.Bridges
{
    [TestFixture]
    public class FileSystemBridgeSpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Bridge between two wolfpack agents")
                .InOrderTo("Transfer agent data")
                .AsA("Sys Admin")
                .IWant("To be able to use the filesystem as the communication mechanism");
        }

        [Test]
        public void HappyPath()
        {
            using (var domain = new FileSystemBridgeDomain(new FileSystemBridgeDomainConfig()
                                                                        {
                                                                            Folder = @"filequeue"
                                                                        }))
            {
                Feature.WithScenario("Happy Path")
                    .Given(domain.TheBridgeComponents)
                        .And(domain.ThePublisherIsInvoked)
                    .When(domain.TheConsumerIsInvoked)
                    .Then(domain.ShouldHavePublished_Messages, 1)
                    .ExecuteWithReport();
            }
        }        
    }
}