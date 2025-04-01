using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int p1Score;
    private int p2Score;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public TextMeshProUGUI p1WinText;
    public TextMeshProUGUI p2WinText;
    public bool isGameActive;
    public Button restartButton;
    public GameObject titleScreen;
    private Spawner spawner;
   

    // Start is called before the first frame update
    void Start()
    {
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (p1Score == 100)
        {
            GameOver1();
        }
        else if (p2Score == 100) 
        {
            GameOver2();
        }
    }

    public void UpdateP1Score(int scoreToAdd)
    {
        if (isGameActive)
        {
            p1Score += scoreToAdd;
            p1ScoreText.text = "Score: " + p1Score;
        }
    }

    public void UpdateP2Score(int scoreToAdd)
    {
        if (isGameActive)
        {
            p2Score += scoreToAdd;
            p2ScoreText.text = "Score: " + p2Score;
        }
    }

    public void GameOver1()
    {
        p1WinText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true );
    }
    public void GameOver2()
    {
        p2WinText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isGameActive = true;
        p1Score = 0;
        p2Score = 0;
        
        titleScreen.gameObject.SetActive(false);
        spawner.SpawnRandom();
    }
}
