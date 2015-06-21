using UnityEngine;
using System.Collections;
using RTS;

public class WorldObject : MonoBehaviour {
    public string objectName;
    public Texture2D buildImage;
    public int cost, sellValue, hitPoints, maxHitPoints;

    protected Player player;
    protected string[] actions = {};
    protected bool currentlySelected = false;

    protected Bounds selectionBounds;
    protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    protected virtual void Awake() {
        selectionBounds = ResourceManager.InvalidBounds;
        CalculateBounds();
    }

    // Use this for initialization
    protected virtual void Start() {
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    protected virtual void Update() {

    }

    private void ChangeSelection(WorldObject worldObject, Player controller) {
        SetSelection(false, playingArea);
        if (controller.selectedObject) {
            controller.selectedObject.SetSelection(false, playingArea);
        }
        controller.selectedObject = worldObject;
        worldObject.SetSelection(true, controller.hud.GetPlayingArea());
    }

    public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint,
            Player controller) {
        // only handle click if selected
        if (currentlySelected && hitObject && hitObject.name != "Ground") {
            WorldObject worldObject =
                hitObject.transform.parent.GetComponent<WorldObject>();
            // clicked on another world object
            if (worldObject) ChangeSelection(worldObject, controller);
        }
    }

    public void CalculateBounds() {
        selectionBounds = new Bounds(transform.position, Vector3.zero);
        foreach(Renderer r in GetComponentsInChildren<Renderer>()) {
            selectionBounds.Encapsulate(r.bounds);
        }
    }

    protected virtual void DrawSelectionBox(Rect selectBox) {
        GUI.Box(selectBox, "");
    }

    private void DrawSelection() {
        GUI.skin = ResourceManager.SelectBoxSkin;
        Rect selectionBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
        // Draw box
        GUI.BeginGroup(playingArea);
        DrawSelectionBox(selectionBox);
        GUI.EndGroup();
    }

    protected virtual void OnGUI() {
        if (currentlySelected) {
            DrawSelection();
        }
    }

    public void SetSelection(bool selected, Rect playingArea) {
        currentlySelected = selected;
        if (selected) {
            this.playingArea = playingArea;
        }
    }

    public virtual void SetHoverState(GameObject hoverObject) {
        // only handle if player is human, and this object is selected
        if (player && player.human && currentlySelected) {
            if (hoverObject.name != "Ground") {
                player.hud.SetCursorState(CursorState.Select);

            }
        }
    }

    public string[] GetActions() {
        return actions;
    }

    public virtual void PerformAction(string action) {
        // children will hande
    }
}
