using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

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
    private bool Shoot;
    float x;

    Kugel currentlyshot;
    private float delta = 0.25f;
    public Vector3 DragDirection = Vector3.zero;
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

            this.transform.rotation = new Quaternion(0, 0, 0, 0);

        if (SA_AttachedKugeln.Count > 0)
        {
            GetComponentInChildren<Image>().fillAmount = 0;

            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            this.GetComponent<LineRenderer>().enabled = true;
            //this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            //this.GetComponent<LineRenderer>().SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            foreach (Kugel Kugel in SA_AttachedKugeln)
            {
                Physics.IgnoreCollision(Kugel.GetComponent<Collider>(), GetComponent<Collider>(), true);
                Kugel.GetComponent<Collider>().enabled = false;
                Kugel.GetComponentInChildren<MeshRenderer>().enabled = false;

                Kugel.GetComponent<LineRenderer>().enabled = true;
                Kugel.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                Kugel.GetComponent<LineRenderer>().SetPosition(1,   this.transform.position - Kugel.transform.position);
                if (Vector3.Distance(Kugel.transform.position, this.transform.position) > 0.1f)
                    delta += Time.deltaTime;
                Kugel.transform.position = Vector3.MoveTowards(Kugel.transform.position, this.transform.position, delta);
                Kugel.GetComponent<Rigidbody>().useGravity = false;
            }

            if (this == All[0])
                if (Input.GetKeyDown(KeyCode.Space) && !Shoot)
            {
                Shoot = true;

                foreach (Kugel Kugel in SA_AttachedKugeln)
                {
                    {
                        currentlyshot = Kugel;
                        delta = 0.25f;
                        Kugel.GetComponentInChildren<MeshRenderer>().enabled = true;
                        Kugel.GetComponent<Collider>().enabled = true;
                        Kugel.GetComponent<Rigidbody>().useGravity = true;
                        Kugel.GetComponent<Rigidbody>().AddForce((All[1].transform.position - this.transform.position).normalized * 55, ForceMode.Impulse);
                        Kugel.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                        Kugel.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
                        Kugel.GetComponent<LineRenderer>().enabled = false;
                        SA_AttachedKugeln.Remove(Kugel);
                        break;
                    }
                }
            }
            if (this == All[1])
                if (Input.GetKeyDown(KeyCode.M) && !Shoot)
                {
                    Shoot = true;

                    foreach (Kugel Kugel in SA_AttachedKugeln)
                    {
                        {
                            currentlyshot = Kugel;
                            delta = 0.25f;
                            Kugel.GetComponentInChildren<MeshRenderer>().enabled = true;
                            Kugel.GetComponent<Collider>().enabled = true;
                            Kugel.GetComponent<Rigidbody>().useGravity = true;
                            Kugel.GetComponent<Rigidbody>().AddForce((All[0].transform.position - this.transform.position).normalized * 55, ForceMode.Impulse);
                            Kugel.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                            Kugel.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
                            Kugel.GetComponent<LineRenderer>().enabled = false;
                            SA_AttachedKugeln.Remove(Kugel);
                            break;
                        }
                    }
                }
            if (Shoot)
            {
                x += Time.deltaTime;

                if (x > 0.25f)
                {
                    Shoot = false;
                    Physics.IgnoreCollision(currentlyshot.GetComponent<Collider>(), GetComponent<Collider>(), false);
                    x = 0;
                }
            }
        }
        if(SA_AttachedKugeln.Count == 0 && !SpecialAttackReady)
        {
            this.GetComponent<LineRenderer>().enabled = false;
            SAtimer += Time.deltaTime * 2;
            GetComponentInChildren<Image>().fillAmount = SAtimer;
            if(SAtimer > 1)
            {
                SAtimer = 0;
                SpecialAttackReady = true;
            }
        }
    }
    private bool SpecialAttackReady;
    private float SAtimer;
    public bool isPlayer1;
    public GameObject BLocker;

    public float SpecialAttackDistance;

    public bool SpecialAttack;
    void PlayerInput()
    {
        if (this == All[0])
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

            if (Input.GetKey(KeyCode.F))
            {
                BLocker.SetActive(true);            } else
            {
                BLocker.SetActive(false);
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
                SpecialAttackReady = false;
                foreach (Kugel Kugel in Kugel.All)
                {
                    {
                        if (Vector3.Distance(this.transform.position, Kugel.transform.position) < SpecialAttackDistance)
                        {
                            SA_AttachedKugeln.Add(Kugel);
                        }
                    }
                }
            }
        }
        if (this == All[1])
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * speed, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.down * speed, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
                GetComponent<Rigidbody>().AddForce(Vector3.left * speed, ForceMode.Impulse);

            if (Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<Rigidbody>().AddForce(Vector3.right * speed, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.K))
            {
                BLocker.SetActive(true);
            }
            else
            {
                BLocker.SetActive(false);
            }

            if (Input.GetKey(KeyCode.M))
            {
                SpecialAttack = true;
            }
            else
            {
                SpecialAttack = false;
            }
            if (SpecialAttack && SpecialAttackReady)
            {
                GetComponentInChildren<ParticleSystem>().Play();
                SpecialAttackReady = false;
                foreach (Kugel Kugel in Kugel.All)
                {
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

    private List<Kugel> SA_AttachedKugeln = new List<Kugel>();
    public void ResetIsKinematic()
    {

    }
}
