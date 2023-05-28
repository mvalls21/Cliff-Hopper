using System;
using UnityEngine;

public class FireLauncherController : MonoBehaviour
{
    private bool _fireballLaunched = false;

    public GameObject fireballPrefab;

    private GameObject _fireball;

    public void Update()
    {
        var hit = Physics.Raycast(transform.position, new Vector3(-1.0f, 0.0f, 0.0f), out RaycastHit info);

        if (hit && info.collider.gameObject.CompareTag("Player") && !_fireballLaunched)
        {
            _fireballLaunched = true;

            // Launch fireball
            _fireball = Instantiate(fireballPrefab);
            _fireball.transform.position = transform.position + new Vector3(2.0f, 0.0f, 0.0f);

            var rigidbody = _fireball.GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(-4.0f, 0.0f, 0.0f);

            Debug.Log("Launched fireball");
        }

        if (_fireballLaunched && _fireball != null && _fireball.transform.position.x < transform.position.x - 20.0f)
        {
            Destroy(_fireball);
            _fireball = null;
        }
    }
}