using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    public List<Material> Materials = new List<Material>();

    private float timer;
    public float ResetTime;

    int colorcounter;
    // Update is called once per frame
    void Update () {

        timer += Time.deltaTime;

        if (timer > ResetTime)
        {
            timer = 0;
            this.GetComponent<MeshRenderer>().material = Materials[colorcounter];
            colorcounter += 1;
            if (colorcounter == Materials.Count)
                colorcounter = 0;

        }
	}
}
