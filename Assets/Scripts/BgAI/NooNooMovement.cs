using UnityEngine;
using UnityEngine.AI;


public class NooNooMovement : MonoBehaviour
{
    private NavMeshAgent aiAgent;

    private float patrolCountdown;
    private float patrolRadius = 100f;
    private float patrolLookCountdown;
    private bool isMoving = false;

    private Quaternion randomLookAxis = Quaternion.AngleAxis(90, Vector3.up);

    void Start()
    {
        aiAgent = this.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Patrol();
    }

    public void Patrol()
    {
        aiAgent.isStopped = false;
        float dist = aiAgent.remainingDistance;
        patrolCountdown -= Time.deltaTime;
        if (dist != Mathf.Infinity && aiAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            aiAgent.remainingDistance <= 3)
        {
            patrolCountdown -= Time.deltaTime;
            isMoving = false;
            if (patrolCountdown <= 0)
            {
                aiAgent.SetDestination(RandomNavmeshLocation(patrolRadius));
                isMoving = true;
                patrolCountdown = 5;
            }
        }
        RandomLook();
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

    private void RandomLook()
    {
        if (!isMoving)
        {
            patrolLookCountdown -= Time.deltaTime;
            if (patrolLookCountdown <= 0)
            {
                randomLookAxis = Quaternion.AngleAxis(Random.Range(-30f, 30f), Vector3.up);
                patrolLookCountdown = 5;
            }

            //transform.rotation = Quaternion.Slerp(transform.rotation, randomLookAxis, Time.deltaTime * 1);
            transform.rotation = Quaternion.LookRotation(aiAgent.velocity.normalized);
        }
    }
}