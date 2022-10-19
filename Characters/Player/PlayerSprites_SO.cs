using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSprites_SO_", menuName = "Scriptable Object/PlayerSprites")]
public class PlayerSprites_SO : ScriptableObject
{
    [SerializeField] public Sprite[] sprites = new Sprite[3];
}

