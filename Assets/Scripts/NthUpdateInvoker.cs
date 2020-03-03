using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NthUpdateInvoker : MonoBehaviour {

    public List<NthUpdateEvent> events;

    private int updateN = 0;

    private void Awake()
    {
        events = new List<NthUpdateEvent>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        updateN++;
        foreach(NthUpdateEvent nthUpdateEvent in events)
        {
            if(nthUpdateEvent.updateN == updateN)
            {
                nthUpdateEvent.Invoke();
            }
        }
	}
}
