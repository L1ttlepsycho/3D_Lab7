using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public delegate void ScoreEvent();
    public static event ScoreEvent Scoring;

    public delegate void GameoverEvent();
    public static event GameoverEvent Gameover;

    public static EventManager GetInstance()
    {
        if (instance == null)
        {
            instance = (EventManager)FindObjectOfType(typeof(EventManager));
            if (instance == null)
            {
                Debug.LogError("An instance of " + typeof(EventManager)
                    + " is needed in the scene, but there is none.");
            }
        }
        return instance;
    }
    public void PlayerEscape()
    {
        if (Scoring != null)
        {
            Scoring();
        }
    }
    //Íæ¼Ò±»²¶
    public void PlayerGameover()
    {
        if (Gameover != null)
        {
            Gameover();
        }
    }
}
