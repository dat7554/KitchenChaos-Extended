using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }
    
    private static Scene _targetSceneIndex;

    public static void Load(Scene targetScene)
    {
        _targetSceneIndex = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(_targetSceneIndex.ToString());
    }
}
