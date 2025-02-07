using System.Collections;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // FIX THIS
    IEnumerator DestroyAfterXSeconds(float seconds)
    {
        yield return WaitForSeconds(seconds);
        Destroy(gameObject);
        yield return null;
    }
}
