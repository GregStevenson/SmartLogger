//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;
using System.Text;
using System.IO;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   Used for sending packets to a local SmartInspect Console over a
	///   named pipe connection.
	/// </summary>
	/// <!--
	/// <remarks>
	///   This class is used for sending packets through a local named pipe
	///   to the SmartInspect Console. It is used when the 'pipe' protocol
	///   is specified in the <link SmartInspect.Connections,
	///   connections string>. Please see the IsValidOption method for a
	///   list of available protocol options. Please note that this protocol
	///   can only be used for local connections. For remote connections to
	///   other machines, please use TcpProtocol.
	/// </remarks>
	/// <threadsafety>
	///   The public members of this class are threadsafe.
	/// </threadsafety>
	/// -->

	public class PipeProtocol: Protocol
	{
		private static readonly byte[] CLIENT_BANNER =
			Encoding.ASCII.GetBytes("SmartInspect .NET Library v" +
			SmartInspect.Version + "\n");

		private string fPipeName;
		private Stream fStream;
		private Formatter fFormatter;

		/// <summary>
		///   Creates and initializes a PipeProtocol instance. For a list
		///   of available pipe protocol options, please refer to the
		///   IsValidOption method.
		/// </summary>

		public PipeProtocol()
		{
			this.fFormatter = new BinaryFormatter();
			LoadOptions();
		}

		/// <summary>
		///   Overridden. Returns "pipe".
		/// </summary>

		protected override string Name
		{
			get { return "pipe"; }
		}

		/// <summary>
		///   Overridden. Validates if a protocol option is supported.
		/// </summary>
		/// <param name="name">The option name to validate.</param>
		/// <returns>
		///   True if the option is supported and false otherwise.
		/// </returns>
		/// <!--
		/// <remarks>
		///   The following table lists all valid options, their default
		///   values and descriptions for the pipe protocol.
		///   
		///   <table>
		///   Valid Options  Default Value   Description
		///   +              +               +
		///   pipename       "smartinspect"  Specifies the named pipe 
		///                                   for sending log packets to
		///                                   the SmartInspect Console.
		///   </table>
		///   
		///   For further options which affect the behavior of this
		///   protocol, please have a look at the documentation of the
		///   <link Protocol.IsValidOption, IsValidOption> method of the
		///   parent class.
		/// </remarks>
		/// <example>
		/// <code>
		/// SiAuto.Si.Connections = "pipe()";
		/// SiAuto.Si.Connections = "pipe(pipename=\\"logging\\")";
		/// </code>
		/// </example>
		/// -->

		protected override bool IsValidOption(string name)
		{
			return
				name.Equals("pipename") ||
				base.IsValidOption(name);
		}

		/// <summary>
		///   Overridden. Fills a ConnectionsBuilder instance with the
		///   options currently used by this pipe protocol.
		/// </summary>
		/// <param name="builder">
		///   The ConnectionsBuilder object to fill with the current options
		///   of this protocol.
		/// </param>

		protected override void BuildOptions(ConnectionsBuilder builder)
		{
			base.BuildOptions(builder);
			builder.AddOption("pipename", this.fPipeName);
		}

		/// <summary>
		///   Overridden. Loads and inspects pipe specific options.
		/// </summary>
		/// <!--
		/// <remarks>
		///   This method loads all relevant options and ensures their
		///   correctness. See IsValidOption for a list of options which
		///   are recognized by the pipe protocol.
		/// </remarks>
		/// -->

		protected override void LoadOptions()
		{
			base.LoadOptions();
			this.fPipeName = GetStringOption("pipename", "smartinspect");
		}

		/// <summary>
		///   Overridden. Connects to the specified local named pipe.
		/// </summary>
		/// <!--
		/// <remarks>
		///   This method tries to establish a connection to a local named
		///   pipe of a SmartInspect Console. The name of the pipe can be
		///   specified by passing the "pipename" option to the Initialize
		///   method.
		/// </remarks>
		/// <exception>
		/// <table>
		///   Exception Type      Condition
		///   +                   +
		///   Exception           Establishing the named pipe connection
		///                        failed.
		/// </table>
		/// </exception>
		/// -->

		protected override void InternalConnect()
		{
			PipeHandle handle = new PipeHandle(this.fPipeName);
			this.fStream = new BufferedStream(new PipeStream(handle));
			DoHandShake(this.fStream);
			InternalWriteLogHeader(); /* Write a log header */
		}

		/// <summary>
		///   Overriden. Tries to reconnect to the specified local named
		///   pipe.
		/// </summary>
		/// <returns>
		///   True if the reconnect attempt has been successful and false
		///   otherwise.
		/// </returns>
		/// <!--
		/// <remarks>
		///   This method tries to (re-)establish a connection to the local
		///   named pipe of a SmartInspect Console. The name of the pipe can
		///   be specified by passing the "pipename" option to the Initialize
		///   method.
		/// </remarks>
		/// -->

		protected override bool InternalReconnect()
		{
			/* Faster than InternalConnect because this will not
			 * always throw an exception if the pipe server is down
			 * or cannot handle the client request. */

			PipeHandle handle = new PipeHandle(this.fPipeName);

			if (handle.IsInvalid)
			{
				return false;
			}

			this.fStream = new BufferedStream(new PipeStream(handle));
			DoHandShake(this.fStream);
			InternalWriteLogHeader(); /* Write a log header */

			return true;
		}

		private static void DoHandShake(Stream stream)
		{
			int n;

			// Read the server banner from the Console. 
			while ((n = stream.ReadByte()) != '\n')
			{
				if (n == -1)
				{
					// This indicates a failure on the server
					// side. Doesn't make sense to proceed here.

					throw new SmartInspectException(
							"Could not read server banner correctly: " +
							"Connection has been closed unexpectedly"
						);
				}
			}

			// And write ours in return!
			stream.Write(CLIENT_BANNER, 0, CLIENT_BANNER.Length);
			stream.Flush();
		}

		/// <summary>
		///   Overridden. Sends a packet to the Console.
		/// </summary>
		/// <param name="packet">The packet to write.</param>
		/// <!--
		/// <remarks>
		///   This method sends the supplied packet to the SmartInspect
		///   Console over the previously established named pipe connection.
		/// </remarks>
		/// <exception>
		/// <table>
		///   Exception Type      Condition
		///   +                   +
		///   Exception           Sending the packet to the Console failed.
		/// </table>
		/// </exception>
		/// -->

		protected override void InternalWritePacket(Packet packet)
		{
			this.fFormatter.Format(packet, this.fStream);
			this.fStream.Flush();
		}

		/// <summary>
		///   Overridden. Closes the connection to the specified local
		///   named pipe.
		/// </summary>
		/// <!--
		/// <remarks>
		///   This method closes the named pipe handle if previously created
		///   and disposes any supplemental objects.
		/// </remarks>
		/// <exception>
		/// <table>
		///   Exception Type         Condition
		///   +                      +
		///   Exception              Closing the named pipe handle failed.
		/// </table>
		/// </exception>
		/// -->

		protected override void InternalDisconnect()
		{
			if (this.fStream != null)
			{
				this.fStream.Close();
				this.fStream = null;
			}
		}
	}
}
