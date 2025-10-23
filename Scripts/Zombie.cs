using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Inscribed")]
    public float moveSpeed = 3f;
    public int scoreValue = 100;
    public GameObject healthPowerUpPrefab;
    public GameObject fireRatePowerUpPrefab;
    public float powerUpChance = 0.2f;

    private Transform playerTransform;
    private Rigidbody rb;
    private bool hasPlayedGroan = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        rb = GetComponent<Rigidbody>();

        if (Random.value < 0.3f && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ZombieGroan");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0;

            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distToPlayer < 5f && !hasPlayedGroan && Random.value < 0.01f)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("ZombieGroan");
                }
                hasPlayedGroan = true;
                Invoke("ResetGroan", 2f);
            }
        }
    }

    void ResetGroan()
    {
        hasPlayedGroan = false;
    }

    public void Die()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("ZombieDeath");
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.AddScore(scoreValue);
        }

        if (Random.value < powerUpChance)
        {
            DropPowerUp();
        }

        Destroy(gameObject);
    }

    void DropPowerUp()
    {
        GameObject powerUpPrefab = Random.value < 0.5f ? healthPowerUpPrefab : fireRatePowerUpPrefab;

        if (powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}

