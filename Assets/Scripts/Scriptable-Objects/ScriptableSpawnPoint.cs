using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spawn Point", menuName = "ScriptableObjects/SpawnPoint")]
public class ScriptableSpawnPoint : ScriptableObject
{
    public GameObject ReferencePoint;
    public Transform SpawnPoint;
    private Transform originalTransform;
    public List<Transform> PatrolPoints = new List<Transform>();

    public Transform CalculateSpawnPoint()
    {
        Vector3 calculated = SpawnPoint.position + ReferencePoint.transform.position;
        SpawnPoint.position = calculated;
        return SpawnPoint;
    }

    public List<Transform> CalculatePatrolPoints()
    {
        List<Transform> calcPatrolPoints = new List<Transform>();
        foreach (Transform originalTransform in PatrolPoints)
        {
            originalTransform.position += ReferencePoint.transform.position;
            calcPatrolPoints.Add(originalTransform);
        }
        return calcPatrolPoints;
    }

    public Transform GetOriginalTransform()
    {
        return originalTransform;
    }
}
