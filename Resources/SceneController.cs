using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class SceneController : MonoBehaviour, IPlayerAction, ISceneController
{
    private static SceneController instance;
    private GuardFactory factory;
    private GameJudge judge;
    private List<Vector3> route1 = new List<Vector3>();
    private List<Vector3> route2 = new List<Vector3>();
    private List<Vector3> route3 = new List<Vector3>();
    private List<Vector3> route4 = new List<Vector3>();


    public static SceneController getInstance()
    {
        if (instance == null)
        {
            instance = (SceneController)FindObjectOfType(typeof(SceneController));
            if (instance == null)
            {
                Debug.LogError("An instance of " + typeof(SceneController)
                    + " is needed in the scene, but there is none.");
            }
        }
        return instance;
    }

    void OnEnable()
    {
        EventManager.Scoring += Scoring;
        EventManager.Gameover += Gameover;
    }
    void OnDisable()
    {
        EventManager.Scoring -= Scoring;
        EventManager.Gameover-= Gameover;
    }

    // Start is called before the first frame update
    void Start()
    {
        factory = GuardFactory.GetInstance();
        judge = GameJudge.GetInstance();

        route1.Add(new Vector3(-10, 0, 10));
        route1.Add(new Vector3(-10, 0, 13));
        route1.Add(new Vector3(-14, 0, 13));
        route1.Add(new Vector3(-14, 0, 10));

        route2.Add(new Vector3(-4, 0, -13));
        route2.Add(new Vector3(-4, 0, -6));
        route2.Add(new Vector3(-7, 0, -6));
        route2.Add(new Vector3(-7, 0, -13));

        route3.Add(new Vector3(14, 0, -13));
        route3.Add(new Vector3(14, 0, 0));
        route3.Add(new Vector3(11, 0, 0));
        route3.Add(new Vector3(11, 0, -13));

        route4.Add(new Vector3(11, 0, 10));
        route4.Add(new Vector3(11, 0, 13));
        route4.Add(new Vector3(-5, 0, 13));
        route4.Add(new Vector3(-5, 0, 10));

        loadResources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameStart()
    {
        
    }

    public int getScore()
    {
        return judge.getScore();
    }

    public bool getOver()
    {
        return judge.isOver;
    }

    public void loadResources()
    {
        factory.getGuard(route1[0],route1);
        factory.getGuard(route2[0], route2);
        factory.getGuard(route3[0], route3);
        factory.getGuard(route4[0], route4);
    }

    void Scoring()
    {
        if(getScore() >= 1000)
        {
            factory.guardStop();
        }
    }
    void Gameover()
    {
        factory.guardStop();
        // User UI
        judge.isOver = true;
    }
}
