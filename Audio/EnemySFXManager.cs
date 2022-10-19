using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFXManager : MonoBehaviour
{
    [SerializeField]private AudioClip[] hitClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip[] movementClip;
    [SerializeField] private AudioClip[] atackClip;


    private AudioSource hitAudio;
    private AudioSource deathAudio;
    private AudioSource movementAudio;
    private AudioSource atackAudio;
    
    private GameObject hitObj;
    
    private void Awake()
    {
        hitObj = transform.Find("hit").gameObject;

        hitAudio = transform.Find("hit").GetComponent<AudioSource>();
        deathAudio = transform.Find("death").GetComponent<AudioSource>();
        movementAudio = transform.Find("movement").GetComponent<AudioSource>();
        atackAudio = transform.Find("atack").GetComponent<AudioSource>();

        deathAudio.clip = deathClip;

        StartCoroutine(ChangeClip(hitClip, hitAudio));
        StartCoroutine(ChangeClip(movementClip, movementAudio));
        StartCoroutine(ChangeClip(atackClip, atackAudio));
    }
    private void OnEnable()
    {
        GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().OnMuteSFXChange += MuteAllAudio;
        MuteAllAudio(GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().isEnemyMuted);
    }
    public void MuteAllAudio(bool isMute)
    {
        hitAudio.mute = isMute;
        deathAudio.mute = isMute;
        movementAudio.mute = isMute;
        atackAudio.mute = isMute;
    }

    public void hitAudioPlay() => hitAudio.Play();
    public void deathAudioPlay() => deathAudio.Play();
    public bool IsDeathAudioPlaying() => deathAudio.isPlaying;

    public void movementAudioPlay() => movementAudio.Play();
    public void atackAudioPlay() => atackAudio.Play();
    public float movementClipLenght => movementClip.Length;
    public void DisableHurtSFXAfterDeath()
    {
        hitObj.SetActive(false);
    }
    IEnumerator ChangeClip(AudioClip[] clip, AudioSource source)
    {
        int index = 0;
        source.clip = clip[index];

        while (true)
        {
            yield return new WaitUntil(() => source.isPlaying);
            yield return new WaitForSeconds(clip.Length);

            source.clip = clip[index];

            if (index + 1 < clip.Length) index++;
            else index = 0;
        }
    }

}
