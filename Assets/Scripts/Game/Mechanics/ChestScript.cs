using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using Mirror;
using UnityEngine.SceneManagement;

public class ChestScript : NetworkBehaviour
{
    AwardData award;
    PlayerManager awardedPlayer;
    
    private void Start() {
        award = new AwardData();
        int a = Random.Range(0,100);
        if(a < 40){ //0-40
            //Gold
            int e = Random.Range(200, 800);
            award.awardAmounts[0] = e;
            award.AwardMessage = "You won " + award.awardAmounts[0] + " gold !";
        }
        else if(a < 80){ //40-80
        
            //Exp
            int e = Random.Range(50, 75);
            award.awardAmounts[1] = e;
            award.AwardMessage = "You won " + award.awardAmounts[1] + " experience !";
        }
        else{ // 80-100
            //Pearl
            int e = Random.Range(10, 25);
            award.awardAmounts[2] = e;
            award.AwardMessage = "You won " + award.awardAmounts[2] + " pearl !";
        } //TODO : Kısayolu varmı araştır
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player"){
            awardedPlayer = other.GetComponent<PlayerManager>();
            awardedPlayer.TakeLoot(award);
            NetworkServer.Destroy(gameObject);
        }
    }
}