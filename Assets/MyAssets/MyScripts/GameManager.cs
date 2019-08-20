using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Ball myBall;
    [SerializeField]
    private Transform leftPaddleTransform;
    [SerializeField]
    private Transform rightPaddleTransform;
    [SerializeField]
    private int scoreRightPlayer;
    [SerializeField]
    private int scoreLeftPlayer;
    [SerializeField]
    private int healthLeftPlayer;
    [SerializeField]
    private int healthRightPlayer;
    [SerializeField]
    private int countSpawnPowerUp;
    [SerializeField]
    private float timeToSpawn;
    [SerializeField]
    private float waitingTime;
    [SerializeField]
    private GUIManager guiManager;

    private float currentTimerStartGame;
    private float currentTimerSpawn;
    private bool gameStarted;


    private void Start()
    {
        gameStarted = false;
        currentTimerStartGame = waitingTime;
        EventManager.instance.AddListener(MyIndexEvent.playerScored, OnPlayerScore);
        EventManager.instance.AddListener(MyIndexEvent.rope, OnEngagesBall);
    }


    private void Update()
    {
        if (gameStarted)
        {
            if (currentTimerSpawn > 0)
                currentTimerSpawn -= Time.deltaTime;
            else
            {
                currentTimerSpawn = timeToSpawn;
                EventManager.instance.CastEvent(MyIndexEvent.spawnPowerUp, new MyEventArgs { sender = gameObject, myInt = countSpawnPowerUp });
            }
        }
        else 
        {
            if (currentTimerStartGame > 0)
            {
                currentTimerStartGame -= Time.deltaTime;
                EventManager.instance.CastEvent(MyIndexEvent.startToGame, new MyEventArgs { sender = gameObject, myFloat = currentTimerStartGame });
            }
            else
            {
                currentTimerStartGame = waitingTime;
                EventManager.instance.CastEvent(MyIndexEvent.startToGame, new MyEventArgs { sender = gameObject, myFloat = currentTimerStartGame });
                ResetBall();
            }
        }
    }

    public void ResetBall()
    {
        scoreLeftPlayer = 0;
        scoreRightPlayer = 0;
        myBall.InitializeBall();
        gameStarted = true;
        currentTimerSpawn = timeToSpawn;
    }

    public void OnPlayerScore(MyEventArgs e)
    {
        if (e.myBool)
        {
            scoreLeftPlayer++;
            myBall.InitializeBall(rightPaddleTransform);
        }
        else
        {
            scoreRightPlayer++;
            myBall.InitializeBall(leftPaddleTransform);
        }
        guiManager.SetPlayerScore(scoreLeftPlayer, scoreRightPlayer);
    }

    //public void SetHealthPlayer()
    //{
    //    EventManager.instance.CastEvent(MyIndexEvent.playerHitted, new MyEventArgs() { sender = gameObject, myInt = healthLeftPlayer, mySecondInt = healthRightPlayer });
    //}

    public void OnEngagesBall(MyEventArgs e)
    {
        if (e.myString == "LeftPlayer")
        {
            myBall.InitializeBall(leftPaddleTransform);
        }
        else if (e.myString == "RightPlayer")
        {
            myBall.InitializeBall(rightPaddleTransform);
        }
    }
}
