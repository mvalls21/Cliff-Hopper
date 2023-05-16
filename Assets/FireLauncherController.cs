using System;
using UnityEngine;

public class FireLauncherController : MonoBehaviour
{
    private bool _fireballLaunched = false;

    public GameObject fireballPrefab;

    public void Update()
    {
        var hit = Physics.Raycast(transform.position, new Vector3(-1.0f, 0.0f, 0.0f), out RaycastHit info);

        if (hit && info.collider.gameObject.CompareTag("Player") && !_fireballLaunched)
        {
            _fireballLaunched = true;

            // Launch fireball
            var fireball = Instantiate(fireballPrefab);
            fireball.transform.position = transform.position + new Vector3(3.0f, 0.0f, 0.0f);

            var rigidbody = fireball.GetComponent<Rigidbody>();
            rigidbody.velocity = new Vector3(-6.0f, 0.0f, 0.0f);

            Debug.Log("Launched fireball");
        }
    }
}