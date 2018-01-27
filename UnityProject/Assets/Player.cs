using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance;
    // Use this for initialization
    public bool speedblocker;
    [Range(0,1)]
    public float SpeedPercentage;
    public float speed;
    public float botSpeed;

     [Range(1,5)]
    public float speedfactor = 1;
    public float speedlimit;

    public float minspeed;


    private float timer;


    public static List<Player> _All;
    public static List<Player> All
    {
        get { if (_All == null) _All = new List<Player>(); return _All; }
        private set { _All = value; }
    }

    private Vector3 zAxis;
    void Start () {
        All.Add(this);
    }
    private void OnDestroy()
    {
        All.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (speedfactor > speedlimit)
            speedfactor = speedlimit;
        if (speed < minspeed)
            speed = minspeed * SpeedPercentage;
        speed = speedfactor * SpeedPercentage;


        zAxis = transform.localPosition;
        zAxis.z = 0;
        transform.localPosition = zAxis;

        if (timer > 2)
        {
            speedblocker = false;
            timer = 0;
        }
        if (speedblocker)
        {
            speedlimit = 0.5f;
            timer += Time.deltaTime;
        }
        PlayerInput();
        if (!isPlayer)
        {
            speed = speed * botSpeed;
        }
        if (isPlayer)
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

        if (SA_AttachedKugeln.Count > 0)
        {
            foreach (Player Kugel in SA_AttachedKugeln)
            {
                Kugel.GetComponent<LineRenderer>().enabled = true;
                Kugel.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                Kugel.GetComponent<LineRenderer>().SetPosition(1,   this.transform.position - Kugel.transform.position);
                Kugel.GetComponent<Rigidbody>().useGravity = false;
                Kugel.GetComponent<Collider>().enabled = false;
                Kugel.transform.position = Vector3.MoveTowards(Kugel.transform.position, this.transform.position, 0.5f);
                SpecialAttackReady = false;
            }
            SAtimer += Time.deltaTime;
            if (SAtimer > 5)
            {
                SpecialAttackReady = true;
                SAtimer = 0;
                foreach (Player Kugel in SA_AttachedKugeln)
                {
                    if (!Kugel.isPlayer)
                    {

                        Kugel.GetComponent<Rigidbody>().AddForce(Kugel.transform.position - this.transform.position, ForceMode.Impulse);
                        Kugel.GetComponent<LineRenderer>().enabled = false;
                        Kugel.GetComponent<Rigidbody>().useGravity = true;
                        Kugel.GetComponent<Collider>().enabled = true;

                    }
                }
                SA_AttachedKugeln.Clear();

            }
        }
    }

    private float SAtimer;
    public bool isPlayer;
    public float SpecialAttackDistance;

    public bool SpecialAttack;
    private bool SpecialAttackReady = true;
    void PlayerInput()
    {
        if (isPlayer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * speed, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.S))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.down * speed, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.A))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.left * speed, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.right * speed, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                SpecialAttack = true;
            }else
            {
                SpecialAttack = false;
            }
            if (SpecialAttack && SpecialAttackReady)
            {
                GetComponentInChildren<ParticleSystem>().Play();

                foreach (Player Kugel in All)
                {
                    if (!Kugel.isPlayer)
                    {
                        if (Vector3.Distance(this.transform.position, Kugel.transform.position) < SpecialAttackDistance)
                        {
                            SA_AttachedKugeln.Add(Kugel);
                        }
                    }
                }
            }
        }


    }

    private List<Player> SA_AttachedKugeln = new List<Player>();
    public void ResetIsKinematic()
    {

    }
}
