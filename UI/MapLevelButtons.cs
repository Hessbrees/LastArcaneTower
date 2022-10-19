using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapLevelButtons : MonoBehaviour
{
    private Button levelButton;
    [Header ("file with scene settiggs to change")]
    [SerializeField] ScenesSettings_SO sceneSetting;
    [Header ("new level parameters")]
    [SerializeField] LevelData_SO levelWaveSO;
    [SerializeField] Sprite levelBackgrund;

    private void Start()
    {
        Button btn = levelButton.GetComponent<Button>();
        btn.onClick.AddListener(LevelSettings);
    }

    void LevelSettings()
    {
        sceneSetting.levelData_SO = levelWaveSO;
        sceneSetting.background = levelBackgrund;
    }
}
