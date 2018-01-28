using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kugelspawner : MonoBehaviour {
    public static Kugelspawner instance;
    float timer;
    public int MaxKugelAnzahl = 5;

    public GameObject Ziel;
    private void Start()
    {
        instance = this;
    }
    public GameObject KugelPrefab;
    private GameObject instantiatedKugel;
	// Update is called once per frame
	void FixedUpdate () {
        if(Kugel.All.Count < MaxKugelAnzahl)
            timer += Time.deltaTime;

        if(timer > 2f && Kugel.All.Count < MaxKugelAnzahl)
        {
            instantiatedKugel = Instantiate(KugelPrefab, this.transform.position, new Quaternion(0, 0, 0, 0));
            instantiatedKugel.GetComponent<Rigidbody>().AddForce((Ziel.transform.position - this.transform.position).normalized * 55, ForceMode.Impulse);

            timer = 0;
            {

            }
        }

    }
}
