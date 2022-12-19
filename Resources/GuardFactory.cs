using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

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
