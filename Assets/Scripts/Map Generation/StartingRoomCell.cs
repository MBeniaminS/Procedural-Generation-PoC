using UnityEngine;

public class StartingRoomCell : MonoBehaviour
{
    [SerializeField] Transform playerSpawnTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject.FindWithTag("Player").transform.position = playerSpawnTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
