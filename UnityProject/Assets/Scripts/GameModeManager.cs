using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameModeManager : MonoBehaviour {

    public GameObject CUrrentlyLoaded;
    public static GameModeManager instance;
    public UnityEvent TestEvent;
    void Awake ()
    {
        instance = this;
        Debug.Log(instance);
    }

    public void Resettere()
    {
        TestEvent.Invoke();
        if (CUrrentlyLoaded != null)
        {
            Destroy(CUrrentlyLoaded);
            CUrrentlyLoaded = null;
        }
    }
	// Update is called once per frame
	public void LoadMode (GameObject Mode)
    {
        CUrrentlyLoaded = Instantiate(Mode,transform.root);
    }

    public void UnloadMode(GameObject Mode)
    {
        Destroy(CUrrentlyLoaded);
    }
}
