using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KugelState { Shot, Loaded, Floating}
public class Kugel : MonoBehaviour {

    public KugelState kugelstate;
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
    private float x;
    private float y;

    public float delta = 0.25f;
    public void EnableCollider(Player player,Kugel kugel)
    {
        StartCoroutine(EnableCollider2(player, kugel));
    }
    private IEnumerator EnableCollider2(Player player, Kugel kugel)
    {
        while (y < 0.15f)
        {
            y += Time.deltaTime;
            Debug.Log("waitign");
            yield return null;
        }
        Debug.Log("finished");

        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), false);
        y = 0;

    }
    void Update()
    {
        switch (kugelstate)
        {
            case KugelState.Shot:
                this.GetComponentInChildren<MeshRenderer>().enabled = true;
                this.GetComponent<Collider>().enabled = true;
                this.GetComponent<Rigidbody>().useGravity = true;
                this.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                this.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
                this.GetComponent<LineRenderer>().enabled = false;
                this.gameObject.transform.localScale = new Vector3(2,2,2);
                x += Time.deltaTime;
                delta = 0.25f;

                if (x > 0.25f)
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                    x = 0;
                    this.kugelstate = KugelState.Floating;
                }
                break;
            case KugelState.Floating:

            break;
            case KugelState.Loaded:
                this.GetComponent<Collider>().enabled = false;
                this.GetComponentInChildren<MeshRenderer>().enabled = false;

                this.GetComponent<LineRenderer>().enabled = true;
                this.GetComponent<Rigidbody>().useGravity = false;
                break;
        }
   
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
            Destroy(this.gameObject);
        }
        if (collision.gameObject == Endzone2)
        {
            Player.All[1].speedlimit += 5;
            Destroy(this.gameObject);
        }
    }
}