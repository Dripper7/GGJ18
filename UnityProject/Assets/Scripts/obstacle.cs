using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleState { jumppad, slower, DestructableWall, EndZone };
public enum direction { top, bottom, left, right };

public class obstacle : MonoBehaviour
{
    public static List<obstacle> _All;
    public static List<obstacle> All
    {
        get { if (_All == null) _All = new List<obstacle>(); return _All; }
        private set { _All = value; }
    }
    public ObstacleState obstaclestate;
    public direction WallPosition;

    public AudioClip Abprallsound;
    public AudioClip DieSound;
    public AudioClip BreakSound;

    [Range(0, 25)]
    public float PlayerBounceStrength;
    [Range(1, 35)]
    public float BotBounceStrength;

    bool botUltraBoost;
    public float SlowStrength;

    public GameObject GameOver;

    public static bool playerhit;
    // Use this for initialization
    public Material HitMaterial;
    public float Health = 100;
    public void OnCollisionEnter(Collision collision)
    {
        if (botUltraBoost)
            BotBounceStrength *= 25;
        foreach (Player player in Player.All)
        {
            if (collision.gameObject == player.gameObject)
            {
                {
                    switch (obstaclestate)
                    {
                        case ObstacleState.jumppad:
                            if (WallPosition == direction.top)
                                player.GetComponent<Rigidbody>().AddForce(Vector3.down * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.up * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }
                            if (WallPosition == direction.left)
                                player.GetComponent<Rigidbody>().AddForce(Vector3.right * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.right)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.left * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }
                            GetComponent<AudioSource>().clip = Abprallsound;
                            GetComponent<AudioSource>().Play();
                            break;

                        case ObstacleState.slower:

                            player.speedblocker = true;
                            break;

                        case ObstacleState.DestructableWall:
                            GetComponent<AudioSource>().clip = Abprallsound;
                            GetComponent<AudioSource>().Play();
                            if (WallPosition == direction.top)
                                player.GetComponent<Rigidbody>().AddForce(Vector3.down * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.up * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }

                            if (WallPosition == direction.left)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.right * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }
                            if (WallPosition == direction.right)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.left * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }
                            break;
                        case ObstacleState.EndZone:
                            Destroy(player.gameObject);
                            GameOver.SetActive(true);
                            break;
                    }
                }
            }
        }
        foreach (Kugel kugel in Kugel.All)
        {
            if (collision.gameObject == kugel.gameObject)
            {
                kugel.GetComponentInChildren<ParticleSystem>().Play();
                {
                    switch (obstaclestate)
                    {
                        case ObstacleState.jumppad:
                            GetComponent<AudioSource>().clip = Abprallsound;
                            GetComponent<AudioSource>().Play();
                            if (WallPosition == direction.top)
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.down * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.up * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            }
                            if (WallPosition == direction.left)
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.right * kugel.speed * PlayerBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.right)
                            {
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.left * kugel.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }

                            break;

                        case ObstacleState.EndZone:
                            Destroy(kugel.gameObject);
                            break;

                        case ObstacleState.DestructableWall:
                            GetComponent<AudioSource>().clip = Abprallsound;
                            GetComponent<AudioSource>().Play();
                            Health -= 25f;
                            if (WallPosition == direction.top)
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.down * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.up * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            }

                            if (WallPosition == direction.left)
                            {
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.right * kugel.speed * BotBounceStrength, ForceMode.Impulse);

                            }
                            if (WallPosition == direction.right)
                            {
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.left * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            }
                            break;

                    }
                }
            }

        }
    }
    void Start()
    {
        All.Add(this);
    }

    private void OnDestroy()
    {
        All.Remove(this);
    }
    Color testmat;
    bool dead;
    bool broken;
    // Update is called once per frame
    void Update()
    {
        if (Health ==25)
        {
            if (!broken)
            {
                GetComponent<AudioSource>().clip = BreakSound;
                GetComponent<AudioSource>().Play();
                broken = true;
            }
            this.GetComponent<MeshRenderer>().material = HitMaterial;
        }
        if (Health <= 0)
        {
            if (!dead)
            {
                GetComponent<AudioSource>().clip = DieSound;
                GetComponent<AudioSource>().Play();
                dead = true;
            }

            if (!GetComponent<AudioSource>().isPlaying)
                Destroy(this.gameObject);
        }
    }
}

