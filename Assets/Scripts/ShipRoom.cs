using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShipRoom : MonoBehaviour {

    public TeleportMarkerBase[] teleportSurfaces;
    public bool isInRoom;

    public static ShipRoom currentRoom;

    private void Awake()
    {
        if(teleportSurfaces.Length == 0)
        {
            Debug.Log("Ship room has no teleport surfaces set up.");
        }

        if (isInRoom)
        {
            if(currentRoom == null)
            {
                currentRoom = this;
            }
            else
            {
                Debug.Log("Multiple rooms set up to be the starting room for the scene. Some will be ignored." + gameObject.name + "." + transform.parent.name);
                isInRoom = false;
            }
        }
    }

    // Use this for initialization
    void Start () {
		foreach(TeleportMarkerBase surface in teleportSurfaces)
        {
            surface.markerActive = isInRoom;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnterRoom()
    {
        //Leaves the room the player is currently in
        currentRoom.LeaveRoom();
        currentRoom = this;
        isInRoom = true;
        foreach(TeleportMarkerBase surface in teleportSurfaces)
        {
            surface.markerActive = true;
        }
    }

    public void LeaveRoom()
    {
        isInRoom = false;
        foreach(TeleportMarkerBase surface in teleportSurfaces)
        {
            surface.markerActive = false;
        }
    }
}
