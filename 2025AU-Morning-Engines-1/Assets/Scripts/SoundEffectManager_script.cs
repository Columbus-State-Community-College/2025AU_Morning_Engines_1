using UnityEngine;
using System.Collections.Generic;
/* 
This setup for playing audio files was tutorialized by a very 
helpful scottish fellow by the name of HaggisBytes on youtube

The basic idea of this setup is that we make a list and dictionary of sound effects that
other scripts in the game can call upon to play sound effects
for example the code that makes the vending machine keypad work would play 
the associated sound when the key is clicked
*/
public class SoundEffectManager_script : MonoBehaviour
{

    public enum SoundType // A list that we call back to for the specific sound effects
    {
        UI,
        Snack_Stuck,
        Level_Fail,
        Level_Win,
        Soda_Can_Break,
        Snack_Motor,
        Cash_Insert,
        Coin_Insert,
        Machine_Thunk,
        Soda_Fizz,
        Soda_Bottle_Drop,
        Soda_Can_Drop,
        Candy_Bar_Drop,
        Candy_Packet_Drop,
        Chip_Bag_Drop,
        Keypad_input
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [HideInInspector]
        public AudioSource Source;
    }

    public static SoundEffectManager_script Instance; //

    public Sound[] AllSoundfxs;

    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();

    private void Awake()
    {
        Instance = this;

        foreach (var s in AllSoundfxs)
        {
            _soundDictionary[s.Type] = s;
        }
    }

    public void Play(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound{type} not found.");
            return;
        }

        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();

        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;

        audioSrc.Play();

        Destroy(soundObj, s.Clip.length);
    }

    
    

}
