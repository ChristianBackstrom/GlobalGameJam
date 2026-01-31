using UnityEngine;

public class EnemySpawner_Enemy : MonoBehaviour
{
    [HideInInspector] public EnemySpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.OnEnemyDestroyed();
    }
}
