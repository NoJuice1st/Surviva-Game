using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    public Animator animator;
    public NavMeshSurface surface;


    public float speed;
    public float viewDistance = 20f;
    public float wanderDistance = 10f;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        surface = GameObject.Find("NavMesh Surface").GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
        target = GameObject.Find("Player").transform;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void Update() 
    {
        //agent.destination = target.position;

        /*if (target == null) return;
        
        agent.speed = speed;

        if (agent.velocity != Vector3.zero)
        {
            animator.Play("Run");
        }
        else 
        {
            animator.Play("Idle");
        }
        */

        var distance = Vector3.Distance(transform.position, target.position);
        
        if (distance < viewDistance && target.gameObject.TryGetComponent<Player>(out Player plr))
        {
            if (plr.isSafe)
            {
                agent.destination += Random.insideUnitSphere * 5;
            }
            else
            {
                // chase
                agent.destination = target.position;
            }
        }
        else
        {
            // seek or stand
            if (agent.velocity == Vector3.zero)
            {
                if(Random.Range(1, 10) < 6)
                {
                    //seek
                    agent.destination += Random.insideUnitSphere * 5;
                }
            }
        }
    }
}
