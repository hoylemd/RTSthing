﻿using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	public GUISkin resourceSkin, ordersSkin;
	private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
	private Player player;

	// Use this for initialization
	void Start() {
		player = transform.root.GetComponent<Player>();
	}

	void DrawOrdersBar() {
		float xOffset = Screen.width - ORDERS_BAR_WIDTH,
			height = Screen.width - RESOURCE_BAR_HEIGHT;
		GUI.skin = ordersSkin;
		GUI.BeginGroup(new Rect(xOffset, RESOURCE_BAR_HEIGHT, ORDERS_BAR_WIDTH, height));
		GUI.Box (new Rect(0, 0, ORDERS_BAR_WIDTH, height), "");
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
}