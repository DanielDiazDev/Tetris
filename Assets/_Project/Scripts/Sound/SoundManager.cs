using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Prefs;
using UnityEngine;

namespace _Project.Scripts.Sound
{
    public interface ISoundManager
    {
        void PlayMusic(MusicType musicType);
        void PlaySfx(SfxType sfxType);
        void ToggleMusic(bool state);
        void ToggleSfx(bool state);
    }
    public enum MusicType
    {
        MainMenuMusic
    }

    public enum SfxType
    {
        DestroyPieceSfx,
    }
    
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        [Serializable]
        private class MusicData
        {
            public MusicType type;
            public AudioClip clip;
        }

        [Serializable]
        private class SFXData
        {
            public SfxType type;
            public AudioClip clip;
        }
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;
        private Dictionary<MusicType, AudioClip> musicAudioDict = new();
        private Dictionary<SfxType, AudioClip> sfxAudioDict = new();
        [SerializeField] private List<MusicData> musicClips;
        [SerializeField] private List<SFXData> sfxClips;
        private bool musicOn = true;
        private bool sfxOn = true;
        private void Start()
        {
            musicAudioDict = musicClips.ToDictionary(mc => mc.type, mc => mc.clip);
            sfxAudioDict = sfxClips.ToDictionary(sfx => sfx.type, sfx => sfx.clip);
            if (!PlayerPrefsManager.HasKey("MusicToggle"))
            {
                PlayerPrefsManager.SetBool("MusicToggle", true);
            }
            if (!PlayerPrefsManager.HasKey("SfxToggle"))
            {
                PlayerPrefsManager.SetBool("SfxToggle", true);
            }
            sfxOn =  PlayerPrefsManager.GetBool("SfxToggle");
            musicOn = PlayerPrefsManager.GetBool("MusicToggle");
            ToggleMusic(musicOn);
        }

        public void PlayMusic(MusicType musicType)
        {
            if (!musicAudioDict.TryGetValue(musicType, out AudioClip clip))
            {
                Debug.LogError("No clip found for music type: " + musicType);
                return;
            }

            if (musicAudioSource.clip == clip && musicAudioSource.isPlaying)
                return;

            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        public void PlaySfx(SfxType sfxType)
        {
            if (!sfxAudioDict.TryGetValue(sfxType, out AudioClip clip))
            {
                Debug.LogError("No clip found for sfx type: " + sfxType);
            }
            sfxAudioSource.Stop();
            sfxAudioSource.PlayOneShot(clip);
        }

        public void ToggleMusic(bool state)
        {
            musicOn = state;
            if (!musicOn)
            {
                musicAudioSource.mute = true;
                PlayerPrefsManager.SetBool("MusicToggle", false);
            }
            else
            {
                musicAudioSource.mute = false;
                PlayerPrefsManager.SetBool("MusicToggle", true);
            }
        }

        public void ToggleSfx(bool state)
        {
            sfxOn = state;
            sfxAudioSource.mute = !sfxOn;
            PlayerPrefsManager.SetBool("SfxToggle", sfxOn);
        }
    }
}