using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int rows;
    public int level = 1;

    public int rowsInTheLevel = 5;

    public int minRow = 1;
    public int maxRow = 4;

    public TextMeshProUGUI rowTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI scoreTxt;

    public bool isNextLevel;

    private void Start()
    {
        ResetFNC();
    }

    public void ResetFNC()
    {
        level = 1;
        rows = rowsInTheLevel * level;
        TextUpdateFNC();
    }
    
    public void RowScore(int n)
    {
        isNextLevel = false;
        n = Mathf.Clamp(n, minRow, maxRow);
        switch (n)
        {
            case 1:
                score += 30 * level;
                break;
            case 2:
                score += 50 * level;
                break;
            case 3:
                score += 150 * level;
                break;
            case 4:
                score += 500 * level;
                break;
        }

        rows -= n;

        if (rows <= 0)
        {
            nextLevelFNC();
        }
        
        TextUpdateFNC();
    }

    void TextUpdateFNC()
    {
        if (scoreTxt)
        {
            scoreTxt.text = AddZeroAtStartFNC(score,5);
        }
        if (levelTxt)
        {
            levelTxt.text = level.ToString();
        }
        if (rowTxt)
        {
            rowTxt.text = rows.ToString();
        }
    }

    string AddZeroAtStartFNC(int score, int number)
    {
        string scoreStr = score.ToString();

        while (scoreStr.Length < number)
        {
            scoreStr = "0" + scoreStr;
        }
        return scoreStr;
    }

    public void nextLevelFNC()
    {
        level++;
        rows = rowsInTheLevel * level;
        isNextLevel = true;
    }
}
