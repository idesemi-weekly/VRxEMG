using UnityEngine;
using Unity.Netcode;

public class Spawner1 : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    public SpawnableObject[] spawnableObjects;

    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;

    private void OnEnable()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer) // Only the server spawns objects
        {
            Invoke(nameof(SpawnObject), Random.Range(minSpawnRate, maxSpawnRate));
        }
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            CancelInvoke();
        }
    }

    private void SpawnObject()
    {
        float randomValue = Random.value;

        foreach (var obj in spawnableObjects)
        {
            if (randomValue <= obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);
                obstacle.transform.position += transform.position;

                NetworkObject networkObject = obstacle.GetComponent<NetworkObject>();
                networkObject.Spawn();
                break;
            }
            else
            {
                randomValue -= obj.spawnChance;
            }
        }

        Invoke(nameof(SpawnObject), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
