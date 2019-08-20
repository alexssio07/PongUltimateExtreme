using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string isHittedBall;
    [SerializeField]
    private string playerAxisInput;
    [SerializeField]
    private Rigidbody2D rigidbody;
    [SerializeField]
    private float speedPaddle;
    [SerializeField]
    private float boundY = 4.46f;
    [SerializeField]
    private float health;


    private float currentTimerSlowing;
    private float currentTimerScaling;
    private float tempSpeed;
    private Vector3 startScalePaddle;
    private bool isSlowed;



    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isSlowed = false;
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.playerHitted, OnSetDamagePlayer);
        EventManager.instance.AddListener(MyIndexEvent.potion, OnSetHealPlayer);
        EventManager.instance.AddListener(MyIndexEvent.hammer, OnSetScalePaddle);
        EventManager.instance.AddListener(MyIndexEvent.pill, OnSetHealPlayer);
        startScalePaddle = this.transform.localScale;
    }

    private void Update()
    {
        Vector2 velocity = rigidbody.velocity;
        velocity.y = Input.GetAxis(playerAxisInput) * speedPaddle;
        rigidbody.velocity = velocity;
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

    public void OnSetDamagePlayer(MyEventArgs e)
    {
        Debug.Log("OnSetHealthPlayer");
        if (health <= 0)
        {
            health = 0;
            // TODO Gameover
        }
        else
        {
            health -= e.myFloat;
        }
    }

    public void OnSetSpeedPlayer(MyEventArgs e)
    {
        Debug.Log("OnSetSpeedPlayer");
        isSlowed = true;
        if (e.myString != isHittedBall)
        {
            tempSpeed = speedPaddle;
            speedPaddle = speedPaddle - (speedPaddle * e.myFloat / 100);
        }
        currentTimerSlowing = e.mySecondFloat;
    }

    public void OnSetScalePaddle(MyEventArgs e)
    {
        // TODO Implementare duration per lo scale
        // TODO Implementare scaling sul giocatore avversario... (?)
        Debug.Log("SetScalePaddle");
        if (e.myString != isHittedBall)
        {
            if (!e.myBool)
                this.transform.localScale += new Vector3(startScalePaddle.x * e.myFloat / 100, startScalePaddle.y * e.myFloat / 100, 0);
            else
                this.transform.localScale -= new Vector3(startScalePaddle.x * e.myFloat / 100, startScalePaddle.y * e.myFloat / 100, 0);
        }
        currentTimerScaling = e.mySecondFloat;
    }

    public string GetPlayerName()
    {
        return isHittedBall;
    }

    public void OnSetHealPlayer(MyEventArgs e)
    {
        if (health > 0)
        {
            health += e.myFloat;
        }
    } 
}
