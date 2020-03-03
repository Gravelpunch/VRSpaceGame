using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class DoorMovement : MonoBehaviour {

    public Vector3 rightDoorOpenOffset;
    public Vector3 leftDoorOpenOffset;
    public float openTime;

    public GameObject rightDoor;
    public GameObject leftDoor;

    private bool isOpening;
    private bool isClosing;
    private float openCounter = 0;
    private Vector3 rightDoorClosedOffset;
    private Vector3 leftDoorClosedOffset;

    private void Awake()
    {
        rightDoorClosedOffset = rightDoor.transform.localPosition;
        leftDoorClosedOffset = leftDoor.transform.localPosition;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isOpening)
        {
            openCounter += Time.deltaTime;
            if (openCounter >= openTime)
            {
                openCounter = openTime;
                isOpening = false;
            }
            rightDoor.transform.localPosition = VSlerp(rightDoorClosedOffset, rightDoorOpenOffset, openCounter / openTime);
            leftDoor.transform.localPosition = VSlerp(leftDoorClosedOffset, leftDoorOpenOffset, openCounter / openTime);
        }

        if (isClosing)
        {
            openCounter -= Time.deltaTime;
            if (openCounter <= 0)
            {
                openCounter = 0;
                isClosing = false;
            }
            rightDoor.transform.localPosition = VSlerp(rightDoorClosedOffset, rightDoorOpenOffset, openCounter / openTime);
            leftDoor.transform.localPosition = VSlerp(leftDoorClosedOffset, leftDoorOpenOffset, openCounter / openTime);
        }
    }

    private float Slerp(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, (1 + Mathf.Cos(Mathf.Lerp(Mathf.PI, 2 * Mathf.PI, t))) / 2);
    }

    private Vector3 VSlerp(Vector3 a, Vector3 b, float t)
    {
        return new Vector3(Slerp(a.x, b.x, t), Slerp(a.y, b.y, t), Slerp(a.z, b.z, t));
    }

    private void OnHandHoverBegin(Hand hand)
    {

        isOpening = true;
        isClosing = false;
    }

    private void OnHandHoverEnd(Hand hand)
    {
        isOpening = false;
        isClosing = true;
    }
}
