using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListiner : MonoBehaviour
{
   public Signal signal;
   public UnityEvent signalEvent;

   public void OnSignalRaised()
   {
        signalEvent.Invoke();
   }

   private void OnEnamble()
   {
        signal.RegisterListiner(this);
   }

   private void OnDisable()
   {
        signal.DeRegisterListiner(this);
   }
}
