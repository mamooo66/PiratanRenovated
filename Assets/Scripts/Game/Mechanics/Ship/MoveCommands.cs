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
     
    public static void MoveToPos(Vector3 movePos, NavMeshAgent agent) //
    {
        agent.SetDestination(movePos);
    }
    
    public static void Stop(NavMeshAgent agent){
        agent.ResetPath();
    }
    
    public static void MoveToRandomPos(NavMeshAgent agent){
        Vector3 x = new Vector3(UnityEngine.Random.Range(-500, 500), 0 , UnityEngine.Random.Range(-500, 500));
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(x, out navMeshHit, 65, -1);
        agent.SetDestination(navMeshHit.position);
    }

    public static void Rotate(Vector3 targetPos, Transform shipTransform){
        var dir = targetPos - shipTransform.position;
        Quaternion LookAtRotation = Quaternion.LookRotation(dir);
        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(0, LookAtRotation.eulerAngles.y + 180, 0);
        shipTransform.eulerAngles = LookAtRotationOnly_Y.eulerAngles;
    }
}
