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
    
    public static void Rotate(Vector3 targetPos, Vector3 shipPos, Transform shipTransform){
        var dir = targetPos - shipPos;
        Quaternion LookAtRotation = Quaternion.LookRotation(dir);
        Quaternion LookAtRotationOnly_Y = Quaternion.Euler(0, LookAtRotation.eulerAngles.y + 180, 0);
        shipTransform.eulerAngles = LookAtRotationOnly_Y.eulerAngles;
    }
}
