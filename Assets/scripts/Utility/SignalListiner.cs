using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListiner : MonoBehaviour
{
   public PSignal signal;
   public UnityEvent signalEvent;

   public void OnSignalRaised()
   {
        signalEvent.Invoke();
   }

   private void OnEnable()
   {
        signal.RegisterListiner(this);
   }

   private void OnDisable()
   {
        signal.DeRegisterListiner(this);
   }
}
