//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->>>>
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   Specifies the log rotate mode for the FileProtocol class and
	///   derived classes.
	/// </summary>

	public enum FileRotate
	{
		/// <summary>
		///   Completely disables the log rotate functionality.
		/// </summary>

		None,

		/// <summary>
		///   Instructs the file protocol to rotate log files hourly.
		/// </summary>

		Hourly,

		/// <summary>
		///   Instructs the file protocol to rotate log files daily.
		/// </summary>

		Daily,

		/// <summary>
		///   Instructs the file protocol to rotate log files weekly.
		/// </summary>
		
		Weekly,

		/// <summary>
		///   Instructs the file protocol to rotate log files monthly.
		/// </summary>
		
		Monthly
	}
}
