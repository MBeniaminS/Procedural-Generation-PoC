using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform doorEntry1;
    [SerializeField] Transform doorEntry2;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.tag == "Player")
        {
            /// When player hits door, it checks which entrance point is the player closest to,
            /// and whichever it is closest to it teleports the player to the other side's
            /// entrance point.
            Transform playerTransform = other.transform;
            float compareToEntry1 = Vector3.Distance(playerTransform.position, doorEntry1.position);
            float compareToEntry2 = Vector3.Distance(playerTransform.position, doorEntry2.position);

            if (compareToEntry1 < compareToEntry2)
            {
                playerTransform.position = doorEntry2.position;
            }
            else
            {
                playerTransform.position = doorEntry1.position;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
