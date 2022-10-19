using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    private Player_SO player_SO;

    private void Awake() => player_SO = ResourceLoader.LoadPlayer_SO();

    public void Upgrade()
    {
        if (player_SO.upgradePointsAvailable == 0)
            return;

        player_SO.upgradePointsAvailable--;
    }
}

