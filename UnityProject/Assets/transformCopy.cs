using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformCopy : MonoBehaviour {
    public GameObject transformToCopy;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = transformToCopy.transform.position;

    }
}
