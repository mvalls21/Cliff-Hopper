using UnityEngine;

public class LavaMovementController : MonoBehaviour
{
    public float scrollSpeed = 0.2f;

    private Renderer _renderer;

    private GameObject _cameraObject;

    private Vector3 _lastCameraPosition;

    private Vector2 _currentTextureOffset;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
        _cameraObject = transform.parent.gameObject;

        _lastCameraPosition = _cameraObject.transform.position;
        _currentTextureOffset = Vector2.zero;
    }

    public void Update()
    {
        var movement = _cameraObject.transform.position - _lastCameraPosition;
        _lastCameraPosition = _cameraObject.transform.position;

        _currentTextureOffset += new Vector2(movement.x, movement.z) * scrollSpeed;
        _renderer.material.SetTextureOffset("_MainTex", _currentTextureOffset);
    }
}