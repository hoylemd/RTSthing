using UnityEngine;
using System.Collections.Generic;

namespace RTS {
	public static class WorkManager {
		public static Rect CalculateSelectionBox(Bounds selectionBounds, Rect playingArea) {
			// shorthand for selection bounds center coords
			float cx = selectionBounds.center.x,
				cy = selectionBounds.center.y,
				cz = selectionBounds.center.z;
			// shorthand for selection bounds extent
			float ex = selectionBounds.extents.x,
				ey = selectionBounds.extents.y,
				ez = selectionBounds.extents.x;
		}

	}
}
