using System;
using UnityEngine;

public class MissionVisuals : MonoBehaviour
{
    [SerializeField] private MissionVisualData visualData;
    [SerializeField] private GameObject[] missionVisuals;

    private void OnEnable()
    {
        Debug.Log(visualData.Show);
        if (visualData.Show)
        {
            ShowVisuals(true);
            return;
        }
        visualData.ShowChanged += ShowVisuals;
    }

    private void OnDisable()
    {
        if (!visualData.Show) visualData.ShowChanged -= ShowVisuals;
    }

    private void ShowVisuals(bool _state)
    {
        missionVisuals[0].SetActive(!_state);
        missionVisuals[1].SetActive(_state);
    }
}