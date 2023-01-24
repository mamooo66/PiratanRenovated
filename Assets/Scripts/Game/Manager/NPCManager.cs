using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class NPCManager : NetworkBehaviour
{

    public GameObject bottomHealtBar;
    
    Coroutine stopCoroutine;
    [Header("Ship Settings")] 
    public ShipData shipData;

    public int health;

    [Header("Attack System")]
    public bool isAttacking = false;
    public  bool underAttack = false;
    public bool fullOfCannons;
    
    [Header("Movement")]
    public NavMeshAgent agent;
    public Transform ship;

    public PlayerManager target;

    private void Start()
    {
        if (isServer)
        {
            shipData = new ShipData();
            shipData.cannonCount = 6;
            shipData.attackCooldown = 6;
            shipData.maxDistance = 40;
            shipData.speed = 14;
            shipData.hitRate = 60;
            shipData.maxHealth = 8000;
            
            
        }
    }

    void Update()
    {
        if (isServer)
        {
            if (agent.velocity == Vector3.zero && !isAttacking && !underAttack)
            {
                MoveCommands.MoveToRandomPos(agent);
                setRotation(agent.destination);
            }
        
            if(isAttacking && Vector3.Distance(transform.position, target.transform.position) > shipData.maxDistance)
            {
                AttakSystem.CancelAttack(this); //Düzenle
            }
        }
    }
    
    public void takeDamage(int damage, PlayerManager enemy)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        if(stopCoroutine != null)
            StopCoroutine(stopCoroutine);
        stopCoroutine = StartCoroutine(IStop());
        target = enemy;
        if (!isAttacking)
        {
            isAttacking = true;
            AttakSystem.Attack(this, target);
        }

        setHealthBar((float)health / (float)shipData.maxHealth);
    }

    [ClientRpc]
    public void setHealthBar(float ratio)
    {
        bottomHealtBar.transform.localScale = new Vector3(ratio,1,1);
    }

    [ClientRpc]
    public void setRotation(Vector3 targetPos)
    {
        MoveCommands.Rotate(targetPos, ship);
    }
    

    public IEnumerator IStop()
    {
        underAttack = true;
        MoveCommands.Stop(agent);
        yield return new WaitForSeconds(10);
        underAttack = false;
    }
}
