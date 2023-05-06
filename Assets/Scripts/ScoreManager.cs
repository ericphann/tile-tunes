using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Board board;
    public Text scoreText;
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString("N0") + "\nMultiplier " + board.multiplier + "x";
    }

    public void IncreaseScore(int amountToIncrease) {
        score += amountToIncrease;
    }
}
