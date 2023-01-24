using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RepairSystem : NetworkBehaviour
{
    //! GEMİNİN TAMİRİ NASIL İPTAL OLUR
    
    //? Bir saldırı aldığında
    //? Tamir iptal edildiğinde
    //? Can full olduğu zaman
    
    //! GEMİ HANGİ KOŞULLARDA TAMİR YAPABİLİR
    
    //? Saldırı yapmıyorsa
    //? Canı max değilse
    
    //! BİR TAMİR NASIL GERÇEKLEŞİR
    //1- Gerekli koşullar sağlandı ise tamir başlar
    //NOT: Tamir yapılırken hareket edilemez
    //2- Tamir süresi max süreye ulaştığında dolduğunda gemiye can verilir
    //3- Verilen can max değil ise döngü tekrar başlar
    //4- Can fullendi ise can max a çekilir tamir durdurulur
/*
    public static void Repair(NPCManager ship)
    {
        if (ship.isRepairing == false && ship.isAttacking == false && ship.health < ship.maxHealth)
        {
            ship.isRepairing = true;
            ship.StartCoroutine(Repairing(ship));
        }
    }
    */

    private static RepairSystem instance;

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

    public static void startRepairing(PlayerManager ship)
    {
        Debug.Log(2);
        if (!ship.isAttacking &&
            ship.shipData.maxHealth != ship.playerData.health)
        {
            ship.uiManager.startRepair();
            ship.uiManager.setRepairSprite(1);
            ship.isRepairing = true;
            MoveCommands.Stop(ship.agent);
            ship.repairCoroutine = RepairSystem.instance.StartCoroutine(Repairing(ship));
        }
    }
    
    public static void stopRepairing(PlayerManager ship)
    {
        ship.isRepairing = false;
        Debug.Log(3);
        RepairSystem.instance.StopCoroutine(ship.repairCoroutine);
    }

    public static IEnumerator Repairing(NPCManager ship)
    {
        throw new System.NotImplementedException();
    }
    
    public static IEnumerator Repairing(PlayerManager ship)
    {
        yield return new WaitForSeconds(5);
        if (!ship.isRepairing)
        {
            yield break;
        }
        RepairSystem.instance.repairRPC(ship.transform, ship.shipData.maxHealth / 100 * 10);
        ship.takeHeal(ship.shipData.maxHealth / 100 * 10);
        ship.repairCoroutine = RepairSystem.instance.StartCoroutine(Repairing(ship));
    }

    [ClientRpc]
    public void repairRPC(Transform pos, int heal)
    {
        var b = Instantiate(Resources.Load<GameObject>("FloatingText"), pos.position, Quaternion.identity, pos);
        b.GetComponent<TextMesh>().text = "+" + heal;
        b.GetComponent<TextMesh>().color = Color.green;
    }
}
