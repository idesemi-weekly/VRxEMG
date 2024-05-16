using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class NetworkSpawn : NetworkBehaviour
{
    public GameObject objectToSpawnPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            base.OnNetworkSpawn();
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(objectToSpawnPrefab, transform.position, transform.rotation);
        spawnedObject.GetComponent<NetworkObject>().Spawn(); // Ensure the object is spawned on the network
    }

    // Update is called once per frame
    void Update()
    {
        objectToSpawnPrefab.transform.position = transform.position;
        objectToSpawnPrefab.transform.rotation = transform.rotation;
    }
}
