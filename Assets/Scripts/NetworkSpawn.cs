using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class NetworkSpawn : NetworkBehaviour
{
    public GameObject[] objectToSpawnPrefab;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)//to remove (no?)
        {
            foreach(var obj in objectToSpawnPrefab)
            {
                SpawnObject(obj);
            }
        }
    }

    void SpawnObject(GameObject obj)
    {
        GameObject spawnedObject = Instantiate(obj, transform.position, transform.rotation);
        spawnedObjects.Add(spawnedObject); // Add spawned object to the list
        spawnedObject.GetComponent<NetworkObject>().Spawn(); // Ensure the object is spawned on the network
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var spawnedObject in spawnedObjects)
        {
            if (spawnedObject != null) // Check if the spawned object still exists
            {
                spawnedObject.transform.position = transform.position;
                spawnedObject.transform.rotation = transform.rotation;
            }
        }
    }
}
