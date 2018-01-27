using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kugel : MonoBehaviour {
    public static List<Kugel> _All;
    public static List<Kugel> All
    {
        get { if (_All == null) _All = new List<Kugel>(); return _All; }
        private set { _All = value; }
    }
    // Use this for initialization
    void Start() {
        All.Add(this);
    }
    private void OnDestroy()
    {
        All.Remove(this);
    }
   [Range(0, 1)]
    public float SpeedPercentage;
    public float speed;
    public float botSpeed;

    [Range(1, 5)]
    public float speedfactor = 1;
    public float speedlimit;

    public float minspeed;


    private float timer;

    private Vector3 zAxis;
    public GameObject Endzone1;
    public GameObject Endzone2;

    public Vector3 DragDirection = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        speed = speed * botSpeed;

        if (speedfactor > speedlimit)
            speedfactor = speedlimit;
        if (speed < minspeed)
            speed = minspeed * SpeedPercentage;
        speed = speedfactor * SpeedPercentage;


        zAxis = transform.localPosition;
        zAxis.z = 0;
        transform.localPosition = zAxis;

        this.transform.rotation = new Quaternion(0, 0, 0, 0);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Endzone1)
        {
            Player.All[0].speedlimit += 5;
            Destroy(this);
        }
        if (collision.gameObject == Endzone2)
        {
            Player.All[1].speedlimit += 5;
            Destroy(this);
        }
    }
}