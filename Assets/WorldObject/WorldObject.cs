using UnityEngine;
using System.Collections;

public class WorldObject : MonoBehaviour {
	public string objectName;
	public Texture2D buildImage;
	public int cost, sellValue, hitPoints, maxHitPoints;

	protected Player player;
	protected string[] actions = {};
	protected bool currentlySelected = false;

	protected virtual void Awake() {
	}

	// Use this for initialization
	protected virtual void Start() {
	}

	// Update is called once per frame
	protected virtual void Update() {

	}

	private void ChangeSelection(WorldObject worldObject, Player controller) {
		SetSelection(false);
		if (controller.selectedObject) {
			controller.selectedObject.SetSelection(false);
		}
		controller.selectedObject = worldObject;
		worldObject.SetSelection(true);
	}

	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint,
			Player controller) {
		// only handle click if selected
		if (currentlySelected && hitObject && hitObject.name != "Ground") {
			WorldObject worldObject =
				hitObject.transform.root.GetComponent<WorldObject>();
			// clicked on another world object
			if (worldObject) ChangeSelection(worldObject, controller);
		}

	}

	protected virtual void OnGUI() {
	}

	public void SetSelection(bool selected) {
		currentlySelected = selected;
	}

	public string[] GetActions() {
		return actions;
	}

	public virtual void PerformAction(string action) {
		// children will hande
	}
}
