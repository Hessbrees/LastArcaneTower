using UnityEngine;

public class PlayerProgressDataManager : MonoBehaviour
{
    public delegate void OnGoldChangeDelegate(int newValue);  //OnGoldChangeDelegate(int value);
    public event OnGoldChangeDelegate OnGoldChangeEvent;
    private void OnEnable()
    {
        LoadGame();
    }
    private int _gold;
    private int gold
    {
        get => _gold;
        set
        {
            if (_gold == value) return;
            _gold = value;
            if (OnGoldChangeEvent != null)
            {
                OnGoldChangeEvent(value);
            }
        }
    }
    private int[] stars = new int[20];

    private int starsSum
    {
        get
        {
            int value = 0;
            if(stars != null)
            foreach (var s in stars) value += s;
            return value;
        }
    }
    public void ChangeGoldAmount(int _gold)
    {
        gold += _gold;
    }

    public void ChangeGoldAmount(ScenesSettings_SO scenesSettings_SO)
    {
        gold += scenesSettings_SO.gold;
    }

    public void ResetSOGoldAmount(ScenesSettings_SO scenesSettings_SO) => scenesSettings_SO.gold = 0;
    public void ChangeStarsAmount(int index, int _stars)
    {
        if (stars[index] >= _stars) return;
        stars[index] = _stars;
    }
    public int GetLevelStarsAmount(int index) => stars[index];
    public int GetGoldAmount() => gold;
    public int GetStarsAmount() => starsSum;

    public void SaveGame()
    {
        PlayerDataScructure player = new PlayerDataScructure();
        player.gold = gold;
        player.stars = stars;

        SaveSystem.SavePlayer(player);
    }
    public void LoadGame()
    {
        PlayerDataScructure player = SaveSystem.LoadPlayer();
        gold = player.gold;
        stars = player.stars;
    }
}
[System.Serializable]
public class PlayerDataScructure
{
    public int gold;
    public int[] stars;

}