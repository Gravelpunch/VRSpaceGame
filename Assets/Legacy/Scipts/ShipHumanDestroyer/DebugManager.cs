using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {

    public bool deployTurrets;
    public bool retractTurrets;
    public TurretsManager turretsManager;

    public void Update()
    {
        if (deployTurrets)
        {
            deployTurrets = false;
            turretsManager.DeployTurrets();
        }

        if (retractTurrets)
        {
            retractTurrets = false;
            turretsManager.RetractTurrets();
        }
    }
}
