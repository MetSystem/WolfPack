using System;
using System.ServiceProcess;
using Wolfpack.Core;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;
using System.Linq;

namespace Wolfpack.Manager.Commands
{
    public class RestartServiceCommand : ISystemCommand
    {
        private RestartServiceInstruction _instruction;

        public RestartServiceCommand(RestartServiceInstruction instruction)
        {
            _instruction = instruction;
        }

        public void Execute()
        {
            Logger.Info("Stopping Wolfpack...");
            var wolfpack = ServiceController.GetServices().FirstOrDefault(s => 
                s.ServiceName.Equals("wolfpack", StringComparison.OrdinalIgnoreCase));

            if (wolfpack == null)
                throw new InvalidOperationException(string.Format("Wolfpack service not installed :-S"));


            wolfpack.Stop();

            Logger.Info("Starting Wolfpack...");
            wolfpack.Start();
        }
    }
}