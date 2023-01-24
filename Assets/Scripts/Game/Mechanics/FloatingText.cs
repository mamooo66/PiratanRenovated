using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float speed;
    public float DestroyTime;
    public Vector3 offset;
    public Vector3 RandomizeIntensity;
    Canvas canvas;

    private void Start() {
        Destroy(gameObject, DestroyTime);

        transform.eulerAngles = new Vector3(50,0,0);

        transform.localPosition += offset;
        transform.localPosition += new Vector3(
            Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z)
        );
    }

    private void FixedUpdate() {
        transform.Translate(0, speed * Time.deltaTime,0);
    }
}