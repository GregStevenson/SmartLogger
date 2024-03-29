//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   This is the event handler type for the SmartInspect.Filter event.
	/// </summary>
	/// <param name="sender">The object which fired the event.</param>
	/// <param name="e">
	///   A FilterEventArgs argument which offers the possibility of
	///   retrieving information about the packet and canceling its
	///   processing.
	/// </param>
	/// <!--
	/// <remarks>
	///   In addition to the sender parameter, a FilterEventArgs argument
	///   will be passed to the event handlers which offers the possibility
	///   of retrieving information about the packet and canceling its
	///   processing.
	/// </remarks>
	/// -->

	public delegate void FilterEventHandler(object sender, FilterEventArgs e);
}
