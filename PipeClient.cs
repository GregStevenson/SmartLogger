using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogger
{
    public class PipeClient
    {
        public string LogStatusFromApplication()
        {
            var pipeClient =
                new NamedPipeClientStream(".", "testpipe",
                PipeDirection.InOut, PipeOptions.None,
                TokenImpersonationLevel.Impersonation);

            pipeClient.Connect();

            StreamString ss = new StreamString(pipeClient);
            string json = ss.ReadString();
            pipeClient.Close();
            return "garbage";
        }
    }
}
