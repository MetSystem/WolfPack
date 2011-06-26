﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Castle.Windsor;
using Wolfpack.Core.AppStats;
using Wolfpack.Core.AppStats.FileQueue;
using Wolfpack.Core.Bus;
using NServiceBus;

namespace Wolfpack.AppStats.Demo
{
    public partial class AppStatsDemoForm : Form
    {
        private AppStatsEngine.AppStatsEventTimer myTimer;

        public AppStatsDemoForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Size = new Size(639, 278);

            uiNsbPublisherOptions.Visible = false;
            uiNsbPublisherOptions.Location = new Point(6, 53);
            uiFileQueuePublisherOptions.Visible = false;
            uiFileQueuePublisherOptions.Location = new Point(6, 53);

            uiPublisherList.Items.Add("NServiceBus");
            uiPublisherList.Items.Add("Local File Queue");
            uiPublisherList.SelectedIndex = 0;

            uiDestinationQueue.Text = "Wolfpackoutput";
            uiDestinationFolder.Text = @"c:\temp\appstats";
            uiTimerStatId.Text = "TestTimerKpi";
            uiCountStatId.Text = "TestCounterKpi";
        }

        private void uiTimerStartStop_Click(object sender, EventArgs e)
        {
            if (uiTimerStartStop.Text == "Timer Start")
            {
                // starts timing
                // usually you would time something inline like this
                using (AppStatsEngine.AppStatsEventTimer timer = AppStatsEngine.Time("SomeOperation"))
                {
                    // some operation to time
                    // the appstat is automatically published
                    // when it is disposed
                }
                myTimer = AppStatsEngine.Time(uiTimerStatId.Text);
                uiTimerStartStop.Text = "Timer Stop";
            }
            else
            {
                // stops timing
                // this will flush the timing data to the bus
                myTimer.Dispose();
                myTimer = null;
                uiTimerStartStop.Text = "Timer Start";
            }
        }

        private void uiCount_Click(object sender, EventArgs e)
        {
            // example of a simple count
            AppStatsEngine.One(uiCountStatId.Text);
        }

        private void uiCountTen_Click(object sender, EventArgs e)
        {
            // example of a simple count with a context, this 
            // appears in the tag property of the healthcheck
            AppStatsEngine.Count(10, uiCountStatId.Text, "@{0}", DateTime.UtcNow);
        }

        private void uiVote_Click(object sender, EventArgs e)
        {
            string segmentId = "Don't know";

            if (uiVoteA.Checked)
                segmentId = uiVoteA.Text;
            else if (uiVoteB.Checked)
                segmentId = uiVoteB.Text;
            else if (uiVoteC.Checked)
                segmentId = uiVoteC.Text;

            // using the piechart extension allows us to set up the
            // underlying result properties in a more natural way.
            // We can then use the Geckoboard Data Service to visualise
            // this data with a Geckoboard piechart widget.
            //
            // The Wolfpack Geckoboard Data Service url to use in 
            // your geckoboard widget is...
            // 
            AppStatsEngine.Publish(new AppStatsEvent()
                                       .PieChart("WolfpackPoll")
                                       .Segment(segmentId)
                                       .One());
        }

        private void uiPublisherSelectNext_Click(object sender, EventArgs e)
        {
            switch (uiPublisherList.SelectedIndex)
            {
                case 0:
                    // these are a pretend bus & container
                    // to demo where you could use them if you already
                    // have these in your application (otherwise AppStats
                    // will create it's own instances)
                    IBus myBus = null;
                    IWindsorContainer myContainer = null;

                    AppStatsEngine.Initialise(AppStatsConfigBuilder.For("AppStatsDemo")
                                                  // if you already have a bus
                                                  //.PublishWith(myBus)

                                                  // if you need a bus...
                                                  // then use the busbuilder
                                                  .PublishWith(BusBuilder.ForApplication()
                                                                   // use this if you already have a container 
                                                                   // (otherwise the default NSB container is used)
                                                                   //.UseContainer(myContainer)

                                                                   // if you want to customise the msmq settings
                                                                   // then use this method to do it otherwise the
                                                                   // default queues (non transactional) are used
                                                                   //.Msmq("AlternateInput", "AlternateError")

                                                                   // that's it - start it!
                                                                   .FireItUp(),
                                                               // specify the destination queue for messages
                                                               // here otherwise AppStats will assume you 
                                                               // have configured this routing in external
                                                               // configuration for the Send() method
                                                               uiDestinationQueue.Text).Build());
                    break;

                case 1:
                    AppStatsEngine.Initialise(AppStatsConfigBuilder.For("AppStatsDemo")
                                                  .PublishWith(uiDestinationFolder.Text)
                                                  .Build());
                    break;
            }

            uiPublisherSelectPanel.Visible = false;
            uiFinalPanel.Location = new Point(2, 2);
        }

        private void uiPublisherList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (uiPublisherList.SelectedIndex)
            {
                case 0:
                    uiNsbPublisherOptions.Visible = true;
                    uiFileQueuePublisherOptions.Visible = false;
                    break;
                case 1:
                    uiNsbPublisherOptions.Visible = false;
                    uiFileQueuePublisherOptions.Visible = true;
                    break;
            }
        }
    }
}