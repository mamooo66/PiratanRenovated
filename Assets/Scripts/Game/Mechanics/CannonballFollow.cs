using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballFollow : MonoBehaviour
{
    public Transform TargetShip;
    private float z, x;
    [SerializeField] private float speed = 40;
    [SerializeField] private float yStable = -0.8f;
    private Vector3 deflection;

    private void Start()
    {
        Destroy(gameObject, 15);
        x = Vector3.Distance(transform.position, TargetShip.transform.position);
        //speed += UnityEngine.Random.Range(-3f, 1.5f);
        z = 140 / (x / speed);
        transform.rotation = Quaternion.Euler(0, -70, 0);
        deflection = new Vector3(Random.Range(-1.5f, 1.5f),0,Random.Range(-1.5f, 1.5f));
    }

    private void FixedUpdate(){
        if(TargetShip == null){
            Destroy(gameObject);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 70, 0)), z * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0.6f, transform.position.z), yStable * -transform.rotation.y);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(TargetShip.transform.position.x, transform.position.y, TargetShip.transform.position.z) + deflection, speed * Time.deltaTime);
    }

    public void SetBall(Transform targetShip)
    {
        TargetShip = targetShip;
    } 

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") || other.CompareTag("NPC"))
        {
            if (other.transform == transform.parent)
            {
                Destroy(gameObject);
            }
        }
    }
}