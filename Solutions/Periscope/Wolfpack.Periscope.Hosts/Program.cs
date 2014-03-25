using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wolfpack.Periscope.Hosts
{
    class Program
    {
        static void Main(string[] args)
        {
            //HostFactory.Run(
            //    config =>
            //    {
            //        config.SetDisplayName("Wolfpack Agent");
            //        config.SetServiceName("WolfpackAgent");
            //        config.SetDescription("Wolfpack Agent Service");

            //        string username;
            //        string password;

            //        if (CmdLine.Value(CmdLine.SwitchNames.Username, out username) &&
            //            CmdLine.Value(CmdLine.SwitchNames.Password, out password))
            //        {
            //            Logger.Debug("Running As: {0}", username);
            //            config.RunAs(username, password);
            //        }

            //        config.Service<IRolePlugin>(service =>
            //        {
            //            service.SetServiceName("Wolfpack");
            //            service.ConstructUsing(factory => role);
            //            service.WhenStarted(s => s.Start());
            //            service.WhenStopped(s => s.Stop());
            //        });

            //        config.ApplyCommandLine(string.Join(" ", CmdLine.Expanded.ToArray()));
            //    });

        }
    }
}
