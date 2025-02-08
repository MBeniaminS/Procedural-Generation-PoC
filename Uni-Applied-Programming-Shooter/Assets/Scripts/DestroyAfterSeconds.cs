using System.Collections;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float seconds;

    private void Start()
    {
        StartCoroutine(DestroyAfterXSeconds(seconds));
    }

    IEnumerator DestroyAfterXSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
        yield return null;
    }
}
