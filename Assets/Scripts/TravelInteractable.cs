using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

[RequireComponent(typeof(Interactable))]
public class TravelInteractable : MonoBehaviour {

    private Interactable interactable;

    public delegate void TravelHandler(TravelInteractableSide side);

    public enum TravelInteractableSide
    {
        side1,
        side2
    }

    public event TravelHandler OnTravel;


    [Header("Room 1")]
    public ShipRoom room1;
    public Transform room1Entrance;
    public TextMesh textMeshRoom1;

    [Header("Room 2")]
    public ShipRoom room2;
    public Transform room2Entrance;
    public TextMesh textMeshRoom2;

    private float currentFadeTime;

    private void Awake()
    {
        interactable = gameObject.GetComponent<Interactable>();
    }

    // Use this for initialization
    void Start () {
        textMeshRoom1.gameObject.SetActive(false);
        textMeshRoom2.gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        
    }

    

    private Vector3 VLerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t));
    }

    private void OnHandHoverBegin(Hand hand)
    {

        (room1.isInRoom ? textMeshRoom1 : textMeshRoom2).gameObject.SetActive(true);
    }

    private void OnHandHoverEnd(Hand hand)
    {

        (room1.isInRoom ? textMeshRoom1 : textMeshRoom2).gameObject.SetActive(false);
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            TeleportToNextRoom();
        }
    }

    private void TeleportToNextRoom()
    {
        //Disables teleportation while teleporting
        Teleport.instance.gameObject.SetActive(false);

        //Code partially ripped off from Teleport.cs. Used to simulate a teleport from the door script
        //teleporting = true;

        currentFadeTime = Teleport.instance.teleportFadeTime;

        /*if (teleportPoint != null && teleportPoint.teleportType == TeleportPoint.TeleportPointType.SwitchToNewScene)
        {
            currentFadeTime *= 3.0f;
            Teleport.ChangeScene.Send(currentFadeTime);
        }*/

        SteamVR_Fade.Start(Color.clear, 0);
        SteamVR_Fade.Start(Color.black, currentFadeTime);

        Teleport.instance.headAudioSource.transform.SetParent(Player.instance.hmdTransform);
        Teleport.instance.headAudioSource.transform.localPosition = Vector3.zero;
        Teleport.instance.headAudioSource.clip = Teleport.instance.teleportSound;
        Teleport.instance.headAudioSource.Play();

        Invoke("TeleportToNextRoomStep2", currentFadeTime);
    }

    private void TeleportToNextRoomStep2()
    {
        //teleporting = false;

        //Teleport.PlayerPre.Send(pointedAtTeleportMarker);

        SteamVR_Fade.Start(Color.clear, currentFadeTime);

        /*TeleportPoint teleportPoint = teleportingToMarker as TeleportPoint;
        Vector3 teleportPosition = pointedAtPosition;

        if (teleportPoint != null)
        {
            teleportPosition = teleportPoint.transform.position;

            //Teleport to a new scene
            if (teleportPoint.teleportType == TeleportPoint.TeleportPointType.SwitchToNewScene)
            {
                teleportPoint.TeleportToScene();
                return;
            }
        }

        // Find the actual floor position below the navigation mesh
        TeleportArea teleportArea = teleportingToMarker as TeleportArea;
        if (teleportArea != null)
        {
            if (floorFixupMaximumTraceDistance > 0.0f)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(teleportPosition + 0.05f * Vector3.down, Vector3.down, out raycastHit, floorFixupMaximumTraceDistance, floorFixupTraceLayerMask))
                {
                    teleportPosition = raycastHit.point;
                }
            }
        }

        if (teleportingToMarker.ShouldMovePlayer())
        {
            Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
            player.trackingOriginTransform.position = teleportPosition + playerFeetOffset;
        }
        else
        {
            teleportingToMarker.TeleportPlayer(pointedAtPosition);
        }

        Teleport.Player.Send(pointedAtTeleportMarker);*/
        if (!room1.isInRoom && !room2.isInRoom) throw new System.Exception("A player tried to teleport through a door they were on neither side of.");
        if (room1.isInRoom && room2.isInRoom) throw new System.Exception("A player tried to teleport through a door they were somehow in both sides of.");

        bool wasInRoom1 = room1.isInRoom;
        bool wasInRoom2 = room2.isInRoom;


        if (wasInRoom1)
        {
            room2.EnterRoom();
            Player.instance.transform.position = room2Entrance.transform.position;
        }

        if (wasInRoom2)
        {
            room1.EnterRoom();
            Player.instance.transform.position = room1Entrance.transform.position;
        }

        if (OnTravel != null)
            OnTravel(wasInRoom1 ? TravelInteractableSide.side1 : TravelInteractableSide.side2);

        Teleport.instance.gameObject.SetActive(true);
        textMeshRoom1.gameObject.SetActive(false);
        textMeshRoom2.gameObject.SetActive(false);
    }
}
