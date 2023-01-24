using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCommands : MonoBehaviour
{
    public static void CameraMove(Vector3 move)
    {
        Camera.main.transform.position += move;
    }
    
    public static void CameraMoveToPlayer(Transform player)
    {
        Vector3 offset = new Vector3(0, 53f, -50f);
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, 0.150f);
        Camera.main.transform.position = smoothedPosition;
    }

    public static void CameraZoom(float value)
    {
        Camera.main.orthographicSize = 30 + (value * 90);
        //minimapCamObject.transform.localScale = CameraSize();
    }
}
