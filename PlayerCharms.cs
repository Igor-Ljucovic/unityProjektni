using UnityEngine;

public class PlayerCharms : MonoBehaviour
{


    //      OBJECTS:


    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerSoulVessel playerSoulVessel;
    [SerializeField] private PlayerSpells playerSpells;
    [SerializeField] private AudioManager audioManager;


    //      GENERAL:


    public    int currentPlayerCharmNotchesUsed = 0;
    public    int maxPlayerCharmNotches = 7;


    //      CHARMS - CLASSIC:


    public   bool stalwartShellCharmEquipped = false;   // eventualno dictionary za kostanje charm notch-eva za svaki od ovih charm-ova
    public    int stalwartShellCharmNotchCost = 2;
    private float stalwartShellInvulnerabilityDurationMultiplicator = 1.4f;   // mogu i onaj specijalan keyword da budu svi ovi, da ne budu vidljivi u inspector-u!

    public   bool soulCatcherCharmEquipped = false;
    public    int soulCatcherCharmNotchCost = 2;
    private   int soulCatcherBonusSoulGainPerHit = 4;

    public   bool shamanStoneCharmEquipped = false;
    public    int shamanStoneCharmNotchCost = 3;
    private float shamanStoneVengefulSpiritDamageMultiplicator = 1.33f;
    private float shamanStoneAbyssShriekDamageMultiplicator = 1.5f;
    private float shamanStoneDescendingDarkDamageMultiplicator = 1.5f;

    public   bool soulEaterCharmEquipped = false;
    public    int soulEaterCharmNotchCost = 4;
    private   int soulEaterBonusSoulGainPerHit = 8;

    public   bool sprintMasterCharmEquipped = false;
    public    int sprintMasterCharmNotchCost = 1;
    private float sprintMasterSpeedMultiplicator = 1.3f;

    public   bool spellTwisterCharmEquipped = false;
    public    int spellTwisterCharmNotchCost = 2;
    private   int spellTwisterSpellSoulCost = 24;

    public   bool unbreakableStrengthCharmEquipped = false;
    public    int unbreakableStrengthCharmNotchCost = 3;
    private float unbreakableStrengthNailDamageMultiplayer = 1.3f;

    public   bool quickSlashCharmEquipped = false;
    public    int quickSlashCharmNotchCost = 3;
    private float quickSlashNailSlashCooldownMultiplayer = 0.7f;

    public   bool steadyBodyCharmEquipped = false;
    public    int steadyBodyCharmNotchCost = 1;   // The player no longer gets knockedback when slashing an enemy/wall

    public   bool markOfPrideCharmEquipped = false;
    public    int markOfPrideCharmNotchCost = 3;
    private float markOfPrideNailRangeMultiplayer = 1.25f;   // ova 2 za range izbaci da zaokruzis na 10 ako ne uspes drugacije

    public   bool longNailCharmEquipped = false;
    public    int longNailCharmNotchCost = 2;
    private float longNailNailRangeMultiplayer = 1.15f;   // ovde treba jos varijabli za radius-e nove, ili njih horizontalno samo pomnozis, ali moras i sprite!

    public   bool furyOfTheFallenCharmEquipped = false;
    public     int furyOfTheFallenCharmNotchCost = 2;
    private    int furyOfTheFallenHealthRequirementForNailDamageMultiplayer = 1;
    private  float furyOfTheFallenNailDamageMultiplayer = 1.75f;
    private   bool furyOfTheFallenCurrentlyActive = false;


    //      CHARMS - ULTIMATE:


    public   bool spellboundGraceCharmEquipped = false;
    public    int spellboundGraceCharmNotchCost = 3;
    private float spellboundGraceAbyssShriekDelay = 0;
    private float spellboundGraceVengefulSpiritDelay = 0;
    private float spellboundGraceDescendingDarkDescendDelay = 0;

    public   bool bladeDanceSymphonyCharmEquipped = false;
    public    int bladeDanceSymphonyCharmNotchCost = 1;
    private   int bladeDanceSymphonyTotalEnemyNailSlashesRequiredForCriticalHit = 5;   // the critical hit also contributes to the slash hit counter
    private   int bladeDanceSymphonyCurrentEnemyNailSlashesRequiredForCriticalHit = 0;
    private float bladeDanceSymphonyNailDamageMultiplayer = 3f;
    private   int bladeDanceSymphonyMaximumFramesWithoutSlashingEnemyForCriticalHit = 45;
    public    int bladeDanceSymphonyCurrentFrameWithoutSlashingEnemyForCriticalHit = 0;
    private  bool bladeDanceSymphonyCurrentlyActive = false;

    public   bool curseOfTheSoullessCharmEquipped = false;
    public    int curseOfTheSoullessCharmNotchCost = 2;
    private   int curseOfTheSoullessSoulGainPerHit = 0;
    private float curseOfTheSoullessUpslashDamageMultiplayer = 1.5f;
    private  bool curseOfTheSoullessCurrentlyActive = false;

    public bool abyssalDesperationCharmEquipped = false;
    public int abyssalDesperationCharmNotchCost = 1;
    public int abyssalDesperationMaximumHealthForAbyssShriekBonusDamage = 4;
    private float abyssalDesperationAbyssShriekDamageMultiplayer = 1.25f;
    private  bool abyssalDesperationCurrentlyActive = false;

    public bool sonicSoulCharmEquipped = false;
    public int sonicSoulCharmNotchCost = 3;
    private float sonicSoulNailSlashCooldownIfSoulVesselsAreFullMultiplayer = 0.6f;
    private  bool sonicSoulCurrentlyActive = false;

    public bool overflowingEssenceCharmEquipped = false;
    public int overflowingEssenceCharmNotchCost = 4;
    private float overflowingEssenceSoulGainMultiplayer = 2f;
    private  bool overflowingEssenceCurrentlyActive = false;

    public bool arcaneEnigmaCharmEquipped = false;
    public int arcaneEnigmaCharmNotchCost = 3;
    private int arcaneEnigmaNailDamage = 0;
    private float arcaneEnigmaSpellDamageMultiplayer = 1.75f;

    public bool diamondPointCharmEquipped = false;
    public int diamondPointNotchCost = 1;
    private float diamondPointNailSlashCooldownMultiplayer = 4f;
    private float diamondPointNailDamageMultiplayer = 4f;
    private float diamondPointSoulGainMultiplayer = 1.5f;

    public bool daredevilsGambleCharmEquipped = false;
    public int daredevilsGambleCharmNotchCost = 1;   // napravi novu varijablu za dmg taken
    private int daredevilsGambleDamageTakenMultiplayer = 2;
    private float daredevilsGambleDescendingDarkPlayerInvunlerabilityDurationAfterLandingBonus = 1;

    public bool unimpairedCataclysmCharmEquipped = false;
    public int unimpairedCataclysmCharmNotchCost = 2;
    private float unimpairedCataclysmDescendingDarkDamageMultiplayer = 1.5f;
    private bool unimpairedCataclysmCurrentlyActive = false;

    public bool airborneSupremacyCharmEquipped = false;
    public int airborneSupremacyCharmNotchCost = 3;
    private float airborneSupremacyNailDamageMultiplayer = 1.4f;
    private bool airborneSupremacyCurrentlyActive = false;

    public bool excessivelyExtendedCharmEquipped = false;
    public int excessivelyExtendedCharmNotchCost = 2; 
    private float excessivelyExtendedNailRangeMultiplayer = 1.6f;   // it also disables roll and dash


    //      SETUP:


    private static PlayerCharms instance;

    public static PlayerCharms Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerCharms>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("PlayerCharms");
                    instance = managerObject.AddComponent<PlayerCharms>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return instance;
        }
    }


    //      METHODS:


    //      CHARMS - CLASSIC:


    private void ActivateSpellboundGraceCharm()
    {
        playerSpells.abyssShriekDelayBeforeSpawning = spellboundGraceAbyssShriekDelay;
        playerSpells.descendingDarkDescendDelay = spellboundGraceDescendingDarkDescendDelay;
        playerSpells.vengefulSpiritDelayBeforeSpawning = spellboundGraceVengefulSpiritDelay;
    }

    public void TriggerSonicSoulCharmCheck()
    {
        if (!sonicSoulCharmEquipped)
            return;
        
        if (playerSoulVessel.AllSoulVesselsFull() && !sonicSoulCurrentlyActive)
        {
            playerNail.totalSlashCooldown = (int)(playerNail.totalSlashCooldown * sonicSoulNailSlashCooldownIfSoulVesselsAreFullMultiplayer + 0.5f);
            sonicSoulCurrentlyActive = true;
        }
        else if (!playerSoulVessel.AllSoulVesselsFull() && sonicSoulCurrentlyActive)
        {
            playerNail.totalSlashCooldown = (int)(playerNail.totalSlashCooldown / sonicSoulNailSlashCooldownIfSoulVesselsAreFullMultiplayer + 0.5f) + 1;
            sonicSoulCurrentlyActive = false;
        }
    }

    public void TriggerBladeDanceSymphonyCharmCheck()
    {
        if (!bladeDanceSymphonyCharmEquipped)
            return;
        
        if (bladeDanceSymphonyCurrentFrameWithoutSlashingEnemyForCriticalHit > bladeDanceSymphonyMaximumFramesWithoutSlashingEnemyForCriticalHit)
            bladeDanceSymphonyCurrentEnemyNailSlashesRequiredForCriticalHit = 0;
            
        bladeDanceSymphonyCurrentEnemyNailSlashesRequiredForCriticalHit++;

        if (bladeDanceSymphonyCurrentFrameWithoutSlashingEnemyForCriticalHit <= bladeDanceSymphonyMaximumFramesWithoutSlashingEnemyForCriticalHit
            && !bladeDanceSymphonyCurrentlyActive)
        {
            if (bladeDanceSymphonyCurrentEnemyNailSlashesRequiredForCriticalHit == bladeDanceSymphonyTotalEnemyNailSlashesRequiredForCriticalHit)
            {
                playerNail.currentNailDamage = (int)(playerNail.currentNailDamage * bladeDanceSymphonyNailDamageMultiplayer + 0.5f);
                bladeDanceSymphonyCurrentEnemyNailSlashesRequiredForCriticalHit = 1;
                bladeDanceSymphonyCurrentlyActive = true;
            }
            else
                bladeDanceSymphonyCurrentlyActive = false;
        }
        else if (bladeDanceSymphonyCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage / bladeDanceSymphonyNailDamageMultiplayer + 0.5f);
            bladeDanceSymphonyCurrentlyActive = false;
        }

        bladeDanceSymphonyCurrentFrameWithoutSlashingEnemyForCriticalHit = 0;
    }

    private void ActivateCurseOfTheSoullessCharm() => playerSoulVessel.playerCurrentSoulGain = curseOfTheSoullessSoulGainPerHit;
    
    public void TriggerCurseOfTheSoullessCharmCheck()
    {
        if (!curseOfTheSoullessCharmEquipped)
            return;
        
        if (Input.GetKey(KeyCode.UpArrow) && !curseOfTheSoullessCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage * curseOfTheSoullessUpslashDamageMultiplayer + 0.5f);
            curseOfTheSoullessCurrentlyActive = true;
        }
        else if (!Input.GetKey(KeyCode.UpArrow) && curseOfTheSoullessCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage / curseOfTheSoullessUpslashDamageMultiplayer + 0.5f);
            curseOfTheSoullessCurrentlyActive = false;
        }
    }

    public void TriggerAbyssalDesperationCharmCheck()
    {
        if (!abyssalDesperationCharmEquipped)
            return;
        
        if (playerHealth.currentPlayerHealth <= playerHealth.maxPlayerHealth / 2 && !abyssalDesperationCurrentlyActive)
        {
            playerSpells.abyssShriekDamagePerHit = (int)(playerSpells.abyssShriekDamagePerHit * abyssalDesperationAbyssShriekDamageMultiplayer);
            abyssalDesperationCurrentlyActive = true;
        }
        else if (playerHealth.currentPlayerHealth > playerHealth.maxPlayerHealth / 2 && abyssalDesperationCurrentlyActive)
        {
            playerSpells.abyssShriekDamagePerHit = (int)(playerSpells.abyssShriekDamagePerHit / abyssalDesperationAbyssShriekDamageMultiplayer);
            abyssalDesperationCurrentlyActive = false;
        }
}

    public void TriggerOverflowingEssenceCharmCheck()
    {
        if (!overflowingEssenceCharmEquipped)
            return;
        
        if (playerSoulVessel.currentBigSoulVesselSoul < playerSoulVessel.maximumBigSoulVesselSoul && !overflowingEssenceCurrentlyActive)
        {
            playerSoulVessel.playerCurrentSoulGain = (int)(playerSoulVessel.playerCurrentSoulGain * overflowingEssenceSoulGainMultiplayer + 0.5f);

            if (diamondPointCharmEquipped)
                playerSoulVessel.playerCurrentSoulGain--;

            overflowingEssenceCurrentlyActive = true;
        }
        else if (playerSoulVessel.currentBigSoulVesselSoul >= playerSoulVessel.maximumBigSoulVesselSoul && overflowingEssenceCurrentlyActive)
        {
            playerSoulVessel.playerCurrentSoulGain = (int)(playerSoulVessel.playerCurrentSoulGain / overflowingEssenceSoulGainMultiplayer + 0.5f);
            overflowingEssenceCurrentlyActive = false;
        }
    }

    private void ActivateArcaneEnigmaCharm()
    {
        playerNail.currentNailDamage = arcaneEnigmaNailDamage;
        playerSpells.vengefulSpiritDamagePerHit = (int)(playerSpells.vengefulSpiritDamagePerHit * arcaneEnigmaSpellDamageMultiplayer + 0.5f);
        playerSpells.abyssShriekDamagePerHit = (int)(playerSpells.abyssShriekDamagePerHit * arcaneEnigmaSpellDamageMultiplayer + 0.5f);
        playerSpells.descendingDarkDescendDamage = (int)(playerSpells.descendingDarkDescendDamage * arcaneEnigmaSpellDamageMultiplayer + 0.5f);
        playerSpells.descendingDarkExplosionDamage = (int)(playerSpells.descendingDarkExplosionDamage * arcaneEnigmaSpellDamageMultiplayer + 0.5f);
    }

    public void TriggerAirborneSupremacyCharmCheck()
    {
        if (!airborneSupremacyCharmEquipped)
            return;
        
        if (!playerMovement.IsGrounded() && !airborneSupremacyCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage * airborneSupremacyNailDamageMultiplayer + 0.5f);
            airborneSupremacyCurrentlyActive = true;
        }
        else if (playerMovement.IsGrounded() && airborneSupremacyCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage / airborneSupremacyNailDamageMultiplayer + 0.5f);
            airborneSupremacyCurrentlyActive = false;
        }
    }

    private void ActivateDiamondPointCharm()
    {
        playerNail.currentNailDamage *= (int) diamondPointNailDamageMultiplayer;
        playerNail.totalSlashCooldown *= (int) diamondPointNailSlashCooldownMultiplayer;
        playerSoulVessel.playerCurrentSoulGain = (int) (playerSoulVessel.playerCurrentSoulGain * diamondPointSoulGainMultiplayer + 0.5f);
    }

    private void ActivateDaredevilsGambleCharm()
    {
        playerHealth.damageTakenForTouchingEnemy *= daredevilsGambleDamageTakenMultiplayer;
        playerSpells.descendingDarkPlayerInvunlerabilityDurationAfterLanding += daredevilsGambleDescendingDarkPlayerInvunlerabilityDurationAfterLandingBonus;
    }

    public void TriggerUnimpairedCataclysmCharm()
    {
        if (!unimpairedCataclysmCharmEquipped)
            return;
        
        if (playerHealth.currentPlayerHealth == playerHealth.maxPlayerHealth && !unimpairedCataclysmCurrentlyActive)
        {
            playerSpells.descendingDarkExplosionDamage = (int)(playerSpells.descendingDarkExplosionDamage * unimpairedCataclysmDescendingDarkDamageMultiplayer);
            playerSpells.descendingDarkDescendDamage = (int)(playerSpells.descendingDarkDescendDamage * unimpairedCataclysmDescendingDarkDamageMultiplayer);
            unimpairedCataclysmCurrentlyActive = true;
        }
        else if (playerHealth.currentPlayerHealth != playerHealth.maxPlayerHealth && unimpairedCataclysmCurrentlyActive)
        {
            playerSpells.descendingDarkExplosionDamage = (int)(playerSpells.descendingDarkExplosionDamage / unimpairedCataclysmDescendingDarkDamageMultiplayer);
            playerSpells.descendingDarkDescendDamage = (int)(playerSpells.descendingDarkDescendDamage / unimpairedCataclysmDescendingDarkDamageMultiplayer);
            unimpairedCataclysmCurrentlyActive = false;
        }
    }

    private void ActivateExcessivelyExtendedCharm()
    {
        playerNail.normalSlashHorizontalOffset *= excessivelyExtendedNailRangeMultiplayer;
        playerNail.normalSlashVerticalOffset *= excessivelyExtendedNailRangeMultiplayer;
        playerNail.slashHorizontalHitboxWhenSlashingHorizontally *= excessivelyExtendedNailRangeMultiplayer;
        playerNail.slashVerticalHitboxWhenSlashingVertically *= excessivelyExtendedNailRangeMultiplayer;

        Transform nailHitbox = playerNail.slash.transform;   // these 2 will be completely removed later because the knight will have a sprite instead of the nail sprite
        nailHitbox.localScale = new Vector2(nailHitbox.localScale.x * excessivelyExtendedNailRangeMultiplayer, nailHitbox.localScale.y);
    }


    //      CHARMS - CLASSIC:


    private void ActivateStalwartShellCharm() =>
        playerHealth.totalPlayerInvulnerabilityAfterTakingDamage = 
        (int)(stalwartShellInvulnerabilityDurationMultiplicator * playerHealth.totalPlayerInvulnerabilityAfterTakingDamage + 0.5f);
    
    private void ActivateSoulCatcherCharm() => playerSoulVessel.playerCurrentSoulGain += soulCatcherBonusSoulGainPerHit;
    
    private void ActivateShamanStoneCharm()
    {
        playerSpells.vengefulSpiritDamagePerHit = (int)(playerSpells.vengefulSpiritDamagePerHit * shamanStoneVengefulSpiritDamageMultiplicator + 0.5f);
        playerSpells.abyssShriekDamagePerHit = (int)(playerSpells.abyssShriekDamagePerHit * shamanStoneAbyssShriekDamageMultiplicator + 0.5f);
        playerSpells.descendingDarkDescendDamage = (int)(playerSpells.descendingDarkDescendDamage * shamanStoneDescendingDarkDamageMultiplicator + 0.5f);
        playerSpells.descendingDarkExplosionDamage = (int)(playerSpells.descendingDarkExplosionDamage * shamanStoneDescendingDarkDamageMultiplicator + 0.5f);
    }

    private void ActivateSoulEaterCharm() => playerSoulVessel.playerCurrentSoulGain += soulEaterBonusSoulGainPerHit;
    
    private void ActivateSprintMasterCharm() => playerMovement.speed *= sprintMasterSpeedMultiplicator;
    
    private void ActivateSpellTwisterCharm() => playerSpells.spellSoulCost = spellTwisterSpellSoulCost;
    
    private void ActivateUnbreakableStrengthCharm() => playerNail.currentNailDamage = (int)(playerNail.currentNailDamage * unbreakableStrengthNailDamageMultiplayer + 0.5f);
    
    private void ActivateQuickSlashCharm() => playerNail.totalSlashCooldown = (int)(playerNail.totalSlashCooldown * quickSlashNailSlashCooldownMultiplayer + 0.5f);
    
    private void ActivateMarkOfPrideCharm()
    {
        playerNail.normalSlashHorizontalOffset *= markOfPrideNailRangeMultiplayer;
        playerNail.normalSlashVerticalOffset *= markOfPrideNailRangeMultiplayer;
        playerNail.slashHorizontalHitboxWhenSlashingHorizontally *= markOfPrideNailRangeMultiplayer;
        playerNail.slashVerticalHitboxWhenSlashingVertically *= markOfPrideNailRangeMultiplayer;

        Transform nailHitbox = playerNail.slash.transform;  // these 2 will be completely removed later because the knight will have a sprite instead of the nail sprite
        nailHitbox.localScale = new Vector2(nailHitbox.localScale.x * markOfPrideNailRangeMultiplayer, nailHitbox.localScale.y);
    }

    private void ActivateLongNailCharm()
    {
        playerNail.normalSlashHorizontalOffset *= longNailNailRangeMultiplayer;
        playerNail.normalSlashVerticalOffset *= longNailNailRangeMultiplayer;
        playerNail.slashHorizontalHitboxWhenSlashingHorizontally *= longNailNailRangeMultiplayer;
        playerNail.slashVerticalHitboxWhenSlashingVertically *= longNailNailRangeMultiplayer;

        Transform nailHitbox = playerNail.slash.transform;  // these 2 will be completely removed later because the knight will have a sprite instead of the nail sprite
        nailHitbox.localScale = new Vector2(nailHitbox.localScale.x * longNailNailRangeMultiplayer, nailHitbox.localScale.y);
    }

    public void TriggerFuryOfTheFallenCharmCheck() //it has to check every time player health changes (focus and damaged by enemy)
    {
        if (!furyOfTheFallenCharmEquipped)
            return;
        
        if (playerHealth.currentPlayerHealth <= furyOfTheFallenHealthRequirementForNailDamageMultiplayer && !furyOfTheFallenCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage * furyOfTheFallenNailDamageMultiplayer + 0.5f);
            audioManager.PlayAudioClip(audioManager.playerCharmsSounds, "FuryOfTheFallenActivated");
            audioManager.PlayAudioClip(audioManager.playerCharmsSounds, "FuryOfTheFallenActive");
            furyOfTheFallenCurrentlyActive = true;
        }
        else if (playerHealth.currentPlayerHealth > furyOfTheFallenHealthRequirementForNailDamageMultiplayer && furyOfTheFallenCurrentlyActive)
        {
            playerNail.currentNailDamage = (int)(playerNail.currentNailDamage / furyOfTheFallenNailDamageMultiplayer + 0.5f);
            furyOfTheFallenCurrentlyActive = false;
            audioManager.StopAudioClip(audioManager.playerCharmsSounds, "FuryOfTheFallenActive");
        }
    }

    private void EquipCharm(ref bool charmEquipped, int charmNotchCost)
    {
        charmEquipped = true;
        currentPlayerCharmNotchesUsed += charmNotchCost;
    }

    private void UnequipCharm(ref bool charmEquipped, int charmNotchCost)
    {
        charmEquipped = false;
        currentPlayerCharmNotchesUsed -= charmNotchCost;
    }

    private void UnequipCharmIfEquipped(ref bool charmEquipped, int charmNotchCost)
    {
        if (charmEquipped)
            UnequipCharm(ref charmEquipped, charmNotchCost);
    }

    public void UnequipAllClassicModeCharmsCheck()
    {
        UnequipCharmIfEquipped(ref stalwartShellCharmEquipped, stalwartShellCharmNotchCost);
        UnequipCharmIfEquipped(ref soulCatcherCharmEquipped, soulCatcherCharmNotchCost);
        UnequipCharmIfEquipped(ref shamanStoneCharmEquipped, shamanStoneCharmNotchCost);
        UnequipCharmIfEquipped(ref soulEaterCharmEquipped, soulEaterCharmNotchCost);
        UnequipCharmIfEquipped(ref sprintMasterCharmEquipped, sprintMasterCharmNotchCost);
        UnequipCharmIfEquipped(ref spellTwisterCharmEquipped, spellTwisterCharmNotchCost);
        UnequipCharmIfEquipped(ref unbreakableStrengthCharmEquipped, unbreakableStrengthCharmNotchCost);
        UnequipCharmIfEquipped(ref quickSlashCharmEquipped, quickSlashCharmNotchCost);
        UnequipCharmIfEquipped(ref steadyBodyCharmEquipped, steadyBodyCharmNotchCost);
        UnequipCharmIfEquipped(ref markOfPrideCharmEquipped, markOfPrideCharmNotchCost);
        UnequipCharmIfEquipped(ref longNailCharmEquipped, longNailCharmNotchCost);
        UnequipCharmIfEquipped(ref furyOfTheFallenCharmEquipped, furyOfTheFallenCharmNotchCost);
    }

    public void UnequipAllUltimateModeCharmsCheck()
    {
        UnequipCharmIfEquipped(ref spellboundGraceCharmEquipped, spellboundGraceCharmNotchCost);
        UnequipCharmIfEquipped(ref bladeDanceSymphonyCharmEquipped, bladeDanceSymphonyCharmNotchCost);
        UnequipCharmIfEquipped(ref curseOfTheSoullessCharmEquipped, curseOfTheSoullessCharmNotchCost);
        UnequipCharmIfEquipped(ref abyssalDesperationCharmEquipped, abyssalDesperationCharmNotchCost);
        UnequipCharmIfEquipped(ref sonicSoulCharmEquipped, sonicSoulCharmNotchCost);
        UnequipCharmIfEquipped(ref overflowingEssenceCharmEquipped, overflowingEssenceCharmNotchCost);
        UnequipCharmIfEquipped(ref arcaneEnigmaCharmEquipped, arcaneEnigmaCharmNotchCost);
        UnequipCharmIfEquipped(ref diamondPointCharmEquipped, diamondPointNotchCost);
        UnequipCharmIfEquipped(ref daredevilsGambleCharmEquipped, daredevilsGambleCharmNotchCost);
        UnequipCharmIfEquipped(ref unimpairedCataclysmCharmEquipped, unimpairedCataclysmCharmNotchCost);
        UnequipCharmIfEquipped(ref airborneSupremacyCharmEquipped, airborneSupremacyCharmNotchCost);
        UnequipCharmIfEquipped(ref excessivelyExtendedCharmEquipped, excessivelyExtendedCharmNotchCost);
    }

    public bool PlayerHasEnoughCharmNotchesAvailableToEquipCharm(int charmCharmNotchesRequired) => currentPlayerCharmNotchesUsed + charmCharmNotchesRequired <= maxPlayerCharmNotches;
    
    /// <summary></summary>
    /// <param name="charmEquipped"></param>
    /// <param name="charmNotchCost"></param>
    /// <returns>
    /// 0 - charm equipped   successfully,
    /// 1 - charm unequipped successfully,
    /// 2 - charm equipped unsuccessfully - not enough charm notches available to equip charm
    /// </returns>

    public int CharmToggle(ref bool charmEquipped, int charmNotchCost)
    {
        if (PlayerHasEnoughCharmNotchesAvailableToEquipCharm(charmNotchCost) && !charmEquipped)
        {
            EquipCharm(ref charmEquipped, charmNotchCost);
            audioManager.PlayAudioClip(audioManager.mainMenuSounds, "CharmEquipped");
            return 0;
        }
        else if (charmEquipped)
        {
            UnequipCharm(ref charmEquipped, charmNotchCost);
            audioManager.PlayAudioClip(audioManager.mainMenuSounds, "CharmUnequipped");
            return 1;
        }
        else
        {
            audioManager.PlayAudioClip(audioManager.mainMenuSounds, "CantEquipCharm");
            return 2;
        }
    }

    public void ActivateAllCharmsCheck()
    {
        if (Game.CurrentGameModeIsClassic())
            ActivateAllClassicCharmsCheck();

        if (!Game.CurrentGameModeIsClassic())
            ActivateAllUltimateCharmsCheck();
    }

    private void ActivateAllClassicCharmsCheck()
    {
        if (stalwartShellCharmEquipped)
            ActivateStalwartShellCharm();
        if (soulCatcherCharmEquipped)
            ActivateSoulCatcherCharm();
        if (shamanStoneCharmEquipped)
            ActivateShamanStoneCharm();
        if (soulEaterCharmEquipped)
            ActivateSoulEaterCharm();
        if (sprintMasterCharmEquipped)
            ActivateSprintMasterCharm();
        if (spellTwisterCharmEquipped)
            ActivateSpellTwisterCharm();
        if (unbreakableStrengthCharmEquipped)
            ActivateUnbreakableStrengthCharm();
        if (markOfPrideCharmEquipped)
            ActivateMarkOfPrideCharm();
        if (longNailCharmEquipped)
            ActivateLongNailCharm();
        if (quickSlashCharmEquipped)
            ActivateQuickSlashCharm();
    }

    private void ActivateAllUltimateCharmsCheck()
    {
        if (spellboundGraceCharmEquipped)
            ActivateSpellboundGraceCharm();
        if (curseOfTheSoullessCharmEquipped)
            ActivateCurseOfTheSoullessCharm();
        if (arcaneEnigmaCharmEquipped)
            ActivateArcaneEnigmaCharm();
        if (diamondPointCharmEquipped)
            ActivateDiamondPointCharm();
        if (daredevilsGambleCharmEquipped)
            ActivateDaredevilsGambleCharm();
        if (excessivelyExtendedCharmEquipped)
            ActivateExcessivelyExtendedCharm();
    }
}
