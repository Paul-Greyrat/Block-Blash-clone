using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreTex;
    private int currentScore;

    void Start()
    {
        currentScore = 0;
        GetScoretext();
    }
    private void OnEnable()
    {
        GameEvents.AddScores += AddScore;
    }
    private void OnDisable()
    {
        GameEvents.AddScores -= AddScore;
    }

    private void AddScore(int Score)
    {
        currentScore += Score;
        GetScoretext();
    }

    private void GetScoretext()
    {
        scoreTex.text = currentScore.ToString();
    }
}
