using UnityEngine;

public class SpawnPrefabAtPoint : MonoBehaviour
{
    public static SpawnPrefabAtPoint instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnPrefab(GameObject prefab, Vector3 spawnPosition, Transform parent)
    {
        GameObject m_spawnedPrefab = Instantiate(prefab);
        if (parent != null )
        {
            m_spawnedPrefab.transform.parent = parent;
        }
        m_spawnedPrefab.transform.position = spawnPosition;
    }
}
