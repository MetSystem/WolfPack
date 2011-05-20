using System;
using System.Configuration;
using System.Windows.Forms;
using Castle.Windsor;
using Wolfpack.Core.AppStats;
using Wolfpack.Core.AppStats.FileQueue;
using Wolfpack.Core.Bus;
using NServiceBus;

namespace Wolfpack.AppStats.Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppStatsDemoForm());
        }
    }
}
