using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerManager : NetworkBehaviour
{
    private Vector3 difference;
    private Vector3 dragOrigin;
    private bool dragEnough;
    
    [Header("Player Settings")]
    public ShipData shipData;
    public PlayerData playerData;
    public bool isRepairing;
    public GameObject bottomHealtBar;
    public Coroutine repairCoroutine;
    
    [Header("Attack System")]
    public bool isAttacking;
    public bool fullOfCannons;
    public GameObject target;
    public TargetType targetType;
    
    [Header("Movement")]
    public NavMeshAgent agent;
    RaycastHit hit;
    public Joystick moveJoystick;
    public Transform ship;
    public bool pressMoveJoystick;
    
    [Header("Camera")]
    public Joystick cameraJoystick;
    public bool pressCameraJoystick;
    public bool focusCamera;
    
    public UIManager uiManager;

    private void Start()
    {
        if (isLocalPlayer)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }

        if (isServer)
        {
            playerData = new PlayerData();
            shipData = new ShipData();
            shipData.cannonCount = 12;
            shipData.attackCooldown = 6;
            shipData.maxDistance = 40;
            shipData.speed = 20;
            shipData.hitRate = 80;
            shipData.maxHealth = 20000;
            playerData.health = 20000;
            playerData.Gold = 0;
            playerData.Experience = 0;
            playerData.requiredExperience = 1000;
            playerData.Pearl = 0;
            playerData.shipNo = 1;
            playerData.level = 1;
            playerData.cannonballData = "";
        
            agent.speed = shipData.speed;

            //setUISettings(5, shipData.attackCooldown);
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

    public void dragAreaInputDown() 
    {
        dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void dragAreaInputDrag()
    {
        CalculateDifference();
        if (dragEnough == false)
        {
            if (CalculateDifferenceAmount())
            {
                SetOrigin();
            }
        }
        else
        {
            cameraMoveWithDrag();
        }
    }
    
    public void dragAreaInputUp() 
    {
        if (dragEnough)
        {
            dragEnough = false;
        }
        else
        {
            NavMeshHit navMeshHit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition) ,out hit,  Mathf.Infinity)){
                switch (hit.transform.tag)
                {
                    case "Player":
                        selectTarget(hit.transform.gameObject, TargetType.player);
                        break;
                    case "NPC":
                        selectTarget(hit.transform.gameObject, TargetType.NPC);
                        break;
                    default:
                        NavMesh.SamplePosition(hit.point, out navMeshHit, 65, -1);
                        shipMoveCMD(navMeshHit.position);
                        break;
                }
            }
        }
    }
    
    private void CalculateDifference(){
        difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool CalculateDifferenceAmount(){
        float a = Mathf.Abs(difference.x) + Mathf.Abs(difference.y) + Mathf.Abs(difference.z);
        if(a > 1.5f){
            return true;
        }
        else{
            return false;
        }
    }
    private void SetOrigin(){
        dragEnough = true;
        //focusButton.SetActive(true);
        focusCamera = false;
        dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        difference = dragOrigin - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    [Command]
    public void shipMoveCMD(Vector3 targetPos) //Harita üzerinde kaydırma yapılmadan tıklanılan yere gidilmesi içindir. Bu fonsksiyon server üzerinde çalışır. MoveCommands'a hareket isteği gönderilir.
    {
        if (isRepairing)
        {
            uiManager.OnWriteScreenText("You cannot move when repairing.");
            pressMoveJoystick = false;
            return;
        }
        MoveCommands.MoveToPos(targetPos, agent);
        RotateShip(targetPos);
    }
    
    [ClientRpc]
    public void RotateShip(Vector3 targetPos) //Gemi gittiği yöne doğru yönelir
    {
        MoveCommands.Rotate(targetPos, ship);
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
        uiManager.focusButtonShow();
        pressCameraJoystick = true;
    } 
    
    public void cameraJoystickUp()
    {
        pressCameraJoystick = false;
    }

    public void cameraMoveWithDrag()
    {
        Camera.main.transform.position += new Vector3(difference.x, 0, difference.z);
        uiManager.focusButtonShow();

    }
    
    public void focusShipButton()
    {
        focusCamera = true;
        
    }

    [TargetRpc]
    public void setUISettings(float repairTime, float reloadTime)
    {
        uiManager.setShipSettings(repairTime, reloadTime);
    }

    public void attackButton()
    {
        Debug.Log("tss");
        attackCMD(target, targetType);
    }
    
    [Command(requiresAuthority = false)]
    public void attackCMD(GameObject _target, TargetType _targetType)
    {
        Debug.Log(345);
        if (isAttacking)// Düzenle bunları
        {
            cancelAttackUI();
            AttakSystem.CancelAttack(this);
        }
        else
        {
            Debug.Log(567);
            if (_target != null)// Düzenle bunları
            {
                Debug.Log(980);
                switch (_targetType)
                {
                    case TargetType.player:
                        AttakSystem.Attack(this,_target.GetComponent<PlayerManager>());
                        break;
                    case TargetType.NPC:
                        AttakSystem.Attack(this,_target.GetComponent<NPCManager>());
                        break;
                }
            }
        }
    }
    
    
    [TargetRpc]
    public void cancelAttackUI()// Düzenle bunları
    {
        attackButtonUI(1);
    }

    [Command]
    public void cancelAttackCMD()// Düzenle bunları
    {
        cancelAttackUI();
        AttakSystem.CancelAttack(this);
    }
    
    public void attackButtonUI(int spriteNo)
    {
        uiManager.setAttackSprite(spriteNo);
    }
    
    [Command]
    public void repairCMD()
    {
        if (isRepairing)
        {
            Debug.Log(2);
            RepairSystem.stopRepairing(this);
            uiManager.stopRepair();
            uiManager.setRepairSprite(0);
        }
        else
        {
            Debug.Log(1);
            RepairSystem.startRepairing(this);
        }
    }

    [TargetRpc]
    public void reloadedFull()
    {
        attackButtonUI(0);
        uiManager.stopReload();
    }
    
    [TargetRpc]
    public void reloadUI()
    {
        uiManager.startReload();
        uiManager.setAttackSprite(2);
    }
    

    public void selectTarget(GameObject selectedShip, TargetType _targetType)
    {
        if (selectedShip.gameObject == gameObject)
        {
            return;
        }

        if (target != null)
        {
            if (selectedShip == target)
            {
                if (isAttacking)// Düzenle bunları
                {
                    cancelAttackCMD();
                }
                target.transform.GetChild(1).gameObject.SetActive(false);
                target = null;
                return;
            }
            target.transform.GetChild(1).gameObject.SetActive(false);;
        }
        target = selectedShip;
        target.transform.GetChild(1).gameObject.SetActive(true);
        targetType = _targetType;
        //target.transform.GetChild(2).gameObject.SetActive(true);
        if(isAttacking){
            //target.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else{
            //target.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void takeDamage(int damage)
    {
        if(isRepairing){
            RepairSystem.stopRepairing(this);
            uiManager.stopRepair();
            uiManager.setRepairSprite(0);
            
            uiManager.OnWriteScreenText("You cannot repair while under attack.");
            uiManager.stopRepair();
            uiManager.setRepairSprite(0);
        }
        playerData.health -= damage;
        if (playerData.health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        uiManager.setHealthBar(playerData.health , shipData.maxHealth);
        setHealthBar((float)playerData.health / (float)shipData.maxHealth);
    }
    
    public void takeHeal(int heal)
    {
        if(shipData.maxHealth - playerData.health <= heal){
            playerData.health = shipData.maxHealth;
            repairCMD();
        }
        else{
            playerData.health += heal;
            uiManager.startRepair();
        }
        uiManager.setHealthBar(playerData.health , shipData.maxHealth);
        setHealthBar((float)playerData.health / (float)shipData.maxHealth);
    }

    [ClientRpc]
    public void setHealthBar(float ratio)
    {
        bottomHealtBar.transform.localScale = new Vector3(ratio,1,1);
        if (ratio > 0.5f)
        {
            bottomHealtBar.GetComponent<Image>().color = Color.green;
        }
        else if (ratio > 0.25f)
        {
            bottomHealtBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            bottomHealtBar.GetComponent<Image>().color = Color.red;
        }
    }

    public void TakeLoot(AwardData _award)
    {
        uiManager.OnWriteScreenText(_award.AwardMessage);
        if(_award.awardAmounts[0] != 0){
            playerData.Gold += _award.awardAmounts[0];
            uiManager.setGoldText(playerData.Gold);
        }
        if(_award.awardAmounts[1] != 0){
            playerData.Experience += _award.awardAmounts[1];
            uiManager.setExpBar(playerData.Experience,playerData.requiredExperience);
        }
        if(_award.awardAmounts[2] != 0){
            playerData.Pearl += _award.awardAmounts[2];
            uiManager.setPearlText(playerData.Pearl);
        }
    }
}

public enum TargetType
{
    player,
    NPC
}
