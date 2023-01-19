using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    public string[] sceneNames;
    public Button[] buttons;
    public GraphicRaycaster raycaster;

    private int _currentScene;
    private int _sceneToLoad;
    private AsyncOperation _loadSceneOperation;
    private AsyncOperation _unloadSceneOperation;

    private void Start()
    {
        _sceneToLoad = 0;
        LoadNewScene();
    }

    public void LoadNewScene_UI(int newSceneIndex)
    {
        raycaster.enabled = false;
        buttons[_currentScene].interactable = true;
        
        _sceneToLoad = newSceneIndex;
        _unloadSceneOperation = SceneManager.UnloadSceneAsync(sceneNames[_currentScene]);
        _unloadSceneOperation.completed += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(AsyncOperation obj)
    {
        _unloadSceneOperation.completed -= OnSceneUnloaded;
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        buttons[_sceneToLoad].interactable = false;
        _loadSceneOperation = SceneManager.LoadSceneAsync(sceneNames[_sceneToLoad], LoadSceneMode.Additive);
        _loadSceneOperation.completed += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(AsyncOperation obj)
    {
        _loadSceneOperation.completed -= OnSceneLoaded;
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        _currentScene = _sceneToLoad;
        raycaster.enabled = true;
    }
}
