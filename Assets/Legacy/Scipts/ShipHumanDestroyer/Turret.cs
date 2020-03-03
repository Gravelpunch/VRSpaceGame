using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public Transform target;
    public Vector3 trackingTo;

    public GameObject turretAzimuth;
    public GameObject turretAltitude;
    public GameObject reference;

    public bool isActivated;
    public bool isTrackable;
    public bool isReadyToRetract { get; private set; }
    public bool preparingToRetract;

    public float azimuthSpeed;
    public float elevationSpeed;

    public float retractedAzimuth;
    public float retractedElevation;
	
	void Update () {

        float targetAzimuth = 0;
        float targetElevation = 0;

        float currentAzimuth = turretAzimuth.transform.localRotation.eulerAngles.y;
        if (currentAzimuth > 180) currentAzimuth -= 360;

        float currentElevation = turretAltitude.transform.localRotation.eulerAngles.x;
        if (currentElevation > 180) currentElevation -= 360;

        if (isActivated)
        {
            Vector3 trackingTo = Quaternion.Inverse(transform.rotation) * (target.position - reference.transform.position);
            targetAzimuth = Mathf.Atan2(trackingTo.x, trackingTo.z) * Mathf.Rad2Deg;
            targetElevation = Mathf.Atan2(-trackingTo.y, Mathf.Sqrt(trackingTo.z * trackingTo.z + trackingTo.x * trackingTo.x)) * Mathf.Rad2Deg;
        }
        
        if (!isActivated && isTrackable)
        {
            targetAzimuth = retractedAzimuth;
            targetElevation = retractedElevation;
        }

        if (isTrackable)
        {
            //Tracks the base towards the target azimuth
            if (Mathf.Abs(currentAzimuth - targetAzimuth) < azimuthSpeed / Time.deltaTime)
            {
                turretAzimuth.transform.Rotate(new Vector3(0, targetAzimuth - currentAzimuth, 0));
            }
            else
            {
                turretAzimuth.transform.Rotate(new Vector3(0, azimuthSpeed * Mathf.Sign(targetAzimuth - currentAzimuth) / Time.deltaTime, 0));
            }

            //Tracks the top towards the target elevation
            if (Mathf.Abs(currentElevation - targetElevation) < elevationSpeed / Time.deltaTime)
            {
                turretAltitude.transform.Rotate(new Vector3(targetElevation - currentElevation, 0, 0));
            }
            else
            {
                turretAltitude.transform.Rotate(new Vector3(elevationSpeed * Mathf.Sign(targetElevation - currentElevation) / Time.deltaTime, 0, 0));
            }

            //If the turret has been tracking towards its 'retraction' 
            if (!isActivated && isTrackable)
            {
                if(currentAzimuth == targetAzimuth && currentElevation == targetElevation)
                {
                    isTrackable = false;
                    isReadyToRetract = true;
                }
            }
        }
    }

    public void SetActivated(bool activated)
    {
        isActivated = activated;
    }

    public void Activate()
    {
        SetActivated(true);
        isTrackable = true;
    }

    public void DeActivate()
    {
        SetActivated(false);
    }

}
