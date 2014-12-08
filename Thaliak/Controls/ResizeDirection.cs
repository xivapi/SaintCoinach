using System;
using System.Collections.Generic;
using System.Text;

namespace Kent.Boogaart.Windows.Controls
{
	/// <summary>
	/// Defines possible directions a <see cref="Resizer"/> can resize.
	/// </summary>
	public enum ResizeDirection
	{
		/// <summary>
		/// Size is increased by dragging up and to the right, and decreased by dragging down and to the left.
		/// </summary>
		NorthEast,
		/// <summary>
		/// Size is increased by dragging up and to the left, and decreased by dragging down and to the right.
		/// </summary>
		NorthWest,
		/// <summary>
		/// Size is increased by dragging down and to the right, and decreased by dragging up and to the left.
		/// </summary>
		SouthEast,
		/// <summary>
		/// Size is increased by dragging down and to the left, and decreased by dragging up and to the right.
		/// </summary>
		SouthWest
	}
}
