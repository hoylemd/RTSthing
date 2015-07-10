using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public string username;
	public bool human;
	public WorldObject selectedObject {get; set;}

	public HUD hud;

	// Use this for initialization
	void Start () {
		if (!hud) {
			hud = GetComponentInChildren<HUD>();
		}
	}

	// Update is called once per frame
	void Update () {
	}

	public void deselect() {
		selectedObject.SetSelection(false, hud.GetPlayingArea());
		selectedObject = null;
	}
}
