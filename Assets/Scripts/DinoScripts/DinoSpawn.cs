using UnityEngine;
using Unity.Netcode;

public class ConditionalNetworkObjectSpawner : NetworkBehaviour
{
    public GameObject prefab; // Assign the prefab in the inspector

    void Update()
    {
        // Example condition: Player must press Space and have enough points
        if (Input.GetKeyDown(KeyCode.Space)) //CHANGE CONDITION
        {
            CmdSpawnObject();
        }
    }

    void CmdSpawnObject()
    {
        GameObject spawnedObject = Instantiate(prefab, transform.position, transform.rotation);
        spawnedObject.GetComponent<NetworkObject>().Spawn(); // Ensure the object is spawned on the network
    }
}
