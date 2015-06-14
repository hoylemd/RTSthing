using UnityEngine;
using System.Collections;
using RTS;

public class HUD : MonoBehaviour {

	public GUISkin resourceSkin, ordersSkin, selectBoxSkin;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40,
		SELECTION_NAME_HEIGHT = 30;
	private Player player;

	// Use this for initialization
	void Start() {
		player = transform.root.GetComponent<Player>();

		ResourceManager.StoreSelectBoxItems(selectBoxSkin);
	}

	void DrawOrdersBar() {
		float xOffset = Screen.width - ORDERS_BAR_WIDTH,
			height = Screen.width - RESOURCE_BAR_HEIGHT;
		GUI.skin = ordersSkin;
		GUI.BeginGroup(new Rect(xOffset, RESOURCE_BAR_HEIGHT,
								ORDERS_BAR_WIDTH, height));
		GUI.Box (new Rect(0, 0, ORDERS_BAR_WIDTH, height), "");

		// Draw Selection
		string selectionName = "";
		if (player.selectedObject) {
			selectionName = player.selectedObject.objectName;
		}
		if (!selectionName.Equals("")) {
			GUI.Label(new Rect(0, 10, ORDERS_BAR_WIDTH, SELECTION_NAME_HEIGHT),
					  selectionName);
		}

		GUI.EndGroup();
	}

	void DrawResourceBar() {
		Rect barArea = new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT);
		GUI.skin = resourceSkin;
		GUI.BeginGroup(barArea);
		GUI.Box (barArea, "");
		GUI.EndGroup();
	}

	// Update is called once per frame
	void OnGUI() {
		if (player && player.human) {
			DrawOrdersBar();
			DrawResourceBar();
		}
	}

	public bool MouseInBounds() {
		Vector3 mousePosition = Input.mousePosition;
		return (mousePosition.y <= Screen.height - RESOURCE_BAR_HEIGHT&&
				mousePosition.y >= 0) &&
			(mousePosition.x <= Screen.width - ORDERS_BAR_WIDTH &&
				mousePosition.x >= 0);
	}
}
