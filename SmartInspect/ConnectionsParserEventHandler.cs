//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   This is the callback type for the ConnectionsParser.Parse method.
	/// </summary>
	/// <param name="sender">The object which fired the event.</param>
	/// <param name="e">
	///   A ConnectionsParserEventArgs argument which offers the possibility
	///   of retrieving information about the found protocol and its options.
	/// </param>
	/// <!--
	/// <remarks>
	///   In addition to the sender parameter, a ConnectionsParserEventArgs
	///   argument will be passed to the event handlers which offers the
	///   possibility of retrieving information about the found protocol.
	/// </remarks>
	/// -->

	public delegate void ConnectionsParserEventHandler(object sender,
		ConnectionsParserEventArgs e);
}
