using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformCopy : MonoBehaviour {
    public bool x;
    public bool y;
    public bool z;

    public GameObject transformToCopy;

    private Vector3 tempvector;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        tempvector = this.transform.position;
        if (x)
            tempvector.x = transformToCopy.transform.position.x;
        if (y)
            tempvector.y = transformToCopy.transform.position.y;
        if (z)
            tempvector.z = transformToCopy.transform.position.z;
        this.transform.position = tempvector;
    }
}
