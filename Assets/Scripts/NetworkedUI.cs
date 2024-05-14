using Mirror;
using UnityEngine;

public class NetworkedObjectSpawner : NetworkBehaviour
{
    public GameObject networkedObjectPrefab;

    [Server]
    private void SpawnNetworkedObject()
    {
        GameObject obj = Instantiate(networkedObjectPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(obj);
    }
}
