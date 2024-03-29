//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   Represents the web viewer in the Console which can display HTML
	///   text content as web pages.
	/// </summary>
	/// <!--
	/// <remarks>
	///   The web viewer in the Console interprets the <link LogEntry.Data,
	///   data of a Log Entry> as an HTML website.
	/// </remarks>
	/// <threadsafety>
	///   This class is not guaranteed to be threadsafe.
	/// </threadsafety>
	/// -->

	public class WebViewerContext: TextContext
	{
		/// <summary>
		///   Creates and initializes a WebViewerContext instance.
		/// </summary>

		public WebViewerContext(): base(ViewerId.Web)
		{
		}
	}
}
