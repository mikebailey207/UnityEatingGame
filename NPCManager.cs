using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject npcPrefab; // Assign your NPC prefab here
    public List<Transform> spawnPoints; // Add spawn points in the inspector
    public int npcCount = 5; // Number of NPCs to spawn, adjustable in the inspector

    private List<GameObject> activeNPCs = new List<GameObject>();

    void Start()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        for (int i = 0; i < npcCount; i++)
        {
            // Pick a random spawn point
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Instantiate NPC and add it to the list
            GameObject npc = Instantiate(npcPrefab, randomSpawnPoint.position, Quaternion.identity);
            activeNPCs.Add(npc);
        }
    }
}