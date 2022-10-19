using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public delegate void OnMuteSFXChangeDelegate(bool isMuted);
    public event OnMuteSFXChangeDelegate OnMuteSFXChange;

    private GameObject buttonClick;
    private GameObject gameOverMusic;
    private GameObject coinSFX;
    private GameObject menuMusic;
    private GameObject battleMusic;
    private GameObject spellSFX;
    private GameObject startWaveButtonSFX;

    private SpellsSFXManager spellSFXManager;

    private AudioSource audioButtonClick;
    private AudioSource audioGameOverMusic;
    private AudioSource audioCoinSFX;
    private AudioSource audioMenuMusic;
    private AudioSource audioBattleMusic;
    private AudioSource audioStartNextWaveSFX;
    private AudioSource audioStartWaveButtonSFX;

    public bool isEnemyMuted = false;
    [SerializeField] private AudioClip[] coinSFXClip;
    private int coinCounter = 0;
    private void Awake()
    {
        spellSFXManager = transform.Find("SpellsSFX").GetComponent<SpellsSFXManager>();

        buttonClick = transform.Find("ButtonClickSFX").gameObject;
        gameOverMusic = transform.Find("GameOverMusic").gameObject;
        coinSFX = transform.Find("CoinSFX").gameObject;
        menuMusic = transform.Find("MenuMusic").gameObject;
        battleMusic = transform.Find("BattleMusic").gameObject;
        startWaveButtonSFX = transform.Find("StartWaveButtonSFX").gameObject;

        audioButtonClick = buttonClick.GetComponent<AudioSource>();
        audioGameOverMusic = gameOverMusic.GetComponent<AudioSource>();
        audioCoinSFX = coinSFX.GetComponent<AudioSource>();
        audioMenuMusic = menuMusic.GetComponent<AudioSource>();
        audioBattleMusic = battleMusic.GetComponent<AudioSource>();
        audioStartWaveButtonSFX = startWaveButtonSFX.GetComponent <AudioSource>();
        audioStartNextWaveSFX = transform.Find("StartWaveButtonSFX").GetComponent<AudioSource>();

    }

    public void GetAudioStartNextWaveSFX() => audioStartNextWaveSFX.Play();
    public void GetAudioButtonClick() => audioButtonClick.Play();
    public void GetAudioGameOverMusic() => audioGameOverMusic.Play();
    public void GetAudioCoinSFX()
    {
        audioCoinSFX.clip = coinSFXClip[coinCounter];
        audioCoinSFX.Play();

        if (coinCounter + 1 < coinSFXClip.Length) coinCounter++;
        else coinCounter = 0;
    }


    public float GetAudioCoinSFXLenght() => audioCoinSFX.clip.length;

    public void MuteMusic()
    {
        audioGameOverMusic.mute = true;
        audioMenuMusic.mute = true;
        audioBattleMusic.mute = true;
        isEnemyMuted = true;

        if(OnMuteSFXChange != null)
        OnMuteSFXChange(isEnemyMuted);
    }
    public void UnmuteMusic()
    {
        audioGameOverMusic.mute = false;
        audioMenuMusic.mute = false;
        audioBattleMusic.mute = false;
        isEnemyMuted = false;

        if (OnMuteSFXChange != null)
            OnMuteSFXChange(isEnemyMuted);
    }
    public void MuteSFX()
    {
        audioButtonClick.mute = true;
        audioCoinSFX.mute = true;
        spellSFXManager.MuteAllSpells();
        audioStartNextWaveSFX.mute = true;
        isEnemyMuted = true;
    }
    public void UnmuteSFX()
    {
        audioButtonClick.mute = false;
        audioCoinSFX.mute = false;
        spellSFXManager.UnMuteAllSpells();
        audioStartNextWaveSFX.mute = false;
        isEnemyMuted = false;
    }
    public void TurnOffBattleMusic() => battleMusic.SetActive(false);
    public void TurnOnBattleMusic() => battleMusic.SetActive(true);
    public void TurnOffMenuMusic() => menuMusic.SetActive(false);
    public void TurnOnMenuMusic() => menuMusic.SetActive(true);
    public void GetProjectileAudio() => spellSFXManager.GetProjectileAudio();
    public void GetWallAudio() => spellSFXManager.GetWallAudio();
    public void GetSkyDropAudio() => spellSFXManager.GetSkyDropAudio();
    public void GetSpecialAudio(int specialIndex) => spellSFXManager.GetSpecialAudio(specialIndex);

}
