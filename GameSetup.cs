using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour
{


    //      OBJECTS:


    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform player;
    [SerializeField] private PlayerCharms playerCharms;
    [SerializeField] private Game game;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PlayerHealthSprites playerHealthSprite;
    
    [SerializeField] private Canvas healthAndSoulVesselsUICanvas;
    [SerializeField] private Canvas loadingScreenCanvas;


    //      GAME SETUP HELPER VARIABLES:


    private static GameSetup instance;

    private bool mainSceneLoaded = false;
    private bool mainSceneSetUp = false;


    //      SETUP:

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        game.SetUpAllCanvases();
        game.DisableAllCanvases();
        game.EnableOnlyThisCanvas(loadingScreenCanvas);

        virtualCamera.Follow = game.loadingScreenCanvas.transform;
        virtualCamera.LookAt = game.loadingScreenCanvas.transform;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;
    }


    //      CODE:


    private void Update()
    {
        // do this after the game is just opened

        if (SceneManager.GetActiveScene().buildIndex == 0 && !mainSceneLoaded)
        {
            mainSceneLoaded = true;
            audioManager.PlayAudioClip(audioManager.musicArray, "MainMenuSoundtrack");
            SceneManager.LoadScene(1);
            return;
        }

        // do this after the starting menu is closed and the main game has started

        if (SceneManager.GetActiveScene().buildIndex == 0 && !mainSceneSetUp)
        {
            game.EnableOnlyThisCanvas(healthAndSoulVesselsUICanvas);
            mainSceneSetUp = true;
            audioManager.StopAudioClip(audioManager.musicArray, "MainMenuSoundtrack");
            Game.mainGameStarted = true;
            playerCharms.ActivateAllCharmsCheck();
            virtualCamera.GetComponent<CameraFollow>().ResetSettings();
        }
    }
}
