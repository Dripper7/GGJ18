using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyPressed { W, A, S, D }
public class InputManager : MonoBehaviour
{

    public static InputManager instance;
    private KeyPressed MainKeyPressed;
    // Use this for initialization
    void Start()
    {
        instance = this;

    }
    private Vector3 temp_playerposition;
    [Range(1, 25)]
    public int speedfactorBegrenzung;
    public int speedfactorVerschnellerung;

    public bool speedblocker;
    // Update is called once per frame
    void Update()
    {

        switch (MainKeyPressed)
        {
            case KeyPressed.W:

                break;
            case KeyPressed.A:

                break;
            case KeyPressed.S:


                break;
            case KeyPressed.D:


                break;
        }



    }
}
