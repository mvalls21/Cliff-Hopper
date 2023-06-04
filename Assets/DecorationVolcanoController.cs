using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecorationVolcanoController : MonoBehaviour
{
    public GameObject fireballPrefab;

    public GameObject previousDirectionChange;

    private bool _fireballLaunched = false;

    private GameObject _fireball;

    public void Update()
    {
        var hit = Physics.Raycast(previousDirectionChange.transform.position, Vector3.up, out RaycastHit hitInfo);

        if (hit && hitInfo.collider.gameObject.CompareTag("Player") && !_fireballLaunched)
        {
            _fireball = Instantiate(fireballPrefab);
            _fireball.transform.position = transform.position;

            _fireball.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            var rigidbody = _fireball.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.AddForce(Vector3.up * 20.0f, ForceMode.Impulse);

            _fireballLaunched = true;
        }

        if (_fireballLaunched && _fireball.transform.position.y < transform.position.y - 1.0f) {
            Destroy(_fireball);
            _fireball = null;
        }
    }
}