using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Loom/MultiReciever")]
public class MultiReciever : MonoBehaviour
{
    [SerializeField] private LoomReciever[] loomRecievers;
    [SerializeField] private int recieverProgression;
    private bool successfulInput;

    public GameEvent_Audio multiInputRecieved;

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
            successfulInput = true;
            multiInputRecieved.TriggerEvent();
            return;
        }

        loomRecievers[recieverProgression].InputRecieved += TrackInputs;
    }

    public void ResetData()
    {
        loomRecievers[0].InputRecieved += TrackInputs;
        successfulInput = false;
        recieverProgression = 0;
    }
}
