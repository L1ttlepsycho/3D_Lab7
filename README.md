# 3D_Lab7
# Patrolman 戏耍巡逻小游戏

*演示视屏链接：*

## 要求
**游戏设计要求：**

1. 创建一个地图和若干巡逻兵(使用动画)；
2. 每个巡逻兵走一个3~5个边的凸多边型，位置数据是相对地址。即每次确定下一个目标位置，用自己当前位置为原点计算；
3. 巡逻兵碰撞到障碍物，则会自动选下一个点为目标；
4. 巡逻兵在设定范围内感知到玩家，会自动追击玩家；
5. 失去玩家目标后，继续巡逻；

**计分：**

玩家每次甩掉一个巡逻兵计100分，达到1000分游戏胜利，与巡逻兵碰撞游戏结束；
程序设计要求：

**必须使用订阅与发布模式传消息**

**工厂模式生产巡逻兵**

## 关键实现

### Guards
守卫采用了Standard Asset中的Third Person AI，添加了一个sphere collision box作为trigger检测玩家是否进入了侦察范围，仿造原有的脚本自己实现了一个```GuardBehaviour```AI脚本。

使用了Unity的Navi Mesh，实现了AI的巡逻和追击路线规划，每个巡逻兵的路线为不同大小的矩形。

```
public class GuardBehaviour : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
    public ThirdPersonCharacter character { get; private set; } // the character we are controlling
    public Transform target;                                    // target to aim for
    public List<Vector3> patrol_route=new List<Vector3>(4);
    private bool on_chase;
    private int pos_index=0;

    private void Start()
    {
        // get the components on the object we need ( should not be null due to require component so no need to check )
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        
        agent.updateRotation = false;
        agent.updatePosition = true;
        on_chase = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == "Player")
        {
            Debug.Log("caught! ");
            EventManager.GetInstance().PlayerGameover();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            Debug.Log("chasing... ");
            target= other.transform;
            on_chase = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "Player")
        {
            Debug.Log("losing target...");
            target = null;
            on_chase = false;
            EventManager.GetInstance().PlayerEscape();
        }
    }

    private void Update()
    {
        if (on_chase)
        {
            if (target != null)
                agent.SetDestination(target.position);
        }
        else
        {
            if(agent.remainingDistance == agent.stoppingDistance)
            {
                pos_index=(pos_index+1)%4;
                agent.SetDestination(patrol_route[pos_index]);
            }

        }
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
            character.Move(Vector3.zero, false, false);
    }

    public void SetRoute(List<Vector3> route)
    {
        for (int i = 0; i < 4; i++)
        {
            patrol_route[i] = route[i];
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
```

```GuardFactory```实现了Guards的生成和统一控制。
```
public class GuardFactory : MonoBehaviour
{
    private static GuardFactory instance;
    private Queue<GameObject> guards = new Queue<GameObject>();
    public static GuardFactory GetInstance()
    {
        if(instance == null)
        {
            instance = (GuardFactory)FindObjectOfType(typeof(GuardFactory));
            if(instance == null)
            {
                Debug.LogError("Error: cannot find instance of " + typeof(GuardFactory));
            }
        }
        return instance;
    }

    public GameObject getGuard(Vector3 pos,List<Vector3> targets)
    {
        GameObject guard =Instantiate<GameObject> (Resources.Load<GameObject>("Guard"));
        GuardBehaviour beh = guard.GetComponent<GuardBehaviour>();
        guard.transform.position = pos;
        beh.SetRoute(targets);
        guards.Enqueue(guard);
        return guard;
    }

    public void guardStop()
    {
        while(guards.Count > 0)
        {
            GuardBehaviour guard=guards.Dequeue().GetComponent<GuardBehaviour>();
            guard.agent.isStopped = true;
        }
    }
}
```

### 玩家实现
玩家也采用了Standard Asset中的Third Person Player，使用了一个跟随玩家的FreeCamera作为第三人称视角的实现。

### 订阅模式实现
订阅发布者
```
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
    //玩家被捕
    public void PlayerGameover()
    {
        if (Gameover != null)
        {
            Gameover();
        }
    }
}
```
订阅者有```SceneController```和```GameJudge```，相关代码
```
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
```

```
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
```

完整代码见Resources目录，资源见代码仓库。
