using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO_", menuName = "Scriptable Object/EnemyBehaviour")]
public class EnemyBehaviour_SO : ScriptableObject
{
    public float movementSpeeed;
    public float damage;
    [Header(" Time beetweeen atack [s]")]
    public float timeBetweenAtack;

    //HP and MP stats

    [Header(" HP stats, Regeneration per second")]
    public float maxHP;
    public float regHP;

    //min-max gold earn per one mob
    [Header("Gold award")]
    public int minGoldAward;
    public int maxGoldAward;
}

