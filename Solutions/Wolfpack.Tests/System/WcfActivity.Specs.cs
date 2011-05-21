using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoryQ;
using Wolfpack.Core.Interfaces.Entities;
using Wolfpack.Tests.Bdd;

namespace Wolfpack.Tests.System
{
    [TestFixture]
    public class WcfActivitySpecs : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Enabling distributed agents to communicate with a central server agent via WCF")
                .InOrderTo("receive data from a remote agent")
                .AsA("server agent")
                .IWant("to use a WCF communications channel");
        }

        [Test]
        public void HappyPath()
        {
            using (var domain = new WcfActivityDomain(new WcfActivityDomainConfig
                                                          {
                                                              Uri = "http://localhost:802/Wolfpack",
                                                              SessionMessage = new HealthCheckAgentStart
                                                                                   {
                                                                                       Activities = new List<PluginDescriptor>(),
                                                                                       Agent = new AgentInfo
                                                                                                   {
                                                                                                     AgentId = "TestAgent",
                                                                                                     SiteId = "TestSiteId"
                                                                                                   },
                                                                                                   Checks = new List<PluginDescriptor>(),
                                                                                                   DiscoveryStarted = DateTime.UtcNow,
                                                                                                   DiscoveryCompleted = DateTime.UtcNow,
                                                                                                   UnhealthyActivities = new List<PluginDescriptor>(),
                                                                                                   UnhealthyChecks = new List<PluginDescriptor>()
                                                                                   }
                                                          }))
            {
                Feature.WithScenario("A session start message is sent and received")
                    .Given(domain.TheActivityIsCorrectlyConfigured)
                    .And(domain.TheAgentIsStarted)
                    .When(domain.TheSessionStartMessageIsSent)
                    .Then(domain.ThereShouldBe_SessionMessagesReceived, 2)
                        .And(domain.TheSessionMessageAtIndex_ShouldExactlyMatchTheOneSent, 1)
                    .ExecuteWithReport();
            }
        }
    }
}