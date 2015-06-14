using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour {
	private Player player;
	private int scrollRightLimit = 0;
	private int scrollDownLimit = 0;

	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<Player>();
		scrollRightLimit = Screen.width - ResourceManager.ScrollWidth;
		scrollDownLimit = Screen.height - ResourceManager.ScrollWidth;
	}

	private void MoveCamera() {
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		Vector3 movement = new Vector3(0, 0, 0);

		// horizontal
		if (xpos >= 0 && xpos < ResourceManager.ScrollWidth) {
			movement.x -= ResourceManager.ScrollSpeed;
		} else if (xpos <= Screen.width && xpos > scrollRightLimit){
			movement.x += ResourceManager.ScrollSpeed;
		}
		// vertical
		if (ypos >= 0 && ypos < ResourceManager.ScrollWidth) {
			movement.z -= ResourceManager.ScrollSpeed;
		} else if (ypos <= Screen.height && ypos > scrollDownLimit){
			movement.z += ResourceManager.ScrollSpeed;
		}

		// transform movement to be relative tro camera orientation, but lock y coord
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;

		// up/down movement
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;

		// limit up/down movement
		if (destination.y > ResourceManager.MaxCameraHeight) {
			destination.y = ResourceManager.MaxCameraHeight;
		} else if (destination.y < ResourceManager.MinCameraHeight) {
			destination.y = ResourceManager.MinCameraHeight;
		}

		if (destination != origin) {
			Camera.main.transform.position = Vector3.MoveTowards(origin,
				destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}
	}

	private void RotateCamera() {
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;

		// detect rotation inf Alt & RMB pressed
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
				Input.GetMouseButton(1)) {
			destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}

		if (destination != origin) {
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, ResourceManager.RotateSpeed);
		}
	}

	private void LeftClick() {
		if (player.hud.MouseInBounds()) {
			GameObject hitObject = FindHitObject();
			Vector3 hitPoint = FindHitPoint();
			if (hitObject && hitPoint != ResourceManager.InvalidPosition) {
				if (player.selectedObject) player.selectedObject.MouseClick(hitObject, hitPoint, player);
				else if (hitObject.name != "Ground") {
					WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
					if (worldObject) {
						player.selectedObject = worldObject;
						worldObject.SetSelection(true, player.hud.GetPlayingArea());
					}
				}

			}
		}

	}

	private void RightClick() {
		if (player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.selectedObject) {
			player.deselect();
		}
	}

	private void MouseActivity() {
		if (Input.GetMouseButtonDown(0)) {
			LeftClick();
		}
		if (Input.GetMouseButtonDown(1)) {
			RightClick();
		}
	}

	private GameObject FindHitObject() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
		return null;
	}

	private Vector3 FindHitPoint() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) return hit.point;
		return ResourceManager.InvalidPosition;
	}

	// Update is called once per frame
	void Update () {
		if(player.human) {
			MoveCamera();
			RotateCamera();
			MouseActivity();
		}

	}
}
