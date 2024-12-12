using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class PSignal : ScriptableObject
{
    public List<SignalListiner> listeners = new List<SignalListiner>();
    public void Raise()
    {
        foreach(SignalListiner listiner in listeners)
        {
            listiner.OnSignalRaised();
        }
    }

    public void RegisterListiner(SignalListiner listiner)
    {
        listeners.Add(listiner);
    }
    
    public void DeRegisterListiner(SignalListiner listiner)
    {
        listeners.Remove(listiner);
    }
}

