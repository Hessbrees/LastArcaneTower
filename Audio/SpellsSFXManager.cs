using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellsSFXManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] projectileClip;
    [SerializeField] private AudioClip[] wallClip;
    [SerializeField] private AudioClip[] skyDropClip;
    [SerializeField] private AudioListClip[] specialListClip;
    
    [Serializable]
    public struct AudioListClip
    {
        public int specialCounter;
        public AudioClip[] specialClip;
    }

    private AudioSource projectile;
    private AudioSource wall;
    private AudioSource skyDrop;
    private AudioSource special;

    private int projectileCounter = 0;
    private int wallCounter = 0;
    private int skyDropCounter = 0;

    private void Awake()
    {
        projectile = transform.Find("Projectile").GetComponent<AudioSource>();
        wall = transform.Find("Wall").GetComponent<AudioSource>();
        skyDrop = transform.Find("SkyDrop").GetComponent<AudioSource>();
        special = transform.Find("Special").GetComponent<AudioSource>();
    }

    public void GetProjectileAudio() => PlayAudioSequence(projectile, ref projectileCounter, projectileClip);
    public void GetWallAudio() => PlayAudioSequence(wall, ref wallCounter, wallClip);
    public void GetSkyDropAudio() => PlayAudioSequence(skyDrop, ref skyDropCounter, skyDropClip);
    public void GetSpecialAudio(int specialIndex) => PlayAudioSequence(special, ref specialListClip[specialIndex].specialCounter, specialListClip[specialIndex].specialClip);

    public void MuteAllSpells()
    {
        projectile.mute = true;
        wall.mute = true;
        skyDrop.mute = true;
        special.mute = true;
    }
    public void UnMuteAllSpells()
    {
        projectile.mute = false;
        wall.mute = false;
        skyDrop.mute = false;
        special.mute = false;
    }

    private void PlayAudioSequence(AudioSource audio, ref int counter, AudioClip[] audioClips)
    {
        if (counter == null) counter = 0;

        if(audio.clip != audioClips[counter]) audio.clip = audioClips[counter];

        //audio.Play();
        audio.PlayOneShot(audio.clip);

        if (counter + 1 < audioClips.Length) counter++;
        else counter = 0;
    }

}
