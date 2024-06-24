using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviourService<SceneController>
{
    public int menuSceneIndex = 0;
    public int gameplaySceneIndex = 1;

    [SerializeField] private OpenCloseAnimTrigger _loadingSceneAnimTrigger;
    [SerializeField] private RawImage _loadingImage;
    [SerializeField] private Texture[] _loadingTextures;

    public OpenCloseAnimTrigger LoadingSceneAnimTrigger => _loadingSceneAnimTrigger;

    public async void ChangeScene(int sceneBuildIndex) {
        var scene = SceneManager.LoadSceneAsync(sceneBuildIndex);
        scene.allowSceneActivation = false;

        _loadingSceneAnimTrigger.Open();
        _loadingImage.texture = _loadingTextures[Random.Range(0, _loadingTextures.Length)];

        SceneManager.sceneLoaded += SceneLoaded;

        while (scene.progress < 0.9f) {
            await Task.Yield();
        }

        scene.allowSceneActivation = true;
        //loadingScene.SetActive(false);
    }

    public void ChangeMainMenuScene() {
        ChangeScene(menuSceneIndex);
    }

    public void ChangeGameplayScene() {
        ChangeScene(gameplaySceneIndex);
    }

    public void ChangeCurrentScene() {
        ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= SceneLoaded;
        _loadingSceneAnimTrigger.Close();
    }

}
