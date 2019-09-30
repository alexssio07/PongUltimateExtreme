using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public NamePlayer NamePlayer;
    public float HealthPlayer;

    public bool IsDeath;
    public float Health
    {
        get { return health; }
        set
        {
            if (health > 0 && !IsDeath)
                health -= value;
            else
            {
                IsDeath = true;
                health = 0;
            }
        }
    }

    [SerializeField]
    private string playerAxisInput;
    [SerializeField]
    private float speedPaddle;

    private float health;
    private static float timeRespawnPlayer = 2f;
    private static float boundY = 4.46f;
    private float currentTimerSlowing;
    private float currentTimerScaling;
    private float tempSpeed;
    private Vector3 startScalePaddle;
    private bool isSlowed;
    private Rigidbody2D rb;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isSlowed = false;
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.spawnPlayer, ResetPlayer);
        EventManager.instance.AddListener(MyIndexEvent.playerHitted, OnSetHealthPlayer);
        EventManager.instance.AddListener(MyIndexEvent.potion, OnSetSpeedPlayer);
        EventManager.instance.AddListener(MyIndexEvent.hammer, OnSetScalePaddle);
        EventManager.instance.AddListener(MyIndexEvent.pill, OnSetHealthPlayer);
        startScalePaddle = this.transform.localScale;
    }

    private void Update()
    {
        Vector2 velocity = rb.velocity;
        velocity.y = Input.GetAxis(playerAxisInput) * speedPaddle;
        rb.velocity = velocity;
        Vector3 position = transform.position;
        if (position.y < -boundY)
        {
            position.y = -boundY;
        }
        else if (position.y > boundY)
        {
            position.y = boundY;
        }
        transform.position = position;

        if (currentTimerSlowing > 0)
            currentTimerSlowing -= Time.deltaTime;
        else if (isSlowed)
        {
            isSlowed = false;
            currentTimerSlowing = 0;
            speedPaddle = tempSpeed;
        }

        if (currentTimerScaling > 0)
        {
            currentTimerScaling -= Time.deltaTime;
        }
        else
        {
            currentTimerScaling = 0;
            this.transform.localScale = startScalePaddle;
        }

    }

    public void OnSetHealthPlayer(MyEventArgs e)
    {
        Debug.Log("OnSetHealthPlayer");
        if (Health > 0 && !IsDeath)
            Health -= e.myFloat;
        else if (!IsDeath)
        {
            EventManager.instance.CastEvent(MyIndexEvent.respawnPlayer, new MyEventArgs() { sender = gameObject, myFloat = timeRespawnPlayer, namePlayer = this.NamePlayer });
        }
    }

    public void OnSetSpeedPlayer(MyEventArgs e)
    {
        Debug.Log("OnSetSpeedPlayer");
        //isSlowed = true;
        //if (e.myString != isHittedBall)
        //{
        //    tempSpeed = speedPaddle;
        //    speedPaddle = speedPaddle - (speedPaddle * e.myFloat / 100);
        //}
        //currentTimerSlowing = e.mySecondFloat;
    }

    public void OnSetScalePaddle(MyEventArgs e)
    {
        // TODO Implementare duration per lo scale
        // TODO Implementare scaling sul giocatore avversario... (?)
        Debug.Log("SetScalePaddle");
        //if (e.myString != isHittedBall)
        //{
        //    if (!e.myBool)
        //        this.transform.localScale += new Vector3(startScalePaddle.x * e.myFloat / 100, startScalePaddle.y * e.myFloat / 100, 0);
        //    else
        //        this.transform.localScale -= new Vector3(startScalePaddle.x * e.myFloat / 100, startScalePaddle.y * e.myFloat / 100, 0);
        //}
        //currentTimerScaling = e.mySecondFloat;
    }

    public void ResetPlayer(MyEventArgs e)
    {
        if (e.namePlayer == NamePlayer.PlayerLeft || e.namePlayer == NamePlayer.PlayerRight)
        {
            if (IsDeath)
            {
                health = HealthPlayer;
                IsDeath = false;
            }
        }
    }
}

public enum NamePlayer
{
    None,
    PlayerLeft,
    PlayerRight
}
