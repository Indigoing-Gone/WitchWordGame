using System;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Visuals")]
public class MissionVisualData : ScriptableObject
{
    private bool show;
    public bool Show
    {
        get => show;
        set
        {
            if (show == value) return;
            show = value;
            ShowChanged?.Invoke(show);
        }
    }
    public GameEvent_Void eventTrigger;

    public event Action<bool> ShowChanged;

    private void OnEnable()
    {
        eventTrigger.AddListener(SetShow);
    }

    private void OnDisable()
    {
        eventTrigger.RemoveListener(SetShow);
    }

    public void ResetData()
    {
        Show = false;
    }

    private void SetShow()
    {
        Show = true;
    }
}