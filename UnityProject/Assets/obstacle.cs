using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Obstacle { jumppad, slower, Wall, right };
public enum direction { top, bottom, left, right };

public class obstacle : MonoBehaviour
{
    public Obstacle obstacle_;
    public direction WallPosition;

    [Range(0,10)]
    public float PlayerBounceStrength;
    [Range(1, 5)]
    public float BotBounceStrength;

    public float SlowStrength;



    public static bool playerhit;
    // Use this for initialization

    public float Health = 100;
    public void OnCollisionEnter(Collision collision)
    {
        foreach (Player player in Player.All)
        {
            if(collision.gameObject == player.gameObject)
            {
                if (player.isPlayer)
                { 
                    switch (obstacle_)
                    {
                        case Obstacle.jumppad:
                            player.GetComponent<Rigidbody>().AddForce(Vector3.up * player.speed * PlayerBounceStrength, ForceMode.Impulse);
                            break;

                        case Obstacle.slower:

                            player.speedblocker = true;
                            break;

                        case Obstacle.Wall:
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
                    }
                }
                else
                {
                    switch (obstacle_)
                    {
                        case Obstacle.jumppad:
                            player.GetComponent<Rigidbody>().AddForce(Vector3.up * player.speed * BotBounceStrength, ForceMode.Impulse);
                            break;

                        case Obstacle.slower:

                            player.speedblocker = true;
                            break;

                        case Obstacle.Wall:
                            Health -= 25f;
                            if (WallPosition == direction.top)
                                player.GetComponent<Rigidbody>().AddForce(Vector3.down * player.speed * BotBounceStrength, ForceMode.Impulse);
                            if (WallPosition == direction.bottom)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.up * player.speed  * BotBounceStrength, ForceMode.Impulse);
                            }

                            if (WallPosition == direction.left)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.right * player.speed  * BotBounceStrength, ForceMode.Impulse);
                            }
                            if (WallPosition == direction.right)
                            {
                                player.GetComponent<Rigidbody>().AddForce(Vector3.left * player.speed  * BotBounceStrength, ForceMode.Impulse);
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

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

