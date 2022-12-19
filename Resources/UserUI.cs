using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class UserUI : MonoBehaviour
{
    private IPlayerAction controller;

    private GUIStyle score_style = new GUIStyle();
    private GUIStyle text_style = new GUIStyle();
    private GUIStyle over_style = new GUIStyle();
    private int displayTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        controller = SceneController.getInstance();

        text_style.normal.textColor = Color.blue;
        text_style.fontSize = 42;
        score_style.normal.textColor = Color.red;
        score_style.fontSize = 42;
        over_style.fontSize = 80;

        StartCoroutine(ShowTip());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 5, 200, 50), "分数:", text_style);
        GUI.Label(new Rect(130, 10, 200, 50), controller.getScore().ToString(), score_style);


        if (controller.getScore()>=1000)
        {
            GUI.Label(new Rect(Screen.width / 2 - 160, 180, 100, 100), "Victory！", over_style);
        }
        else if (controller.getOver())
        {
            GUI.Label(new Rect(Screen.width / 2 - 160, 180, 100, 100), "Game Over！", over_style);
        }
    
        if (displayTime > 0)
        {
            // Debug.Log("GUI show");
            GUI.Label(new Rect(Screen.width / 2 - 160, 180, 100, 100), "按WSAD或方向键移动", text_style);
            GUI.Label(new Rect(Screen.width / 2 - 160, 260, 100, 100), "成功躲避巡逻兵追捕加100分", text_style);
            GUI.Label(new Rect(Screen.width / 2 - 160, 340, 100, 100), "获得1000分以斩获胜利", text_style);
        }

    }


    public IEnumerator ShowTip()
    {
        while (displayTime >= 0)
        {
            yield return new WaitForSeconds(1);
            displayTime--;
        }
    }
}
