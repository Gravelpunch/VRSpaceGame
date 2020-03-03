using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretsManager : MonoBehaviour {

    public Turret[] turrets;

    private bool waitingOnTurrets = false;

    public Animator turretsAnimator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (waitingOnTurrets)
        {
            bool allTurretsReadyToRetract = true;
            foreach(Turret turret in turrets)
            {
                if (!turret.isReadyToRetract)
                {
                    allTurretsReadyToRetract = false;
                }
            }
            if (allTurretsReadyToRetract)
            {
                //Trigger retraction animation
                turretsAnimator.SetBool("WeaponsDeployed", false);
                waitingOnTurrets = false;
            }
        }
	}

    public void RetractTurrets()
    {
        foreach(Turret turret in turrets)
        {
            turret.DeActivate();
        }
        waitingOnTurrets = true;
    }

    public void DeployTurrets()
    {
        turretsAnimator.SetBool("WeaponsDeployed", true);
    }

    public void ActivateTurrets()
    {
        foreach(Turret turret in turrets)
        {
            turret.Activate();
        }
    }
}
