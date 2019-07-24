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

    private float currentTimer;

    private void Start()
    {
        Reset();
        EventManager.instance.AddListener(MyIndexEvent.playerScored, OnPlayerScore);
    }

    public void Reset()
    {
        scoreLeftPlayer = 0;
        scoreRightPlayer = 0;
        myBall.InitializeBall();
        currentTimer = timeToSpawn;
    }

    private void Update()
    {
        if (currentTimer > 0)
            currentTimer -= Time.deltaTime;
        else
        {
            currentTimer = waitingTime;
            EventManager.instance.CastEvent(MyIndexEvent.spawnPowerUp, new MyEventArgs { sender = gameObject });
        }
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

    public void SetHealthPlayer()
    {
        EventManager.instance.CastEvent(MyIndexEvent.playerHitted, new MyEventArgs() { sender = gameObject, myInt = healthLeftPlayer, mySecondInt = healthRightPlayer });
    }
}
