using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kugelspawner : MonoBehaviour {
    public static Kugelspawner instance;
    float timer;
    public int MaxKugelAnzahl = 5;
    Random rnd = new Random();
    public GameObject Ziel;
    private void Start()
    {
        instance = this;
    }
    public GameObject KugelPrefab;
    public GameObject CapsulePrefab;
    public GameObject RandomPrefab;

    private GameObject InstantiatePreb;


    int i;
    private GameObject instantiatedKugel;
	// Update is called once per frame
	void FixedUpdate () {
             i = Random.Range(0, 2);
            if (i == 0)
            {
                InstantiatePreb = KugelPrefab;
            }
            if (i == 1)
            {
                InstantiatePreb = CapsulePrefab;
            }
            if (i == 2)
            {
                InstantiatePreb = RandomPrefab;
            }        
        if (Kugel.All.Count < MaxKugelAnzahl)
            timer += Time.deltaTime;

        if(timer > 0.5f && Kugel.All.Count < MaxKugelAnzahl)
        {

            instantiatedKugel = Instantiate(InstantiatePreb, this.transform.position, new Quaternion(0, 0, 0, 0));
            instantiatedKugel.GetComponent<Rigidbody>().AddForce((Ziel.transform.position - this.transform.position).normalized * 45, ForceMode.Impulse);

            timer = 0;
            {

            }
        }

    }
}
