using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = playerTransform.position;
        targetPos.y = transform.position.y;

        transform.LookAt(targetPos);
    }
}
