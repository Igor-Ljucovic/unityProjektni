using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class EventHandlerMainMenu : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private TextMeshProUGUI gameModeHoverDescriptionText;
    
    [SerializeField] private Image charmNotch1Classic;
    [SerializeField] private Image charmNotch2Classic;
    [SerializeField] private Image charmNotch3Classic;
    [SerializeField] private Image charmNotch4Classic;
    [SerializeField] private Image charmNotch5Classic;
    [SerializeField] private Image charmNotch6Classic;
    [SerializeField] private Image charmNotch7Classic;

    [SerializeField] private Image charmNotch1Ultimate;
    [SerializeField] private Image charmNotch2Ultimate;
    [SerializeField] private Image charmNotch3Ultimate;
    [SerializeField] private Image charmNotch4Ultimate;
    [SerializeField] private Image charmNotch5Ultimate;
    [SerializeField] private Image charmNotch6Ultimate;
    [SerializeField] private Image charmNotch7Ultimate;

    [SerializeField] private Sprite charmNotchEmptySprite;
    [SerializeField] private Sprite charmNotchFullSprite;
    
    [SerializeField] private Button stalwartShellCharm;
    [SerializeField] private Button soulCatcherCharm;
    [SerializeField] private Button shamanStoneCharm;
    [SerializeField] private Button soulEaterCharm;
    [SerializeField] private Button sprintMasterCharm;
    [SerializeField] private Button spellTwisterCharm;
    [SerializeField] private Button unbreakableStrengthCharm;
    [SerializeField] private Button quickSlashCharm;
    [SerializeField] private Button steadyBodyCharm;
    [SerializeField] private Button markOfPrideCharm;
    [SerializeField] private Button longnailCharm;
    [SerializeField] private Button furyOfTheFallenCharm;

    [SerializeField] private Button spellboundGraceCharm;
    [SerializeField] private Button sonicSoulCharm;
    [SerializeField] private Button bladeDanceSymphonyCharm;
    [SerializeField] private Button curseOfTheSoullessCharm;
    [SerializeField] private Button abyssalDesperationCharm;
    [SerializeField] private Button overflowingEssenceCharm;
    [SerializeField] private Button unimpairedCataclysmCharm;
    [SerializeField] private Button arcaneEnigmaCharm;
    [SerializeField] private Button airborneSupremacyCharm;
    [SerializeField] private Button diamondPointCharm;
    [SerializeField] private Button daredevilsGambleCharm;
    [SerializeField] private Button excessivelyExtendedCharm;

    private PlayerCharms playerCharms;
    private PlayerMovement playerMovement;
    private AudioManager audioManager;
    private Game game;


    //      GENERAL:


    private List<Button> classicCharmsButtons = new List<Button>();
    private List<bool> classicCharmButtonsEquipped = new List<bool>();

    private List<Button> ultimateCharmsButtons = new List<Button>();
    private List<bool> ultimateCharmButtonsEquipped = new List<bool>();

    private List<Image> classicCharmNotches = new List<Image>();
    private List<Image> ultimateCharmNotches = new List<Image>();

    private Color charmEquippedButtonColor = new Color(0.4156f, 0.5607f, 0.6431f, 1f);
    private Color charmUnequippedButtonColor = new Color(1f, 1f, 1f, 1f);

    private Vector2 testWithADummyEnemyAreaPosition;
    private Vector2 boss1AreaPosition;
    private Vector2 boss2AreaPosition;
    private Vector2 boss3AreaPosition;


    //      SETUP:


    private void Start()
    {
        playerCharms = PlayerCharms.Instance;
        playerMovement = PlayerMovement.Instance;
        audioManager = AudioManager.Instance;
        game = Game.Instance;
        game.DisableAllCanvases();
        testWithADummyEnemyAreaPosition = game.testWithADummyEnemyAreaPosition.transform.position;
        boss1AreaPosition = game.boss1AreaPosition.transform.position;
        boss2AreaPosition = game.boss2AreaPosition.transform.position;
        boss3AreaPosition = game.boss3AreaPosition.transform.position;
        InitializeCharmLists();
    }

    private void InitializeCharmLists()
    {
        classicCharmsButtons.AddElements(stalwartShellCharm, soulCatcherCharm, shamanStoneCharm, soulEaterCharm, sprintMasterCharm,
            spellTwisterCharm, unbreakableStrengthCharm, quickSlashCharm, steadyBodyCharm, markOfPrideCharm, longnailCharm, furyOfTheFallenCharm);

        ultimateCharmsButtons.AddElements(spellboundGraceCharm, sonicSoulCharm, bladeDanceSymphonyCharm, curseOfTheSoullessCharm,
            abyssalDesperationCharm, overflowingEssenceCharm, unimpairedCataclysmCharm, arcaneEnigmaCharm, airborneSupremacyCharm,
            diamondPointCharm, daredevilsGambleCharm, excessivelyExtendedCharm);

        classicCharmNotches.AddElements(charmNotch1Classic, charmNotch2Classic, charmNotch3Classic, charmNotch4Classic,
            charmNotch5Classic, charmNotch6Classic, charmNotch7Classic);

        ultimateCharmNotches.AddElements(charmNotch1Ultimate, charmNotch2Ultimate, charmNotch3Ultimate, charmNotch4Ultimate,
            charmNotch5Ultimate, charmNotch6Ultimate, charmNotch7Ultimate);
    }


    //      METHODS:


    public void MovePlayerToTestWithADummyEnemy() => playerMovement.GetPlayerRigidBody().position = testWithADummyEnemyAreaPosition;

    public void MovePlayerToBoss1() => playerMovement.GetPlayerRigidBody().position = boss1AreaPosition;

    public void MovePlayerToBoss2() => playerMovement.GetPlayerRigidBody().position = boss2AreaPosition;

    public void MovePlayerToBoss3() => playerMovement.GetPlayerRigidBody().position = boss3AreaPosition;

    public void SwitchCharmSpritesCheck()
    {
        if (Game.CurrentGameModeIsClassic())
        {
            classicCharmButtonsEquipped.Clear();

            classicCharmButtonsEquipped.AddElements(playerCharms.stalwartShellCharmEquipped, playerCharms.soulCatcherCharmEquipped,
            playerCharms.shamanStoneCharmEquipped, playerCharms.soulEaterCharmEquipped, playerCharms.sprintMasterCharmEquipped,
            playerCharms.spellTwisterCharmEquipped, playerCharms.unbreakableStrengthCharmEquipped, playerCharms.quickSlashCharmEquipped,
            playerCharms.steadyBodyCharmEquipped, playerCharms.markOfPrideCharmEquipped, playerCharms.longNailCharmEquipped,
            playerCharms.furyOfTheFallenCharmEquipped);

            foreach (var (charmButton, charmEquipped) in classicCharmsButtons.Zip(classicCharmButtonsEquipped, (a, b) => (a, b)))
                SwitchCharmSpriteCheck(charmButton, charmEquipped);
        }
        else
        {
            ultimateCharmButtonsEquipped.Clear();

            ultimateCharmButtonsEquipped.AddElements(playerCharms.spellboundGraceCharmEquipped, playerCharms.sonicSoulCharmEquipped,
            playerCharms.bladeDanceSymphonyCharmEquipped, playerCharms.curseOfTheSoullessCharmEquipped, playerCharms.abyssalDesperationCharmEquipped,
            playerCharms.overflowingEssenceCharmEquipped, playerCharms.unimpairedCataclysmCharmEquipped, playerCharms.arcaneEnigmaCharmEquipped,
            playerCharms.airborneSupremacyCharmEquipped, playerCharms.diamondPointCharmEquipped, playerCharms.daredevilsGambleCharmEquipped,
            playerCharms.excessivelyExtendedCharmEquipped);

            foreach (var (charmButton, charmEquipped) in ultimateCharmsButtons.Zip(ultimateCharmButtonsEquipped, (a, b) => (a, b)))
                SwitchCharmSpriteCheck(charmButton, charmEquipped);
        }
    }

    private void SwitchCharmSpriteCheck(Button charmButton, bool charmEquipped) =>
        charmButton.GetComponent<Image>().color = charmEquipped ? charmEquippedButtonColor : charmUnequippedButtonColor;
    
    public void SwitchCharmNotchesSpritesCheck()
    {
        if (Game.CurrentGameModeIsClassic())
        {
            for (int i = 0; i < classicCharmNotches.Count; i++)
                SwitchCharmNotchesSprite(classicCharmNotches[i], i);
        }
        else
        {
            for (int i = 0; i < ultimateCharmNotches.Count; i++)
                SwitchCharmNotchesSprite(ultimateCharmNotches[i], i);
        }
    }

    private void SwitchCharmNotchesSprite(Image charmNotch, int currentCharmNotchesUsed) =>
    charmNotch.sprite = currentCharmNotchesUsed >= playerCharms.currentPlayerCharmNotchesUsed ? charmNotchEmptySprite : charmNotchFullSprite;

    public void StartMainGame() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    public void SetGameModeToClassic() => Game.SetGameModeToClassic(true);

    public void SetGameModeToUltimate() => Game.SetGameModeToClassic(false);

    public void SetPlayingTestWithADummyEnemyToTrue() => Game.SetPlayingTestWithDummyEnemy(true);

    public void SetPlayingTestWithADummyEnemyToFalse() => Game.SetPlayingTestWithDummyEnemy(false);

    public void QuitGame() => Application.Quit();

    public void RemoveGameModeHoveringText() => gameModeHoverDescriptionText.text = "";

    public void PlayButtonClickSound() => audioManager.PlayAudioClip(audioManager.mainMenuSounds, "ButtonClicked");
}
