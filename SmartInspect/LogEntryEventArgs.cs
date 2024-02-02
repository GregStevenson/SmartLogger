//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   This class is used by the SmartInspect.LogEntry event.
	/// </summary>
	/// <!--
	/// <remarks>
	///   It has only one public class member named LogEntry. This member
	///   is a property, which just returns the sent packet.
	/// </remarks>
	/// <threadsafety>
	///   This class is fully threadsafe.
	/// </threadsafety>
	/// -->

	public sealed class LogEntryEventArgs: System.EventArgs
	{
		private LogEntry fLogEntry;

		/// <summary>
		///   Creates and initializes a LogEntryEventArgs instance.
		/// </summary>
		/// <param name="logEntry">
		///   The Log Entry packet which caused the event.
		/// </param>

		public LogEntryEventArgs(LogEntry logEntry)
		{
			this.fLogEntry = logEntry;
		}

		/// <summary>
		///   This read-only property returns the LogEntry packet,
		///   which has just been sent.
		/// </summary>

		public LogEntry LogEntry
		{
			get { return this.fLogEntry; }
		}
	}
}
