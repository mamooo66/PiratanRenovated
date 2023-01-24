using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordinateCalculate : MonoBehaviour
{
    private static string[] Alphabet = new string[]{"A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
    public static float MapSize = 1000;
    private static float CordinateSize = 60;
    private static float Ratio = CordinateSize / MapSize;

    public static string CordinateReturn(float x, float z){
        int AlphabetNo = 0;
        z = z * Ratio * -1 + CordinateSize / 2;
        while(z > 26){
            z -= 26;
            AlphabetNo++;
        }
        string xCor = ((int)(x * Ratio + CordinateSize / 2 + 1)).ToString("00");
        string zCor = Alphabet[AlphabetNo] + Alphabet[(int)z];
        return xCor + " " + zCor;
    }
}
