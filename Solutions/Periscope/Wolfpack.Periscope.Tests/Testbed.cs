using Moq;
using NUnit.Framework;
using StoryQ;
using Wolfpack.Periscope.Core.Interfaces;
using Wolfpack.Periscope.Core.Interfaces.Entities;
using Wolfpack.Periscope.Core.Repositories.Preset;
using Wolfpack.Periscope.Core.Widgets.Highcharts;
using Wolfpack.Periscope.Tests.Bdd;
using System.Linq;

namespace Wolfpack.Periscope.Tests
{
    [TestFixture]
    public class Testbed
    {
        [Test]
        public void Test()
        {
            var sut = DashboardConfigurationBuilder.New();

            sut.Add("TestPanel1", (infra, builder)
                                  => builder.Add<HighchartPieChart, WidgetConfiguration>(widget
                                                                    =>
                                                                        {
                                                                            widget.Name = "Piechart1";
                                                                        })
                                         .Add<HighchartBarChart, WidgetConfiguration>(widget
                                                                 =>
                                                                     {
                                                                         widget.Name = "Barchar1";
                                                                     })
                                         .SetDwellTime(20));

            var infrastructure = new Mock<IDashboardInfrastructure>();
            var config = sut.Build(infrastructure.Object);

            Assert.That(config.Panels.Count(), Is.EqualTo(1));
            Assert.That(config.Panels.First().Widgets.Count(), Is.EqualTo(2));
        }
    }

    [TestFixture]
    public class DashboardTest : BddFeature
    {
        protected override Feature DescribeFeature()
        {
            return new Story("Core dashboard boots up and starts the clock running")
                .InOrderTo("use the dashboard")
                .AsA("user")
                .IWant("the dashboard to start")
                .Tag("core")
                .Tag("clock");
        }

        [Test]
        public void TheDashboardStartsAndTheClockTicks()
        {
            using (var domain = new SystemDomain())
            {
                Feature.WithScenario("happy path")
                    .Given(domain.WeCreateAPanelNamed_WithSomeDefaultWidgets, "Test")
                    .When(domain.TheDashboardIsStarted)
                        .And(domain.WeWait_Seconds, 650000)
                    .Then(domain.ThereShouldBe_ClockTickEventsGenerated, 5)
                    .ExecuteWithReport();
            }
        }

        [Test]
        public void TheDashboardIsHostedAndStartedAndRendered()
        {
            using (var domain = new SystemDomain())
            {
                Feature.WithScenario("dashboard hosted, started and a client call made to display it")
                    .Given(domain.WeCreateAPanelNamed_WithSomeDefaultWidgets, "Test")
                    .When(domain.TheDashboardIsStarted)
                        .And(domain.ThePanelRequestApiCallIsMade)
                    .Then(domain.ThePanelShouldContain_Widgets, 2)
                    .ExecuteWithReport();
            }
        }
    }
}