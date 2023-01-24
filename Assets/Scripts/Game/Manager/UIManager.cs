using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public Sprite[] attackSprites;
    public float reloadTime;
    public float reloadCounter;
    public bool isReload;
    public Image reloadCircle;
    public Image attackButton;

    public Sprite[] repairSprites;
    public float repairTime;
    public float repairCounter;
    public bool isRepair;
    public Image repairCircle;
    public Image repairButton;

    public Image expBar;
    public Text expText;

    public Image healthBar;
    public Text healthText;

    public Text goldText,pearlText;

    public Text cordinateText;

    public Scrollbar cameraZoomScroll;
    
    public Transform screenTextGrid;
    public GameObject screenTextPrefab;

    public AttackerEnemyPanel myTargetPanel;
    public Coroutine ITargetPanel;
    
    public Transform attackerGrid;
    public GameObject attackerSlotPrefab;
    
    public GameObject focusButton;

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (isRepair)
            {
                setRepairCircle();
            }

            if (isReload)
            {
                setReloadCircle();
            }
            setCordinateText();
        }
    }

    [TargetRpc]
    public void setShipSettings(float _repairTime, float _reloadTime)
    {
        repairTime = _repairTime;
        reloadTime = _reloadTime;
    }

    public void startReload()
    {
        reloadCounter = 0;
        isReload = true;
        reloadCircle.transform.parent.gameObject.SetActive(true);
    }
    
    [TargetRpc]
    public void startRepair()
    {
        repairCounter = 0;
        isRepair = true;
        repairCircle.transform.parent.gameObject.SetActive(true);
    }
    
    public void setRepairCircle()
    {
        repairCounter += Time.deltaTime;
        float ratio = repairCounter / repairTime;
        repairCircle.fillAmount = ratio;
    }

    [TargetRpc]
    public void stopRepair()
    {
        isRepair = false;
        repairCircle.transform.parent.gameObject.SetActive(false);
    }

    
    public void setReloadCircle()
    {
        reloadCounter += Time.deltaTime;
        float ratio = reloadCounter / reloadTime;
        reloadCircle.fillAmount = ratio;
    }

    
    public void stopReload()
    {
        isReload = false;
        reloadCircle.transform.parent.gameObject.SetActive(false);
    }
    
    
    public void setAttackSprite(int spriteNo)
    {
        attackButton.sprite = attackSprites[spriteNo];
    }
    
    [TargetRpc]
    public void setRepairSprite(int spriteNo)
    {
        repairButton.sprite = repairSprites[spriteNo];
    }

    [TargetRpc]
    public void setHealthBar(int health, int maxHealth)
    {
        float ratio =  (float)health / (float)maxHealth;
        healthBar.fillAmount = ratio;
        healthText.text = health + "/" + maxHealth;
        /*
        if (ratio > 0.5f)
        {
            healthBar.GetComponent<Image>().color = Color.green;
        }
        else if (ratio > 0.25f)
        {
            healthBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.GetComponent<Image>().color = Color.red;
        }
        */
    }
    
    public void setCordinateText()
    {
        cordinateText.text = CordinateCalculate.CordinateReturn(transform.position.x, transform.position.z);
    }

    public void changedCameraSize()
    {
        CameraCommands.CameraZoom(cameraZoomScroll.value);
    }
    
    public void OnWriteScreenText(string screenSTR)
    {
        var screenText = Instantiate(screenTextPrefab,screenTextGrid.position,Quaternion.identity, screenTextGrid);
        screenText.transform.GetChild(0).GetComponent<Text>().text = screenSTR;
        StartCoroutine(IScreenText(screenSTR.Length * 0.1f, screenText));
    }

    public IEnumerator IScreenText(float time, GameObject screenText){
        screenText.GetComponent<Animation>().Play("ScreenTextOpen");
        yield return new WaitForSeconds(time);
        screenText.GetComponent<Animation>().Play("ScreenTextClose");
        yield return new WaitForSeconds(0.5f);
        Destroy(screenText);
    }

    [TargetRpc]
    public void setGoldText(int gold)
    {
        goldText.text = "Gold:" + gold;
    }
    
    [TargetRpc]
    public void setPearlText(int pearl)
    {
        pearlText.text = "Pearl:" + pearl;
    }
    
    [TargetRpc]
    public void setExpBar(int exp, int necessaryExp)
    {
        float ratio = (float)exp / (float)necessaryExp;
        expBar.fillAmount = ratio;
        expText.text = exp + "/" + necessaryExp;
    }

    [TargetRpc]
    public void targetPanelSet(GameObject _enemy, string _enemyName, string _enemyHealthText, float _healthRatio)
    {
        myTargetPanel.SetSlot(_enemy,
            _enemyName,
            _enemyHealthText,
            _healthRatio);
        if (ITargetPanel != null)
        {
            StopCoroutine(ITargetPanel);
        }
        ITargetPanel = StartCoroutine(ITargetPanelTimer());
    }

    private IEnumerator ITargetPanelTimer()
    {
        myTargetPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(10);
        myTargetPanel.gameObject.SetActive(false);
    }
    
    [TargetRpc]
    public void AttackerPanelAdd(GameObject _enemy, string _enemyName, string _enemyHealthText, float _healthRatio){
        for (int i = 0; i < attackerGrid.transform.childCount; i++)
        {
            if(attackerGrid.GetChild(i).GetComponent<AttackerEnemyPanel>().enemy == _enemy){
                attackerGrid.GetChild(i).GetComponent<AttackerEnemyPanel>().SetSlot(_enemy,
                    _enemyName,
                    _enemyHealthText,
                    _healthRatio);
                return;
            }
        }
        var bar = Instantiate(attackerSlotPrefab, attackerGrid.transform.position, Quaternion.identity, attackerGrid);
        bar.GetComponent<AttackerEnemyPanel>().SetSlot(_enemy,
            _enemyName,
            _enemyHealthText,
            _healthRatio);
    }
    public void focusButtonShow()
    {
        focusButton.SetActive(true);
    }
}
