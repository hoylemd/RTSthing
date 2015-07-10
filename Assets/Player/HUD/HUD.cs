using UnityEngine;
using System.Collections;
using RTS;

public class HUD : MonoBehaviour {

	public GUISkin resourceSkin, ordersSkin, selectBoxSkin;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40,
		SELECTION_NAME_HEIGHT = 30;
	public Player player;

	public Texture2D activeCursor, selectCursor,
		   leftCursor, upCursor, rightCursor, downCursor;
	public Texture2D[] attackCursors, harvestCursors, moveCursors;
	public GUISkin mouseCursorSkin;
	private CursorState activeCursorState;
	private int currentFrame = 0;

	void Awake() {

	}

	// Use this for initialization
	void Start() {
		if (!player) {
			player = transform.root.GetComponent<Player>();
		}

		ResourceManager.StoreSelectBoxItems(selectBoxSkin);

		SetCursorState(CursorState.Select);
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

	void UpdateCursorAnimation() {
		// animate 1 frame / second
		if(activeCursorState == CursorState.Move) {
			currentFrame = (int)Time.time % moveCursors.Length;
			activeCursor = moveCursors[currentFrame];
		} else if(activeCursorState == CursorState.Attack) {
			currentFrame = (int)Time.time % attackCursors.Length;
			activeCursor = attackCursors[currentFrame];
		} else if(activeCursorState == CursorState.Harvest) {
			currentFrame = (int)Time.time % harvestCursors.Length;
			activeCursor = harvestCursors[currentFrame];
		}
	}
	private Rect GetCursorDrawPosition() {
	    //set base position for custom cursor image
	    float leftPos = Input.mousePosition.x;
	    float topPos = Screen.height - Input.mousePosition.y;
	    //adjust position base on the type of cursor being shown
	    if (activeCursorState == CursorState.PanRight) {
			leftPos = Screen.width - activeCursor.width;
	    } else if (activeCursorState == CursorState.PanDown) {
			topPos = Screen.height - activeCursor.height;
	    } else if (activeCursorState == CursorState.Move ||
				activeCursorState == CursorState.Select ||
				activeCursorState == CursorState.Harvest) {
	        topPos -= activeCursor.height / 2;
	        leftPos -= activeCursor.width / 2;
	    }
	    return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
	}

	void DrawMouseCursor() {
		bool mouseOverHud = !MouseInBounds() && activeCursorState != CursorState.PanRight &&
			activeCursorState != CursorState.PanUp;
		if (mouseOverHud) {
			Cursor.visible = true;
		} else {
			Cursor.visible = false;
			GUI.skin = mouseCursorSkin;
			GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height));
			UpdateCursorAnimation();
			Rect cursorPosition = GetCursorDrawPosition();
			GUI.Label(cursorPosition, activeCursor);
			GUI.EndGroup();
		}
	}

	public void SetCursorState(CursorState newState) {
	    activeCursorState = newState;
	    switch(newState) {
		    case CursorState.Select:
		        activeCursor = selectCursor;
		        break;
		    case CursorState.Attack:
		        currentFrame = (int)Time.time % attackCursors.Length;
		        activeCursor = attackCursors[currentFrame];
		        break;
		    case CursorState.Harvest:
		        currentFrame = (int)Time.time % harvestCursors.Length;
		        activeCursor = harvestCursors[currentFrame];
		        break;
		    case CursorState.Move:
		        currentFrame = (int)Time.time % moveCursors.Length;
		        activeCursor = moveCursors[currentFrame];
				break;
			case CursorState.PanLeft:
				activeCursor = leftCursor;
				break;
			case CursorState.PanRight:
				activeCursor = rightCursor;
				break;
			case CursorState.PanUp:
				activeCursor = upCursor;
				break;
			case CursorState.PanDown:
				activeCursor = downCursor;
				break;
			default: break;
		}
	}
	// Update is called once per frame
	void OnGUI() {
		if (player && player.human) {
			DrawOrdersBar();
			DrawResourceBar();
			DrawMouseCursor();
		}
	}

	public bool MouseInBounds() {
		Vector3 mousePosition = Input.mousePosition;
		return (mousePosition.y <= Screen.height - RESOURCE_BAR_HEIGHT&&
				mousePosition.y >= 0) &&
			(mousePosition.x <= Screen.width - ORDERS_BAR_WIDTH &&
				mousePosition.x >= 0);
	}

	public Rect GetPlayingArea() {
		return new Rect(0, RESOURCE_BAR_HEIGHT, Screen.width - ORDERS_BAR_WIDTH,
		                Screen.height - RESOURCE_BAR_HEIGHT);
	}
}
