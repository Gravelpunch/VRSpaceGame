using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NthUpdateEvent {

    public int updateN; //The update number where this event will be called

    public delegate void UpdateEvent();

    public UpdateEvent updateEvent;

    public void Invoke()
    {
        updateEvent();
    }

    public NthUpdateEvent(int n, UpdateEvent updateEvent)
    {
        this.updateN = n;
        this.updateEvent = updateEvent;
    }
}
