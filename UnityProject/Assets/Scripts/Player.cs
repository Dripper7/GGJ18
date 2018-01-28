using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    // Use this for initialization
    public bool speedblocker;
    [Range(0, 1)]
    public float SpeedPercentage;
    public float speed;
    public float botSpeed;

    [Range(1, 5)]
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
    void Start()
    {
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
    void FixedUpdate()
    {
        PlayerInput();

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

        this.transform.rotation = new Quaternion(0, 0, 0, 0);

        if (SA_AttachedKugeln.Count > 0)
        {
            GetComponentInChildren<Image>().fillAmount = 0;

            var v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            //this.GetComponent<LineRenderer>().enabled = true;
            //this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            //this.GetComponent<LineRenderer>().SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            foreach (Kugel Kugel in SA_AttachedKugeln)
            {
                Kugel.kugelstate = KugelState.Loaded;

                Physics.IgnoreCollision(Kugel.GetComponent<Collider>(), GetComponent<Collider>(), true);
                this.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                Kugel.GetComponent<LineRenderer>().SetPosition(1, this.transform.position - Kugel.transform.position);
                if (Vector3.Distance(Kugel.transform.position, this.transform.position) > 0.1f)
                    Kugel.delta += Time.deltaTime * 2;
                Kugel.transform.position = Vector3.MoveTowards(Kugel.transform.position, this.transform.position, Kugel.delta);
            }

            if (this == All[0])
                foreach (Kugel Kugel in SA_AttachedKugeln)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && !Shoot && Vector3.Distance(this.transform.position, Kugel.transform.position) < 0.5f)
                    {
                        //Shoot = true;

                        {
                            {
                                GetComponent<AudioSource>().clip = aShot;
                                GetComponent<AudioSource>().Play();
                                Kugel.kugelstate = KugelState.Shot;
                                SA_AttachedKugeln.Remove(Kugel);
                                Kugel.GetComponent<Rigidbody>().AddForce((All[1].transform.position - this.transform.position).normalized * 55, ForceMode.Impulse);
                                Kugel.EnableCollider(this, Kugel);
                                break;
                            }
                        }
                    }
                }
            if (this == All[1])
                foreach (Kugel kugel in SA_AttachedKugeln)
                {
                    if (Input.GetKeyDown(KeyCode.M) && !Shoot && Vector3.Distance(this.transform.position, kugel.transform.position) < 0.5f)
                {
                        {
                            {
                                GetComponent<AudioSource>().clip = aShot;
                                GetComponent<AudioSource>().Play();
                                kugel.kugelstate = KugelState.Shot;
                                SA_AttachedKugeln.Remove(kugel);
                                kugel.GetComponent<Rigidbody>().AddForce((All[0].transform.position - this.transform.position).normalized * 55, ForceMode.Impulse);
                                kugel.EnableCollider(this, kugel);
                                break;
                            }
                        }
                    }
                }
        }

        if (SA_AttachedKugeln.Count == 0 && !SpecialAttackReady)
        {
            this.GetComponent<LineRenderer>().enabled = false;
            SAtimer += Time.deltaTime * 2;
            GetComponentInChildren<Image>().fillAmount = SAtimer;
            if (SAtimer > 1)
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
    float timerkey = 3;
    float timerKeyw = 0;
    float timerKeya = 0;
    float timerKeys = 0;
    float timerKeyd = 0;

    public bool SpecialAttack;
    void PlayerInput()
    {
        timerKeyw = timerkey;
        timerKeya = timerkey;
        timerKeys = timerkey;
        timerKeyd = timerkey;
        if (this == All[0])
        {


            if (Input.GetKey(KeyCode.W))
            {
                if (GetComponent<Rigidbody>().velocity.magnitude < 50)
                {
                    timerKeyw += Time.deltaTime;
                    GetComponent<Rigidbody>().AddForce(Vector3.up * speed * timerKeyw, ForceMode.Impulse);
                }
                else
                {
                    timerKeyw = timerkey;
                    GetComponent<Rigidbody>().AddForce(Vector3.up * speed, ForceMode.Impulse);
                }

            }
            if (Input.GetKey(KeyCode.S))
            {
                timerKeys += Time.deltaTime;

                if (GetComponent<Rigidbody>().velocity.magnitude < 50)
                {
                    GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * speed * timerKeys, ForceMode.Impulse);
                }
                else
                {
                    timerKeys = timerkey;
                    GetComponent<Rigidbody>().AddForce(Vector3.down * speed, ForceMode.Impulse);

                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                timerKeya += Time.deltaTime;

                if (GetComponent<Rigidbody>().velocity.magnitude < 50)
                {
                    GetComponent<Rigidbody>().AddForce(Vector3.left * speed * timerKeya, ForceMode.Impulse);
                }
                else
                {
                    timerKeya = timerkey;

                    GetComponent<Rigidbody>().AddForce(Vector3.left * speed, ForceMode.Impulse);

                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                timerKeyd += Time.deltaTime;

                if (GetComponent<Rigidbody>().velocity.magnitude < 50)
                {
                    GetComponent<Rigidbody>().AddForce(Vector3.right * speed * timerKeyd, ForceMode.Impulse);
                }
                else
                {
                    timerKeyd = timerkey;

                    GetComponent<Rigidbody>().AddForce(Vector3.right * speed, ForceMode.Impulse);

                }
            }

            if (Input.GetKey(KeyCode.F))
            {
                BLocker.SetActive(true);
            }
            else
            {
                BLocker.SetActive(false);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                SpecialAttack = true;
            }
            else
            {
                SpecialAttack = false;
            }
            if (SpecialAttack && SpecialAttackReady)
            {
                GetComponent<AudioSource>().clip = aSpecialAttacK;
                GetComponent<AudioSource>().Play();
                GetComponentInChildren<ParticleSystem>().Play();
                SpecialAttackReady = false;
                foreach (Kugel Kugel in Kugel.All)
                {
                    if(Kugel.kugelstate != KugelState.Loaded)
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
                GetComponent<AudioSource>().clip = aSpecialAttacK;
                GetComponent<AudioSource>().Play();
                GetComponentInChildren<ParticleSystem>().Play();
                SpecialAttackReady = false;
                foreach (Kugel Kugel in Kugel.All)
                {
                    if (Kugel.kugelstate != KugelState.Loaded)
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

    public AudioClip aSpecialAttacK;
    public AudioClip aShot;
    public AudioClip aMove;


    private List<Kugel> SA_AttachedKugeln = new List<Kugel>();
    public void ResetIsKinematic()
    {

    }
}
