using Eflatun.SceneReference;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneReference[] gameplaySystemScenes;
    [SerializeField] private SceneReference startScene;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static private async void Init()
    {
        if (SceneManager.GetActiveScene().name == "System") return;

#if UNITY_EDITOR
        await SceneManager.LoadSceneAsync("System", LoadSceneMode.Additive);
#else
        await SceneManager.LoadSceneAsync("System", LoadSceneMode.Single);
#endif
    }

    private void OnEnable()
    {
        SceneSwitcher.SwitchingScene += LoadScene;
    }

    private void OnDisable()
    {
        SceneSwitcher.SwitchingScene -= LoadScene;
    }

    private async void Awake()
    {
        if (startScene.Name == "System") Debug.LogError("StartScene is System, causing a recursive load loop");

        for (int i = 0; i < gameplaySystemScenes.Length; i++) await LoadSceneAdditive(gameplaySystemScenes[i]);

#if UNITY_EDITOR
        Scene _currentScene = SceneManager.GetActiveScene();
        for (int i = 0; i < gameplaySystemScenes.Length; i++)
        {
            if (_currentScene.path == gameplaySystemScenes[i].Path || _currentScene.name == "System")
            {
                await LoadActiveScene(startScene);
                return;
            }
        }
#else
        await LoadActiveScene(startScene);
#endif
    }

    public async void LoadScene(SceneReference _scene) => await LoadActiveScene(_scene);

    public async Task LoadActiveScene(SceneReference _scene)
    {
        await LoadSceneAdditive(_scene);

        //Set Active
        Scene _oldScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_scene.Path));

        await UnloadScene(_oldScene);
        await Resources.UnloadUnusedAssets();
    }

    private async Task LoadSceneAdditive(SceneReference _scene)
    {
        //Load New Scene
        AsyncOperation _loadingOperation = SceneManager.LoadSceneAsync(_scene.Path, LoadSceneMode.Additive);
        _loadingOperation.allowSceneActivation = false;
        while (_loadingOperation.progress < 0.9f) await Task.Yield();
        _loadingOperation.allowSceneActivation = true;
        while (!_loadingOperation.isDone) await Task.Yield();
    }

    private async Task UnloadScene(Scene _scene)
    {
        if (_scene == null || _scene.name == "System") return;
        AsyncOperation _unloadingScene = SceneManager.UnloadSceneAsync(_scene.path);
        while(!_unloadingScene.isDone) await Task.Yield();
    }
}
