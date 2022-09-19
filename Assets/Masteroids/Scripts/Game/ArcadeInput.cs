using UnityEngine;

public class ArcadeInput : MonoBehaviour
{
    [System.NonSerialized] public string hori, verti, R1, R2, R3, B1, B2, B3, start;
    public int playerNum;
    
    void Awake()
    {
        string prefix = "P" + playerNum + " ";
        hori = prefix + "Hori";
        verti = prefix + "Verti";

        R1 = prefix + "R1";
        R2 = prefix + "R2";
        R3 = prefix + "R3";

        B1 = prefix + "B1";
        B2 = prefix + "B2";
        B3 = prefix + "B3";

        start = prefix + "Start";
    }
}
