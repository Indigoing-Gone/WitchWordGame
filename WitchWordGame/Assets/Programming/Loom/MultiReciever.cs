using System;
using UnityEngine;

public class MultiReciever : MonoBehaviour
{
    [SerializeField] private LoomReciever[] loomRecievers;
    [SerializeField] private int recieverProgression;
    [SerializeField] private GameEvent_Void multiInputRecieved;

    private void OnEnable()
    {
        ResetData();
    }

    private void OnDisable()
    {
        if (recieverProgression >= loomRecievers.Length) return;
        loomRecievers[recieverProgression].InputRecieved -= TrackInputs;
    }

    private void TrackInputs()
    {
        loomRecievers[recieverProgression].InputRecieved -= TrackInputs;
        recieverProgression++;

        if(recieverProgression >= loomRecievers.Length)
        {
            multiInputRecieved.TriggerEvent();
            return;
        }

        loomRecievers[recieverProgression].InputRecieved += TrackInputs;
    }

    public void ResetData()
    {
        loomRecievers[0].InputRecieved += TrackInputs;
        recieverProgression = 0;
    }
}
