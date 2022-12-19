using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameJudge : MonoBehaviour
{
    private static GameJudge instance;

    public int score;
    public bool isOver;
    public static GameJudge GetInstance()
    {
        if (instance == null)
        {
            instance = (GameJudge)FindObjectOfType(typeof(GameJudge));
            if (instance == null)
            {
                Debug.LogError("An instance of " + typeof(GameJudge)
                    + " is needed in the scene, but there is none.");
            }
        }
        return instance;
    }

    void Start()
    {
        score = 0;
        isOver = false;
    }

    void OnEnable()
    {
        EventManager.Scoring += Scoring;
    }
    void OnDisable()
    {
        EventManager.Scoring -= Scoring;
    }

    public void Scoring()
    {
        score+=100;
        Debug.Log(score);
    }

    public int getScore()
    {
        return score;
    }
}
