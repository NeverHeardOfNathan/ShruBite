using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 20f;
    public float lifetime = 3f;

    private Vector3 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        if (direction != Vector3.zero)
        {
            // Calculate rotation to face direction
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // Apply the rotation with X = 90 to keep bullet horizontal
            transform.rotation = Quaternion.Euler(90, angle, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.Die();
            }
            Destroy(gameObject);
        }
    }
}
