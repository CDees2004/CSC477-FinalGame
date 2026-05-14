using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    SWING,
    PLAYER_HURT,
    ENEMY_HURT,
    SPAWN_ENEMY,
    MENU_CLICK,
    SHOP_YES,
    SHOP_NO,
    ROOM_CLEAR,
    HEAL,
}


// Helper class storing sounds
public class SoundCollection
{
    private AudioClip[] clips;
    private int lastClipIndex;

    public SoundCollection(params string[] clipNames)
    {
        this.clips = new AudioClip[clipNames.Length];
        for (int i = 0; i < clipNames.Length; i++)
        {
            // unity goes through folder named specifically Resources 
            // to be able to dynamically load files of certain names 
            clips[i] = Resources.Load<AudioClip>(clipNames[i]);
            if (clips[i] == null)
            {

                Debug.LogError($"dynamically loaded clip is null {clipNames[i]}");
            }
        }
        lastClipIndex = -1;
    }

    public AudioClip GetRandomClip()
    {
        if (clips.Length == 0)
        {
            Debug.LogWarning("Must have at least one clip");
            return null;
        }

        else if (clips.Length == 1)
        {
            return clips[0];
        }

        else
        {
            int index = lastClipIndex;
            while (index == lastClipIndex)
            {
                index = Random.Range(0, clips.Length);
            }
            lastClipIndex = index;
            return clips[index];
        }
    }
}


// forces an audio source component onto the sound manager 
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public float mainVolume = 1.0f;
    private Dictionary<SoundType, SoundCollection> sounds;
    private AudioSource audioSrc;

    // making singleton so it can be accessed from anywhere directly 
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        audioSrc = GetComponent<AudioSource>();
        sounds = new()
        {
            // Must match the file path WITHIN the Resources folder
            {SoundType.SWING, new SoundCollection("Swing") },
            {SoundType.ENEMY_HURT, new SoundCollection("enemyHurt1", "enemyHurt2", "enemyHurt3") },
            {SoundType.PLAYER_HURT, new SoundCollection("playerHurt", "playerHurt2", "playerHurt3", "playerHurt4") },
            {SoundType.SPAWN_ENEMY, new SoundCollection("spawnEnemy1", "spawnEnemy2", "spawnEnemy3") },
            {SoundType.MENU_CLICK, new SoundCollection("menuClick") },
            {SoundType.SHOP_YES, new SoundCollection("shopYES") },
            {SoundType.SHOP_NO, new SoundCollection("shopNO") },
            {SoundType.ROOM_CLEAR, new SoundCollection("roomClear", "roomClear2", "roomClear3") },
            {SoundType.HEAL, new SoundCollection("roomHeal", "roomHeal2") },
        };
    }


    public static void Play(SoundType type, AudioSource extAudioSource = null, float pitch = -1.0f)
    {
        if (Instance.sounds.ContainsKey(type))
        {
            extAudioSource ??= Instance.audioSrc;
            extAudioSource.volume = Random.Range(0.7f, 1.0f) * Instance.mainVolume;
            extAudioSource.pitch = pitch >= 0 ? pitch : Random.Range(0.75f, 1.25f);
            extAudioSource.clip = Instance.sounds[type].GetRandomClip();
            // debug 
            extAudioSource.Play();

        }
    }
}

// for use : SoundManager.Play(SoundType.CLICK_ON);
