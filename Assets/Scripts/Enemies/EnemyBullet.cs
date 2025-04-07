using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    [SerializeField] GameObject damageHitPrefab;
    [SerializeField] GameObject environHitPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject m_hitPrefab;
        switch (collision.gameObject.layer)
        {
            // 8 - Enemy
            case 8:
                m_hitPrefab = damageHitPrefab;
                EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
                print(enemyHealth);
                enemyHealth.DestroySelf();
                break;

            // 9 - EnemyBullet
            case 9:
                m_hitPrefab = damageHitPrefab;
                break;

            // 10 - Player Layer
            case 10:
                m_hitPrefab = damageHitPrefab;
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                print(playerHealth);
                playerHealth.DestroySelf();
                break;

            // Everything else, mainly Arena Layer and Ground Layer
            default:
                m_hitPrefab = environHitPrefab;
                break;
        }
        SpawnPrefabAtPoint.instance.SpawnPrefab(m_hitPrefab, transform.position, SpawnPrefabAtPoint.instance.transform);
        Destroy(gameObject);
    }
}

