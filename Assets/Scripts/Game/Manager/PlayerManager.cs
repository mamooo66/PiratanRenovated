using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void dragAreaInputUp()
    {
        Debug.Log("Basıldı Client");
        ShipMove(Camera.main.ScreenPointToRay(Input.mousePosition));
    }
    
    [Command]
    public void ShipMove(Ray ray)
    {
        Debug.Log("Basıldı Server");
        MoveCommands.MoveToPos(ray, agent);
    }
}
