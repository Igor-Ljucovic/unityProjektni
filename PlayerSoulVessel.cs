using UnityEngine;

public class PlayerSoulVessel : MonoBehaviour
{ 

    //      OBJECTS:


    [SerializeField] private PlayerCharms playerCharms;
    [SerializeField] private AudioManager audioManager;


    //      GENERAL:


    private int playerMaximumSoul;
    public  int maximumBigSoulVesselSoul = 99;
    public  int maximumSmallSoulVesselSoul = 33;
    public  int playerCurrentSoulGain = 11;


    //      TIMERS:


    public int currentSoulPourTimer = 0;
    public int totalSoulPourTimer = 75;


    //      COUNTERS:


    public float currentBigSoulVesselSoul = 0;

    public float currentSmallSoulVessel1Soul = 0;
    public float currentSmallSoulVessel2Soul = 0;
    public float currentSmallSoulVessel3Soul = 0;

    
    //      HELPER VARIABLES:


    private bool soulFullBeforePlayingSound = false;


    //      SETUP:


    private void Start()
    {
        playerMaximumSoul = maximumBigSoulVesselSoul + 3 * maximumSmallSoulVesselSoul;
    }


    //      METHODS:


    public void SetPlayerSoulAfterSlashingEnemy()
    {
        playerCharms.TriggerOverflowingEssenceCharmCheck();

        audioManager.PlayRandomSound(audioManager.playerSoulGainSounds);

        soulFullBeforePlayingSound = BigSoulVesselFull();

        if (GetCurrentPlayerSoul() + playerCurrentSoulGain > playerMaximumSoul)
            SetAllVesselsToMaximumSoul();
        else
            FillSoulVessels();

        if (!soulFullBeforePlayingSound && BigSoulVesselFull())
            audioManager.PlayAudioClip(audioManager.playerSoulVesselSoundsWithoutSoulGain, "FullSoul");
    }

    private float GetCurrentPlayerSoul() => currentBigSoulVesselSoul + currentSmallSoulVessel1Soul + currentSmallSoulVessel2Soul + currentSmallSoulVessel3Soul;

    private void SetAllVesselsToMaximumSoul() =>
        (currentSmallSoulVessel1Soul, currentSmallSoulVessel2Soul, currentSmallSoulVessel3Soul, currentBigSoulVesselSoul) =
        (maximumSmallSoulVesselSoul,  maximumSmallSoulVesselSoul,  maximumSmallSoulVesselSoul,  maximumBigSoulVesselSoul);
    
    public bool AllSoulVesselsFull() =>
        (currentSmallSoulVessel1Soul, currentSmallSoulVessel2Soul, currentSmallSoulVessel3Soul, currentBigSoulVesselSoul) ==
        (maximumSmallSoulVesselSoul, maximumSmallSoulVesselSoul, maximumSmallSoulVesselSoul, maximumBigSoulVesselSoul);

    private bool BigSoulVesselFull() => currentBigSoulVesselSoul == maximumBigSoulVesselSoul;

    public void RoundDownAllSoulVessels() =>
        (currentSmallSoulVessel1Soul, currentSmallSoulVessel2Soul, currentSmallSoulVessel3Soul, currentBigSoulVesselSoul) =
        (Mathf.Floor(currentSmallSoulVessel1Soul), Mathf.Floor(currentSmallSoulVessel2Soul), Mathf.Floor(currentSmallSoulVessel3Soul), Mathf.Floor(currentBigSoulVesselSoul));

    private void FillSoulVessels()
    {
        for (int i = playerCurrentSoulGain; i > 0; i--)
        {
            if (currentBigSoulVesselSoul < maximumBigSoulVesselSoul)
                currentBigSoulVesselSoul++;
            else if (currentSmallSoulVessel1Soul < maximumSmallSoulVesselSoul)
                currentSmallSoulVessel1Soul++;
            else if (currentSmallSoulVessel2Soul < maximumSmallSoulVesselSoul)
                currentSmallSoulVessel2Soul++;
            else if (currentSmallSoulVessel3Soul < maximumSmallSoulVesselSoul)
                currentSmallSoulVessel3Soul++;
        }
    }

    public void DrainSoulVesselsCheck(float drainSoulAmount) {
        if (currentBigSoulVesselSoul > drainSoulAmount)
            DrainSoulVessels(drainSoulAmount);
    }

    public void DrainSoulVessels(float drainSoulAmount) => currentBigSoulVesselSoul -= drainSoulAmount;

    public void ResetBigSoulVesselSoulToZero() => currentBigSoulVesselSoul = 0;

    private bool AllSmallSoulVesselsEmpty() => currentSmallSoulVessel1Soul == 0 && currentSmallSoulVessel2Soul == 0 && currentSmallSoulVessel3Soul == 0;
    
    public void FillBigSoulVesselCheck()
    {
        if (currentBigSoulVesselSoul < maximumBigSoulVesselSoul && !AllSmallSoulVesselsEmpty())
        {
            if (currentSoulPourTimer == 0)
                PourSmallSoulVesselsSoulIntoBigVessel();
            else
                currentSoulPourTimer--;
        }
        else
            currentSoulPourTimer = totalSoulPourTimer;
    }

    private void PourSmallSoulVesselsSoulIntoBigVessel()
    {
        soulFullBeforePlayingSound = BigSoulVesselFull();

        while (currentBigSoulVesselSoul < maximumBigSoulVesselSoul && !AllSmallSoulVesselsEmpty())
        {
            if (currentSmallSoulVessel3Soul > 0 && currentSmallSoulVessel2Soul < maximumSmallSoulVesselSoul)
            {
                currentSmallSoulVessel3Soul--;
                currentSmallSoulVessel2Soul++;
            }
            else if (currentSmallSoulVessel2Soul > 0 && currentSmallSoulVessel1Soul < maximumSmallSoulVesselSoul)
            {
                currentSmallSoulVessel2Soul--;
                currentSmallSoulVessel1Soul++;
            }
            else if (currentSmallSoulVessel1Soul > 0 && currentBigSoulVesselSoul < maximumBigSoulVesselSoul)
            {
                currentSmallSoulVessel1Soul--;
                currentBigSoulVesselSoul++;
            }
        }
        PourSmallSoulVesselsSoulIntoBigVesselBugFix32Soul();

        if (!soulFullBeforePlayingSound && BigSoulVesselFull())
            audioManager.PlayAudioClip(audioManager.playerSoulVesselSoundsWithoutSoulGain, "FullSoul");
    }

    private void PourSmallSoulVesselsSoulIntoBigVesselBugFix32Soul()
    {
        if (currentSmallSoulVessel1Soul == maximumSmallSoulVesselSoul - 1)
        {
            currentSmallSoulVessel1Soul++;
            currentSmallSoulVessel2Soul--;
        }
        if (currentSmallSoulVessel2Soul == maximumSmallSoulVesselSoul - 1)
        {
            currentSmallSoulVessel2Soul++;
            currentSmallSoulVessel3Soul--;
        }
    }
}
