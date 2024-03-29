//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   This is the event handler type for the SmartInspect.ProcessFlow
	///   event.
	/// </summary>
	/// <param name="sender">The object which fired the event.</param>
	/// <param name="e">
	///   A ProcessFlowEventArgs argument which offers the possibility of
	///   retrieving information about the sent packet.
	/// </param>
	/// <!--
	/// <remarks>
	///   In addition to the sender parameter, a ProcessFlowEventArgs argument
	///   will be passed to the event handlers which offers the possibility
	///   of retrieving information about the sent packet.
	/// </remarks>
	/// -->

	public delegate void ProcessFlowEventHandler(object sender,
		ProcessFlowEventArgs e);
}
