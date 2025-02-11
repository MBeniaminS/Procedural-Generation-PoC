using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] GameObject damageHitPrefab;
    [SerializeField] GameObject environHitPrefab;

    //[SerializeField] bool bouncesOffWalls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

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
                break;

            // Everything else, mainly Arena Layer and Ground Layer
            default:
                m_hitPrefab = environHitPrefab;
                break;
        }
        SpawnPrefabAtPoint.instance.SpawnPrefab(m_hitPrefab, transform.position, null);
        Destroy(gameObject);
    }
}
