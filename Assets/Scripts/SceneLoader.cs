using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    public enum SceneEnum
    {
        PersistentScene,
        MainMenuScene,
        GameScene_Easy,
        GameScene_Normal,
        GameScene_Hard
    }
    
    [SerializeField] private Transform loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    
    public bool IsSkipToShopEnable { get; private set; }

    private void Awake()
    {
        if (Instance is null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!SceneManager.GetSceneByBuildIndex((int)SceneEnum.MainMenuScene).isLoaded)
            {
                SceneManager.LoadSceneAsync((int)SceneEnum.MainMenuScene, LoadSceneMode.Additive);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSkipToShopEnable(bool enable)
    {
        IsSkipToShopEnable = enable;
    }
    
    public void Load(SceneEnum targetSceneEnum)
    {
        if (targetSceneEnum == SceneEnum.MainMenuScene)
        {
            StartCoroutine(LoadMainMenuSceneRoutine());
        }
        else
        {
            StartCoroutine(LoadGameSceneRoutine(targetSceneEnum));
        }
    }
    
    private IEnumerator LoadGameSceneRoutine(SceneEnum targetSceneEnum)
    {
        loadingText.text = "Preparing Kitchen";
        loadingScreen.gameObject.SetActive(true);
        
        int gameWorldIndex = (int) targetSceneEnum;

        // Unload if already loaded
        Scene gameWorldSceneEnum = SceneManager.GetSceneByBuildIndex(gameWorldIndex);
        if (gameWorldSceneEnum.isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(gameWorldIndex);
            yield return unloadOperation;
            
            gameWorldSceneEnum = SceneManager.GetSceneByBuildIndex(gameWorldIndex);
        }
        
        // Load game world
        if (!gameWorldSceneEnum.isLoaded)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(gameWorldIndex, LoadSceneMode.Additive);
            yield return loadOperation;
            
            gameWorldSceneEnum = SceneManager.GetSceneByBuildIndex(gameWorldIndex);
        }
        
        
        // Let all Awake() run
        yield return null;

        // Let all Start() run
        yield return null;
        
        SceneManager.SetActiveScene(gameWorldSceneEnum);

        // Unload main menu
        Scene mainMenuSceneEnum = SceneManager.GetSceneByBuildIndex((int)SceneEnum.MainMenuScene);
        if (mainMenuSceneEnum.isLoaded)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync((int)SceneEnum.MainMenuScene);
            yield return unloadOperation;
        }
        
        yield return null;
        
        loadingScreen.gameObject.SetActive(false);
        
        UpgradeManager.LogActiveUpgrade();
    }
    
    private IEnumerator LoadMainMenuSceneRoutine()
    {
        loadingText.text = "Closing Kitchen";
        loadingScreen.gameObject.SetActive(true);
        
        // save 0 budget for this difficulty - player didn't finish the run
        string difficultyKey = GameModeSelector.GetGameModeSO()?.difficultyKey;
        if (difficultyKey != null)
            SaveManager.SaveBudget(difficultyKey, 0);

        UpgradeManager.ResetUpgrades();

        Scene currentScene = SceneManager.GetActiveScene();
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync((int)SceneEnum.MainMenuScene, LoadSceneMode.Additive);
        yield return loadOperation;
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneEnum.MainMenuScene));
        
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);
        yield return unloadOperation;
        
        yield return null;
        
        loadingScreen.gameObject.SetActive(false);
        if (IsSkipToShopEnable) SetSkipToShopEnable(false);
    }
}
