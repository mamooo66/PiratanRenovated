using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttakSystem : MonoBehaviour
{
    /*
    ! BİR GEMİNİN DİĞER GEMİYE SALDIRMASI İÇİN GEREKEN KOŞULLAR
    
    ? Geminin ateş edebileceği menzil sınırları içinde olmalı
    ? Tamir modunda olmaması gerekir
    ? Yeterli Cephaneye sahip olması gerekir (Mühimmat sistemi eklenince eklenecek)
    ? Mühimmatın gemiye yüklü olması gerekmektedir
    ? Hedef gemi veya saldıran geminin batıyor durumda olmaması gerekir

    ! BİR SALDIRI NE GİBİ DURUMLARDA İPTAL OLUR
    
    ? Hedef gemi saldıran geminin menzili dışına çıkmış olabilir
    ? Saldıran gemi saldırısını iptal etmiş olabilir
    ? Mühimmat bitmiş olabilir (Mühimmat sistemi eklenince eklenecek)
    ? Hedef gemi veya saldıran geminin batmaya başlamış olabilir
    
    */
    public static void Attack(PlayerManager attacker, NPCManager target){

    }

    public static void Attack(PlayerManager attacker, PlayerManager target){
        
    }

    public static void Attack(NPCManager attacker, PlayerManager target){
        
    }

    public void AttackControl(){

    }

    public IEnumerator spawnCannonBall(Transform spawnPoint, Transform targetPoint, int cannonCount){
        yield return null;
    }

    public IEnumerator reloadingCannonball(PlayerManager ship){
        yield return null;
    }

    public IEnumerator reloadingCannonball(NPCManager ship){
        yield return null;
    }
}
