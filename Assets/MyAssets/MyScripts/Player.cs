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
    private float tempScaleX;
    private float tempScaleY;
    private float tempScaleZ;
    private bool isSlowed;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        isSlowed = false;
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.playerHitted, OnSetHealthPlayer);
        EventManager.instance.AddListener(MyIndexEvent.potion, OnSetSpeedPlayer);
        EventManager.instance.AddListener(MyIndexEvent.hammer, OnSetScalePaddle);
        EventManager.instance.AddListener(MyIndexEvent.pill, OnSetHealthPlayer);
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
            this.transform.localScale = new Vector3(tempScaleX, tempScaleY, tempScaleZ);
        }

    }

    public void OnSetHealthPlayer(MyEventArgs e)
    {
        Debug.Log("OnSetHealthPlayer");
        health -= e.myFloat;
        if (health <= 0)
        {
            health = 0;
            // TODO Gameover
        }
    }

    public void OnSetSpeedPlayer(MyEventArgs e)
    {
        Debug.Log("OnSetSpeedPlayer");
        isSlowed = true;
        currentTimerSlowing = e.mySecondFloat;
        tempSpeed = speedPaddle;
        speedPaddle = speedPaddle - (speedPaddle * e.myFloat / 100); 
    }

    public void OnSetScalePaddle(MyEventArgs e)
    {
        // TODO Implementare duration per lo scale
        // TODO Implementare scaling sul giocatore avversario... (?)
        Debug.Log("SetScalePaddle");
        Vector3 currentScale = this.transform.localScale;
        if (e.myBool)
            this.transform.localScale += new Vector3(currentScale.x * e.myFloat / 100, currentScale.y * e.myFloat / 100, 0);
        else
            this.transform.localScale -= new Vector3(currentScale.x * e.myFloat / 100, currentScale.y * e.myFloat / 100, 0);
    }

    public string GetPlayerName ()
    {
        return isHittedBall;
    }
}
