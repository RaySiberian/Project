using UnityEngine;

public class EnemyAnimeation : MonoBehaviour
{
    public Enemy Enemy;

    public void EnemyHit()
    {
        Enemy.DealDamage();
    }
}
