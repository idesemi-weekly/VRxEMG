using UnityEngine;
using Unity.Netcode;

public class Ready : NetworkBehaviour
{
    //might need to change these to public
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool p1ready = false;
    [SerializeField] private bool p2ready = false;

    void Update()
    {
        if(p1ready){
            Debug.Log("Player 1 is ready");//Replace with a checkmark in game !
        }

        if(p2ready){
            Debug.Log("Player 2 is ready");//Replace with a checkmark in game !
        }

        if (p1ready && p2ready)
        {
            CmdSpawnObject();
        }
    }

    void CmdSpawnObject()
    {
        GameObject spawnedObject = Instantiate(prefab, transform.position, transform.rotation);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }
}
