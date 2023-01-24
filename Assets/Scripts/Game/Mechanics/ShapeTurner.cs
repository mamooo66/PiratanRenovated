using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTurner : MonoBehaviour
{
    private void FixedUpdate() {
        transform.Rotate(new Vector3(0,0,1) * 35 * Time.deltaTime);
    }
}