using UnityEngine;

[CreateAssetMenu(fileName = "Player_SO_", menuName = "Scriptable Object/Player")]
public class Player_SO : ScriptableObject
{
    public int upgradePointsAvailable;

    private void Awake()
    {
        upgradePointsAvailable = 5;
    }
}

