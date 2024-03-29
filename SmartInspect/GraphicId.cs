//
// <!-- Copyright (C) Code Partners Pty Ltd. All rights reserved. -->
//

using System;

namespace Gurock.SmartInspect
{
	/// <summary>
	///   Used by the GraphicViewerContext class to specify the desired
	///   picture type.
	/// </summary>

	public enum GraphicId
	{
		/// <summary>
		///   Instructs the GraphicViewerContext class to treat the data
		///   as bitmap image.
		/// </summary>

		Bitmap = (int) ViewerId.Bitmap,

		/// <summary>
		///   Instructs the GraphicViewerContext class to treat the data
		///   as JPEG image.
		/// </summary>

		Jpeg = (int) ViewerId.Jpeg,

		/// <summary>
		///   Instructs the GraphicViewerContext class to treat the data
		///   as Window icon.
		/// </summary>

		Icon = (int) ViewerId.Icon,

		/// <summary>
		///   Instructs the GraphicViewerContext class to treat the data
		///   as Window Metafile image.
		/// </summary>

		Metafile = (int) ViewerId.Metafile
	}
}
