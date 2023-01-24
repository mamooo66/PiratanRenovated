using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackerEnemyPanel : MonoBehaviour
{
    public GameObject enemy;
    public float countDown = 10f;
    public Text enemyName;
    public Text enemyHealthText;
    public Image enemyHealthBar;

    public bool isMyTarget;
    private void FixedUpdate() {
        if (!isMyTarget)
        {
            countDown -= Time.deltaTime;
            if(countDown <= 0){
                Destroy(gameObject);
            }
        }
    }

    public void SetSlot(GameObject _enemy, string _enemyName, string _enemyHealthText, float _healthRatio)
    {
        countDown = 10;
        enemy = _enemy;
        enemyName.text = _enemyName;
        enemyHealthText.text = _enemyHealthText;
        enemyHealthBar.fillAmount = _healthRatio;
    }
}
