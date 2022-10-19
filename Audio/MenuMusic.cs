using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class MenuMusic : MonoBehaviour
{
    [SerializeField] AudioClip[] menuMusic;
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
        while(true)
        {
            source.clip = menuMusic[musicCounter];
            source.Play();
            
            yield return new WaitForSeconds((float)menuMusic[musicCounter].length) ;

            if (musicCounter + 1 < menuMusic.Length) musicCounter++;
            else musicCounter = 0;
        }
    }

}
