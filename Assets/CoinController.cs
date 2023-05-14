using Unity.VisualScripting;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Vector3 _originalPosition;

    public GameManager GameManager;
    
    public void Start()
    {
        _originalPosition = transform.position;
    }

    public void Update()
    {
        transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));

        var y = Mathf.Sin(Time.time) * 0.2f;
        transform.position = _originalPosition + new Vector3(0.0f, y, 0.0f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().CompareTag("Player"))
            Destroy(this.GameObject());
        
        GameManager.IncreaseCoin();
    }
}
