using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class MoveCommands : NetworkBehaviour
{
    // !HAREKET MEKANİKLERİ
    
    // Rastgele bir noktaya hareket etme(NPC ler için)
    // Seçilen bir noktaya hareket etme
    // Hareket ederken rotasyonu değiştirme
    
    public static void MoveToPos(Ray mousePos, NavMeshAgent agent){
        RaycastHit hit;
        NavMeshHit navMeshHit;
        if(Physics.Raycast(mousePos,out hit,  Mathf.Infinity)){
            NavMesh.SamplePosition(hit.point, out navMeshHit, 65, -1);
            agent.SetDestination(navMeshHit.position);
            Debug.Log(agent.destination);
        }
    }

    public static Vector3 Rotate(Vector3 movePos, Vector3 pos){
        var dir = movePos - pos;
        Quaternion LookAtRotation = Quaternion.LookRotation(dir);
        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(0, LookAtRotation.eulerAngles.y + 180, 0);
        return LookAtRotationOnly_Y.eulerAngles;
    }

    public static void Stop(NavMeshAgent agent){
        agent.SetDestination(agent.transform.position);
    }
}
