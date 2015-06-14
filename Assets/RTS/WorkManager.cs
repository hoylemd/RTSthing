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

			// determine screen coords
			List< Vector3 > corners = new List< Vector3 >();
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex, cy+ey, cz+ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex, cy+ey, cz-ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex, cy-ey, cz+ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex, cy+ey, cz+ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx+ex, cy-ey, cz-ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex, cy-ey, cz+ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex, cy+ey, cz-ez)));
			corners.Add(Camera.main.WorldToScreenPoint(new Vector3(cx-ex, cy-ey, cz-ez)));

			// Determine the bounds on screen for the selection bounds
			Bounds screenBounds = new Bounds(corners[0], Vector3.zero);
			for(int i = 1; i < corners.Count; i++) {
				screenBounds.Encapsulate(corners[i]);
			}

			// Adjust the bounds for the bottom-left origin of the screen
			float selectBoxTop = playingArea.height - (screenBounds.center.y + screenBounds.extents.y);
			float selectBoxLeft = screenBounds.center.x - screenBounds.extents.x;
			float selectBoxWidth = 2 * screenBounds.extents.x;
			float selectBoxHeight = 2 * screenBounds.extents.y;

			return new Rect(selectBoxLeft, selectBoxTop, selectBoxWidth, selectBoxHeight);
		}
	}
}
