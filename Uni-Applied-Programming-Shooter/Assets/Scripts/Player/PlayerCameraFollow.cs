using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followPlayer;
    [SerializeField] private Vector3 playerOffset;
    [SerializeField] private float cameraMoveSpeed = 5f;

    private Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (followPlayer != null)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playerOffset, cameraMoveSpeed * Time.deltaTime);
        }
    }
}
