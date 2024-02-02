//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   Represents the graphic viewer in the Console which can display
	///   images.
	/// </summary>
	/// <!--
	/// <remarks>
	///   The graphic viewer in the Console interprets the <link LogEntry.Data,
	///   data of a Log Entry> as picture.
	/// </remarks>
	/// <threadsafety>
	///   This class is not guaranteed to be threadsafe.
	/// </threadsafety>
	/// -->

	public class GraphicViewerContext: BinaryContext
	{
		/// <summary>
		///   Creates and initializes a GraphicViewerContext instance. 
		/// </summary>
		/// <param name="id">The graphic ID to use.</param>

		public GraphicViewerContext(GraphicId id): base((ViewerId) id)
		{
		}
	}
}
