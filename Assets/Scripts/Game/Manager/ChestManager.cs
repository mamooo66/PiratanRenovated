using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Mirror;
using UnityEngine.SceneManagement;

public class ChestManager : NetworkBehaviour
{
    public GameObject[] chests;
    private float timer = 0f;
    public float maxTime;
    public GameObject chestobject;
    public float mapSize;
    private void Awake()
    {
        chestobject = Resources.Load<GameObject>("chest");
        mapSize = CordinateCalculate.MapSize;
    }


    private void FixedUpdate()
    {
        if(isServer){
            if (timer <= 0)
            {
                for (int i = 0; i < chests.Length; i++)
                {
                    
                    if (chests[i] == null)
                    {
                        Vector3 randDirection = new Vector3(UnityEngine.Random.Range(-mapSize/2, mapSize/2), 0 , UnityEngine.Random.Range(-mapSize/2, mapSize/2));
                        NavMeshHit navHit;
                        NavMesh.SamplePosition(randDirection, out navHit, 65, -1);
                        chests[i] = Instantiate(chestobject, navHit.position+new Vector3 (0, .47f, 0), Quaternion.identity, transform);
                        NetworkServer.Spawn(chests[i]);
                    }

                }
                timer = maxTime;

            }
            timer -= Time.deltaTime;
        }
    }
}