using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int amountToSpawn;
    [SerializeField] BoxCollider spawnRegions;
    [SerializeField] Transform parentTransform;

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentTransform = GameObject.FindWithTag("EnemyList").transform;
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.parent = parentTransform;
            enemy.transform.position = RandomPointInBounds(spawnRegions);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Public Methods
    public static Vector3 RandomPointInBounds(BoxCollider collider)
    {
        Vector3 extents = collider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            1,
            Random.Range(-extents.z, extents.z));

        return collider.transform.TransformPoint(point);
    }
    #endregion
}
