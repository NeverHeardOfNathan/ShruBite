using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;

    private GameManager gameManager;
    private Player player;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
        if (gameManager != null)
        {
            scoreText.text = "Score: " + gameManager.currentScore.ToString("#,0");
        }

        if (player != null)
        {
            livesText.text = "Lives: " + player.currentLives;
        }
    }
}
