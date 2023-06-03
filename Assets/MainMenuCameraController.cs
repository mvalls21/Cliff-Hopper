using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
