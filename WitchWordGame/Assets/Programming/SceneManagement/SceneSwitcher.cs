using Eflatun.SceneReference;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Scenes/SceneSwitcher")]
public class SceneSwitcher : ScriptableObject
{
    [SerializeField] private LoomReciever input;
    [SerializeField] private SceneReference target;
    [SerializeField] private SceneReference[] neighbors;
    public static event Action<SceneReference> SwitchingScene;

    private void OnEnable() => input.InputRecieved += SwitchScene;
    private void OnDisable() => input.InputRecieved -= SwitchScene;
    private void SwitchScene()
    {
        bool inNeighbor = false;
        string currentScenePath = SceneManager.GetActiveScene().path;
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (currentScenePath == neighbors[i].Path)
            {
                inNeighbor = true;
                break;
            }
        }

        if (!inNeighbor) return;

        SwitchingScene?.Invoke(target);
    }
}
