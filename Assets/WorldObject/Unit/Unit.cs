using UnityEngine;
using System.Collections;
using RTS;

public class Unit : WorldObject {

    public bool moving, rotating;

    private Vector3 destination;
    private Quaternion targetRotation;

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
            if (hoverObject.name == "Ground") player.hud.SetCursorState(CursorState.Move);
        }
    }

    protected override void OnGUI() {
        base.OnGUI();
    }

    public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller) {
        base.MouseClick(hitObject, hitPoint, controller);

        if (player && player.human && currentlySelected) {
            if (hitObject.name == "Ground" && hitPoint != ResourceManager.InvalidPosition) {
                StartMove(new Vector3(
                    hitPoint.x,
                    hitPoint.y + GetComponent<Transform>().position.y,
                    hitPoint.z));
            }
        }
    }

    private void StartMove(Vector3 dest) {
        destination = dest;
        targetRotation = Quaternion.LookRotation(
            destination - GetComponent<Transform>().position);
        rotating = true;
        moving = false;
    }
}
