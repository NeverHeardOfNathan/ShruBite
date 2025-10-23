using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    public Transform gunMuzzle;
    public float currentFireRate;

    private float nextFireTime = 0f;

    [Header("Inscribed")]
    public float moveSpeed = 8f;
    public float boundaryX = 11f;
    public float boundaryZ = 11f;
    public int maxLives = 3;

    [Header("Dynamic")]
    public int currentLives;
    public bool isInvulnerable = false;
    public float invulnerabilityTime = 2f;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        currentLives = maxLives;
        currentFireRate = fireRate;
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        // Find gun muzzle automatically
        if (gunMuzzle == null)
        {
            Transform gunBarrel = transform.Find("GunBarrel");
            if (gunBarrel != null)
            {
                gunMuzzle = gunBarrel.Find("GunMuzzle");
            }

            if (gunMuzzle == null)
            {
                GameObject muzzle = new GameObject("GunMuzzle");
                muzzle.transform.parent = gunBarrel != null ? gunBarrel : transform;
                muzzle.transform.localPosition = new Vector3(0, 0.6f, 0);
                gunMuzzle = muzzle.transform;
            }
        }
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        movement = movement.normalized * moveSpeed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos += movement;

        pos.x = Mathf.Clamp(pos.x, -boundaryX, boundaryX);
        pos.z = Mathf.Clamp(pos.z, -boundaryZ, boundaryZ);

        transform.position = pos;

        AimAtMouse();

        // Automatic shooting
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + currentFireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;

        currentLives--;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("PlayerHurt");
        }

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;

        while (elapsed < invulnerabilityTime)
        {
            rend.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            rend.material.color = originalColor;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        isInvulnerable = false;
        rend.material.color = originalColor;
    }

    void GameOver()
    {
        // Stop music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.CancelInvoke();
            Debug.Log("Game Over! Final Score: " + gm.currentScore);
        }

        Time.timeScale = 0f;
    }

    void AimAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - transform.position.y;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mousePos);
        mouseWorld.y = transform.position.y;

        Vector3 lookDirection = mouseWorld - transform.position;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    void Shoot()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("Gunfire");
        }

        Vector3 spawnPos = gunMuzzle != null ? gunMuzzle.position : transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(transform.forward);
        }
    }

    public void IncreaseFireRate()
    {
        currentFireRate = fireRate * 0.5f;
        StartCoroutine(ResetFireRateCoroutine());
    }

    IEnumerator ResetFireRateCoroutine()
    {
        yield return new WaitForSeconds(10f);
        currentFireRate = fireRate;
    }
}
