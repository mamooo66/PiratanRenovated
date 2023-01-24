using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttakSystem : NetworkBehaviour
{
    private static AttakSystem instance;
    /*
    ! BİR GEMİNİN DİĞER GEMİYE SALDIRMASI İÇİN GEREKEN KOŞULLAR
    
    ? Geminin ateş edebileceği menzil sınırları içinde olmalı + 
    ? Tamir modunda olmaması gerekir + 
    ? Yeterli Cephaneye sahip olması gerekir (Mühimmat sistemi eklenince eklenecek) -
    ? Mühimmatın gemiye yüklü olması gerekmektedir +
    ? Hedef gemi veya saldıran geminin batıyor durumda olmaması gerekir +

    ! BİR SALDIRI NE GİBİ DURUMLARDA İPTAL OLUR
    
    ? Hedef gemi saldıran geminin menzili dışına çıkmış olabilir
    ? Saldıran gemi saldırısını iptal etmiş olabilir
    ? Mühimmat bitmiş olabilir (Mühimmat sistemi eklenince eklenecek)
    ? Hedef gemi veya saldıran geminin batmaya başlamış olabilir
    
    */

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Attack(PlayerManager attacker, NPCManager target){ // Düzenle
        if(AttakSystem.AttackControl(attacker,target))
        {
            attacker.reloadUI();
            attacker.isAttacking = true;
            float damageTime = Vector3.Distance(target.transform.position, attacker.transform.position) / 10;
            attacker.fullOfCannons = false;
            int damageAmount = instance.calculateDamage(attacker);
            instance.SpawnCannonBallRPC(attacker.transform,target.transform, attacker.shipData.cannonCount, damageTime, damageAmount);
            instance.StartCoroutine(damage(target, attacker,damageTime, damageAmount));
            instance.StartCoroutine(reloadingCannonball(attacker, target));
        }
        else if(attacker.fullOfCannons)
        {
            attacker.isAttacking = false;//değiş direkt cancelattack fonksiyonuyla hallet
            attacker.reloadedFull();//değişebilir belki mermi full değil
        }
    }
    

    public static void Attack(PlayerManager attacker, PlayerManager target){
        if(AttakSystem.AttackControl(attacker,target))
        {
            attacker.reloadUI();
            attacker.isAttacking = true;
            float damageTime = Vector3.Distance(target.transform.position, attacker.transform.position) / 10;
            attacker.fullOfCannons = false;
            int damageAmount = instance.calculateDamage(attacker);
            instance.SpawnCannonBallRPC(attacker.transform,target.transform, attacker.shipData.cannonCount, damageTime,damageAmount);
            instance.StartCoroutine(damage(target,attacker,damageTime, damageAmount));
            instance.StartCoroutine(reloadingCannonball(attacker, target));
        }
        else if(attacker.fullOfCannons)
        {
            attacker.isAttacking = false;//değiş direkt cancelattack fonksiyonuyla hallet
            attacker.reloadedFull();//değişebilir belki mermi full değil
        }
    }

    public static void Attack(NPCManager attacker, PlayerManager target) //Düzenle
    {
        if(AttakSystem.AttackControl(attacker,target)){
            float damageTime = Vector3.Distance(target.transform.position, attacker.transform.position) / 10;
            attacker.fullOfCannons = false;
            int damageAmount = instance.calculateDamage(attacker);
            instance.SpawnCannonBallRPC(attacker.transform,target.transform, attacker.shipData.cannonCount, damageTime, damageAmount);
            instance.StartCoroutine(damage(target,attacker,damageTime, damageAmount));
            instance.StartCoroutine(reloadingCannonball(attacker, target));
        }
        else
        {
            CancelAttack(attacker); //değiş
        }
    }
    
    public static void CancelAttack(PlayerManager ship){
        ship.isAttacking = false;
    }
    
    public static void CancelAttack(NPCManager ship)
    {
        ship.isAttacking = false;
    }

    public static bool AttackControl(PlayerManager attacker, NPCManager target){
        if (target != null &&
            Vector3.Distance(attacker.transform.position, target.transform.position) <= attacker.shipData.maxDistance && 
            attacker.isRepairing == false &&
            attacker.fullOfCannons == true &&
            target.health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public static bool AttackControl(PlayerManager attacker, PlayerManager target){
        if (Vector3.Distance(attacker.transform.position, target.transform.position) <= attacker.shipData.maxDistance && 
            attacker.isRepairing == false &&
            attacker.fullOfCannons == true &&
            target.playerData.health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public static bool AttackControl(NPCManager attacker, PlayerManager target)
    {
        if (Vector3.Distance(attacker.transform.position, target.transform.position) <= attacker.shipData.maxDistance && 
            attacker.fullOfCannons == true &&
            target.playerData.health > 0 &&
            attacker.isAttacking == true) 
        {
            return true;
            
        }
        else
        {
            return false;
        }
    }
    public static IEnumerator damage(NPCManager targetShip, PlayerManager attackerShip, float countdown, int damage)
    {
        yield return new WaitForSeconds(countdown);
        if (targetShip.health - damage <= 0)
        {
            attackerShip.attackCMD(null, TargetType.player);
            targetShip.takeDamage(damage, attackerShip);
        }
        else
        {
            targetShip.takeDamage(damage, attackerShip);
            attackerShip.uiManager.targetPanelSet(targetShip.gameObject, targetShip.name, targetShip.health + "/" + targetShip.shipData.maxHealth, (float)targetShip.health/(float)targetShip.shipData.maxHealth);
        }
    }
    
    public static IEnumerator damage(PlayerManager targetShip, PlayerManager attackerShip ,float countdown, int damage)
    {
        yield return new WaitForSeconds(countdown);
        if (targetShip.playerData.health - damage <= 0)
        {
            attackerShip.attackCMD(null, TargetType.player);
            targetShip.takeDamage(damage);
        }
        else
        {
            targetShip.takeDamage(damage);
            targetShip.uiManager.AttackerPanelAdd(attackerShip.gameObject, attackerShip.name, attackerShip.playerData.health + "/" + attackerShip.shipData.maxHealth, (float)attackerShip.playerData.health/(float)attackerShip.shipData.maxHealth);
            attackerShip.uiManager.targetPanelSet(targetShip.gameObject, targetShip.name, targetShip.playerData.health + "/" + targetShip.shipData.maxHealth, (float)targetShip.playerData.health/(float)targetShip.shipData.maxHealth);
        }
    }
    
    public static IEnumerator damage(PlayerManager targetShip, NPCManager attackerShip, float countdown, int damage)
    {
        yield return new WaitForSeconds(countdown);
        if (targetShip.playerData.health - damage <= 0)
        {
            AttakSystem.CancelAttack(attackerShip);
            targetShip.takeDamage(damage);
        }
        else
        {
            targetShip.takeDamage(damage);
            targetShip.uiManager.AttackerPanelAdd(attackerShip.gameObject, attackerShip.name, attackerShip.health + "/" + attackerShip.shipData.maxHealth, (float)attackerShip.health/(float)attackerShip.shipData.maxHealth);
        }
    }
    

    [ClientRpc] // Server only de çalıştırmayı dene
    public void SpawnCannonBallRPC(Transform spawnPoint, Transform target, int cannonCount, float _damageTime, int _damageAmount)
    {
        StartCoroutine(ISpawnCannonBall(spawnPoint, target, cannonCount, _damageTime, _damageAmount));
    }

    public IEnumerator ISpawnCannonBall(Transform spawnPoint, Transform target, int cannonCount, float _damageTime, int _damageAmount){
        for (int i = 0; i < cannonCount; i++)
        {
            var a = Instantiate(Resources.Load<GameObject>("CannonballData/CannonballsPrefab/" + 0/*Cannonball sistemi eklenince id ye göre yapacaksın*/), spawnPoint.position + new Vector3(UnityEngine.Random.Range(1.2f, -1.2f), 1 , UnityEngine.Random.Range(5f, -5f)), Quaternion.identity, target.transform);
            a.GetComponent<CannonballFollow>().SetBall(target.transform);
            yield return new WaitForSeconds(0.06f);
        }

        yield return new WaitForSeconds(_damageTime - 0.06f * cannonCount);
        var b = Instantiate(Resources.Load<GameObject>("Explosion"), target.position, Quaternion.identity, target.transform);
        Destroy(b.gameObject, 1);
        
        var c = Instantiate(Resources.Load<GameObject>("FloatingText"), target.position, Quaternion.identity, target);
        c.GetComponent<TextMesh>().text = "-" + _damageAmount;
        c.GetComponent<TextMesh>().color = Color.red;
    }

    public static IEnumerator reloadingCannonball(PlayerManager attacker, NPCManager target){
        yield return new WaitForSeconds(attacker.shipData.attackCooldown); //Cooldown ismini değiş
        attacker.fullOfCannons = true;

        if (attacker.isAttacking) //Düzenle buraları
        {
            Attack(attacker,target);//direkt çağır
        }
        else
        {
            attacker.reloadedFull();
        }
    }
    
    public static IEnumerator reloadingCannonball(PlayerManager attacker, PlayerManager target){
        yield return new WaitForSeconds(attacker.shipData.attackCooldown);
        attacker.fullOfCannons = true;
        if (attacker.isAttacking) //Düzenle buraları
        {
            Attack(attacker,target);//direkt çağır
        }
        else
        {
            attacker.reloadedFull();
        }
    }

    public static IEnumerator reloadingCannonball(NPCManager attacker, PlayerManager target){
        yield return new WaitForSeconds(attacker.shipData.attackCooldown);
        attacker.fullOfCannons = true;
        Attack(attacker, target);
    }

    public int calculateDamage(NPCManager ship)
    {
        int damage = 0;
        for (int i = 0; i < ship.shipData.cannonCount; i++)
        {
            int possibility = Random.Range(0,100);
            if(possibility <= ship.shipData.hitRate){
                damage += 150;
            }
        }
        return damage;
    }
    
    public int calculateDamage(PlayerManager ship)
    {
        int damage = 0;
        for (int i = 0; i < ship.shipData.cannonCount; i++)
        {
            int possibility = Random.Range(0,100);
            if(possibility <= ship.shipData.hitRate){
                damage += 150;
            }
        }
        return damage;
    }
}
