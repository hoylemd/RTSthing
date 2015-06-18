using UnityEngine;
using System.Collections;
using RTS;

public class Unit : WorldObject {

    /*** Game Engine methods, all can be overridden by subclass ***/

    protected override void Awake() {
		base.Awake();
	}

    protected override void Start () {
		base.Start();
	}

    protected override void Update () {
		base.Update();
	}

	public override void SetHoverState(GameObject hoverObject) {
		base.SetHoverState(hoverObject);

		if (player && player.human && currentlySelected) {
			if(hoverObject.name == "Ground") player.hud.SetCursorState(CursorState.Move);
		}
	}

    protected override void OnGUI() {
		base.OnGUI();
	}
}
