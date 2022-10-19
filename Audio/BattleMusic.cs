using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusic : MonoBehaviour
{
    [SerializeField] AudioClip[] battleMusic;
    private AudioSource source;
    private int musicCounter = 0;
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        StartCoroutine(MenuMusicPlay());
    }
    IEnumerator MenuMusicPlay()
    {
        while (true)
        {
            source.clip = battleMusic[musicCounter];
            source.Play();

            yield return new WaitForSeconds((float)battleMusic[musicCounter].length);

            if (musicCounter + 1 < battleMusic.Length) musicCounter++;
            else musicCounter = 0;
        }
    }
}
