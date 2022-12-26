using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairSystem : MonoBehaviour
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
    

    private static IEnumerator Repairing(NPCManager ship)
    {
        throw new System.NotImplementedException();
    }
}
