using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
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
            Debug.Log("cached! ");
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
