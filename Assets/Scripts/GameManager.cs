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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateP1Score(int scoreToAdd)
    {
        p1Score += scoreToAdd;
        p1ScoreText.text = "Score: " + p1Score;
    }

    public void UpdateP2Score(int scoreToAdd)
    {
        p2Score += scoreToAdd;
        p2ScoreText.text = "Score: " + p2Score;
    }

}
