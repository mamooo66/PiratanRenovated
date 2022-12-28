using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using UnityEngine.PlayerLoop;

public class PlayerManager : NetworkBehaviour
{
    private Vector3 difference;
    private Vector3 dragOrigin;
    private bool dragPressed;
    private bool dragEnough;
    
    [Header("Movement")]
    [SerializeField] private NavMeshAgent agent;
    RaycastHit hit;
    public Joystick moveJoystick;
    public Transform ship;
    public bool pressMoveJoystick;
    
    [Header("Camera")]
    public Joystick cameraJoystick;
    public bool pressCameraJoystick;
    public bool focusCamera;

    private void Start()
    {
        if (isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (pressMoveJoystick)//Move Joysticke basılı tutulduğu sürece hareket fonksiyonuna veri gönderir
            {
                Vector3 direction = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);
                shipMoveCMD(direction + transform.position);
            }
        }
    }

    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            if (pressCameraJoystick)
            {
                float cameraSizeRatio = Camera.main.orthographicSize / 25f;
                CameraCommands.CameraMove(new Vector3(cameraJoystick.Horizontal * 0.15f * cameraSizeRatio, 0, cameraJoystick.Vertical * 0.15f * cameraSizeRatio));
            }
            else if(focusCamera)
            {
                CameraCommands.CameraMoveToPlayer(transform);
            }
        }
    }

    public void dragAreaInputUp() 
    {
        NavMeshHit navMeshHit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition) ,out hit,  Mathf.Infinity)){
            NavMesh.SamplePosition(hit.point, out navMeshHit, 65, -1);
            shipMoveCMD(navMeshHit.position);
        }
    }
    
    [Command]
    public void shipMoveCMD(Vector3 targetPos) //Harita üzerinde kaydırma yapılmadan tıklanılan yere gidilmesi içindir. Bu fonsksiyon server üzerinde çalışır. MoveCommands'a hareket isteği gönderilir.
    {
        MoveCommands.MoveToPos(targetPos, agent);
        RotateShip(targetPos);
    }
    
    [ClientRpc]
    public void RotateShip(Vector3 targetPos) //Gemi gittiği yöne doğru yönelir
    {
        MoveCommands.Rotate(targetPos, transform.position, ship);
    }

    [Command]
    public void moveJoystickDownCMD() //Joysticke basıldığında hali hazırda gidilen path sıfırlanır
    {
        MoveCommands.Stop(agent);
    }
    
    public void moveJoystickDown()
    {
        pressMoveJoystick = true;
    }
    
    public void moveJoystickUp()
    {
        pressMoveJoystick = false;
    }
    
    public void cameraJoystickDown()
    {
        focusCamera = false;
        pressCameraJoystick = true;
    } 
    
    public void cameraJoystickUp()
    {
        pressCameraJoystick = false;
    }
    
    public void focusShipButton()
    {
        focusCamera = true;
    }
}
