﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obstacle { jumppad, slower, Wall, EndZone };
public enum direction { top, bottom, left, right };

public class obstacle : MonoBehaviour
{
    public Obstacle obstacle_;
    public direction WallPosition;

    public AudioClip Abprallsound;
    public AudioClip DieSound;
    public AudioClip BreakSound;

    [Range(0, 25)]
    public float PlayerBounceStrength;
    [Range(1, 15)]
    public float BotBounceStrength;

    public float SlowStrength;

    public GameObject GameOver;

    public static bool playerhit;
    // Use this for initialization
    public Material HitMaterial;
    public float Health = 100;
    public void OnCollisionEnter(Collision collision)
    {
        foreach (Player player in Player.All)
        {
            if (collision.gameObject == player.gameObject)
            {
                {
                    switch (obstacle_)
                    {
                        case Obstacle.jumppad:
                            if (WallPosition == direction.top)
                                player.GetComponent<Rigidbody>().AddForce(Vector3.down * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.up * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            }
                            GetComponent<AudioSource>().clip = Abprallsound;
                            GetComponent<AudioSource>().Play();
                            break;

                        case Obstacle.slower:

                            player.speedblocker = true;
                            break;

                        case Obstacle.Wall:
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
                        case Obstacle.EndZone:
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
                kugel.GetComponent<ParticleSystem>().Play();
                {
                    switch (obstacle_)
                    {
                        case Obstacle.jumppad:
                            GetComponent<AudioSource>().clip = Abprallsound;
                            GetComponent<AudioSource>().Play();
                            if (WallPosition == direction.top)
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.down * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                kugel.GetComponent<Rigidbody>().AddForce(Vector3.up * kugel.speed * BotBounceStrength, ForceMode.Impulse);
                            }
                            break;

                        case Obstacle.slower:

                            break;

                        case Obstacle.Wall:
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
    void St()
    {

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
