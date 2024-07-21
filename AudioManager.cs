using UnityEngine;
using System.Collections;
using System;



public class AudioManager : MonoBehaviour
{

    //      OBJECTS:


    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerSoulVessel playerSoulVessel;
    [SerializeField] private PlayerNail playerNail;
    [SerializeField] private PlayerCharms playerCharms;
    [SerializeField] private PlayerSpells playerSpells;


    //      SOUNDS:


    public Audio[][] allSounds;

    public Audio[] playerMovementSounds;
    public Audio[] playerHealthSounds;
    public Audio[] playerSoulVesselSoundsWithoutSoulGain;
    public Audio[] playerNailSoundsWithoutSlash;
    public Audio[] playerCharmsSounds;
    public Audio[] playerSpellsSounds;

    public Audio[] playerSoulGainSounds;
    public Audio[] playerSlashSounds;

    public Audio[] enemyGeneral;

    public Audio[] boss1Movement;
    public Audio[] boss1Attacks;

    public Audio[] mainMenuSounds;


    //      MUSIC:


    public Audio[] musicArray;


    //      HELPER VARIABLES:


    private Coroutine slowlyTurningAllSoundsBackCoroutineActive;


    //      SETUP:


    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject("AudioManager");
                    _instance = managerObject.AddComponent<AudioManager>();
                    DontDestroyOnLoad(managerObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        InitializeAllAudioClipArrays();

        playerSoulGainSounds.Contains(enemyGeneral[0]);
    }

    private void InitializeAllAudioClipArrays()
    {
        InitializeAllSoundsArray();
        InitializeMusicArray();
    }

    private void InitializeAllSoundsArray()
    {
        allSounds = new Audio[][]   // HARDCODED
        {
             playerMovementSounds,
             playerHealthSounds,
             playerSoulVesselSoundsWithoutSoulGain,
             playerNailSoundsWithoutSlash,
             playerCharmsSounds,
             playerSpellsSounds,
             playerSoulGainSounds,
             playerSlashSounds,
             enemyGeneral,
             boss1Movement,
             boss1Attacks,
             mainMenuSounds
        };

        foreach (Audio[] soundArray in allSounds)
        {
            foreach (Audio sound in soundArray)
            {
                sound.audioSource = gameObject.AddComponent<AudioSource>();

                sound.audioSource.clip = sound.audioClip;
                sound.audioSource.volume = sound.volume;
                sound.audioSource.pitch = sound.pitch;
                sound.audioSource.loop = sound.loop;
            }
        }
    }

    private void InitializeMusicArray()
    {
        foreach (Audio sound in musicArray)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();

            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }


    //      METHODS:


    public Audio PlayAudioClip(Audio[] audioClipArray, string audioClipName)
    {
        Audio audioClip = AudioClipAudio(audioClipArray, audioClipName);

        if (audioClip == null)
        {
            Debug.LogWarning("ERROR - Audio clip with name '" + audioClipName + "' could not be found.");
            return null;
        }

        audioClip.audioSource.Play();
        return audioClip;
    }

    public Audio AudioClipAudio(Audio[] audioClipArray, string audioClipName)
    {
        foreach (Audio audioClip in audioClipArray)
        {
            if (audioClip.audioClip.ToString().ReformattedAudioClipName() == audioClipName)
                return audioClip;
        } 

        Debug.LogWarning("ERROR - Audio clip with name '" + audioClipName + "' could not be found.");
        return null;
    }

    public bool CurrentlyPlayingAudioClip(Audio[] audioClipArray, string audioClipName) =>
        AudioClipAudio(audioClipArray, audioClipName).audioSource.isPlaying 
        && AudioClipAudio(audioClipArray, audioClipName).audioClip.ToString().ReformattedAudioClipName() == audioClipName;
    
    public Audio PlayAudioClipIfItsNotPlaying(Audio[] audioClipArray, string audioClipName, bool condition)
    {
        if (condition && !CurrentlyPlayingAudioClip(audioClipArray, audioClipName))
            PlayAudioClip(audioClipArray, audioClipName);
        else if (!condition && CurrentlyPlayingAudioClip(audioClipArray, audioClipName))
            StopAudioClip(audioClipArray, audioClipName);

        return AudioClipAudio(audioClipArray, audioClipName);
    }

    public Audio PlayRandomSound(Audio[] soundArray)
    {
        int randomNumber = UnityEngine.Random.Range(0, soundArray.Length);

        Audio audioClip = soundArray[randomNumber];

        if (audioClip == null)
        {
            Debug.LogWarning("Array with name '" + soundArray.ToString() + "' is either null, or it has no elements.");
            return null;
        }  

        audioClip.audioSource.Play();

        return soundArray[randomNumber];
    }

    public void StopAudioClip(Audio[] audioClipArray, string audioClipName)
    {
        foreach (Audio audioClip in audioClipArray)
        {
            if (audioClip.audioClip.ToString().ReformattedAudioClipName() == audioClipName)
            {
                audioClip.audioSource.Stop();
                return;
            }
        }
        Debug.LogWarning("ERROR - Audio clip with name '" + audioClipName + "' could not be found.");
    }

    public void StopAllArraySounds(Audio[] audioClipArray) => Array.ForEach(audioClipArray, sound => sound.audioSource.Stop());

    public void StopAllSounds()
    {
        foreach (Audio[] soundArray in allSounds)
        {
            foreach (Audio sound in soundArray)
            {
                if (CurrentlyPlayingAudioClip(soundArray, sound.audioClip.ToString().ReformattedAudioClipName()))
                    sound.audioSource.Stop();
            }  
        }
    }

    public void StopAllMusic()
    {
        foreach (Audio music in musicArray)
        {
            if (CurrentlyPlayingAudioClip(musicArray, music.audioClip.ToString().ReformattedAudioClipName()))
                music.audioSource.Stop();
        }
    }

    public void ChangeAudioClipVolume(Audio[] audioClipArray, string audioClipName, float volume)
    {
        foreach (Audio audioClip in audioClipArray)
        {
            if (audioClip.audioClip.ToString().ReformattedAudioClipName() == audioClipName)
                audioClip.audioSource.volume = volume;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="volume"> 0 - mute volume, 1 - maximum volume </param>

    public void ChangeAllSoundsVolume(float volume)
    {
        foreach (Audio[] soundArray in allSounds)
        {
            foreach (Audio sound in soundArray)
                sound.audioSource.volume = volume;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="volume"> 0 - mute volume, 1 - maximum volume </param>

    public void ChangeAllMusicVolume(float volume)
    {
        foreach (Audio music in musicArray)
            music.audioSource.volume = volume;
    }

    public void SlowlyTurnAllAudioClipsVolumeBackAfterDelay(float delay, float transitionTime, bool changeSoundAudio, bool changeMusicAudio)
    {
        if (slowlyTurningAllSoundsBackCoroutineActive != null)
            StopCoroutine(slowlyTurningAllSoundsBackCoroutineActive);

        slowlyTurningAllSoundsBackCoroutineActive = StartCoroutine(SlowlyTurnAllAudioClipsVolumeBack(delay, transitionTime, changeSoundAudio, changeMusicAudio));
    }

    private IEnumerator SlowlyTurnAllAudioClipsVolumeBack(float delay, float transitionTime, bool changeSoundAudio, bool changeMusicAudio)
    {
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            if (changeSoundAudio)
                ChangeAllSoundsVolume(elapsedTime / transitionTime);
            if (changeMusicAudio)
                ChangeAllMusicVolume(elapsedTime / transitionTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        slowlyTurningAllSoundsBackCoroutineActive = null;
    }
}
