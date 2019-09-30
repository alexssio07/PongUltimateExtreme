using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private float timerSecPhase1;
    [SerializeField]
    private float timeMinWeaponSpawn;
    [SerializeField]
    private float timeMaxWeaponSpawn;
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
    private int countSpawnWeapon;

    private float currentTimerStartGame;
    private float currentTimerSpawn;
    private Phase phase;
    private static float preTimeStartGame = 3f;


    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.playerScored, OnPlayerScore);
        EventManager.instance.AddListener(MyIndexEvent.magnet, OnEngagesBall);
        phase = Phase.Zero;
        currentTimerStartGame = preTimeStartGame;
        currentTimerSpawn = Random.Range(timeMinWeaponSpawn, timeMaxWeaponSpawn);
    }


    private void Update()
    {
        switch (phase)
        {
            case Phase.Zero:
                GUIManager.Instance.SetTextTimer(currentTimerStartGame, false);
                if (currentTimerStartGame > 0)
                    currentTimerStartGame -= Time.deltaTime;
                else
                {
                    currentTimerStartGame = 0;
                    StartGame();
                }
                break;
            case Phase.Uno:
                GUIManager.Instance.SetTextTimer(Time.realtimeSinceStartup, true);
                if (Time.realtimeSinceStartup >= timerSecPhase1)
                    StartWar();
                break;
            case Phase.Due:
                if (currentTimerSpawn > 0)
                    currentTimerSpawn -= Time.deltaTime;
                else
                {
                    currentTimerSpawn = Random.Range(timeMinWeaponSpawn, timeMaxWeaponSpawn);
                    EventManager.instance.CastEvent(MyIndexEvent.spawnWeapon, new MyEventArgs() { sender = gameObject, myInt = countSpawnWeapon });
                }
                break;
        }
    }

    public void StartGame()
    {
        phase = Phase.Uno;
        scoreLeftPlayer = 0;
        scoreRightPlayer = 0;
        currentTimerStartGame = timerSecPhase1;
        myBall.InitializeBall();
    }

    public void StartWar()
    {
        phase = Phase.Due;
        currentTimerStartGame = 0;
        GUIManager.Instance.ActivateBarHealth();
        EventManager.instance.CastEvent(MyIndexEvent.playerHitted, new MyEventArgs() { sender = gameObject, myFloat = healthLeftPlayer, mySecondFloat = healthRightPlayer });
        EventManager.instance.CastEvent(MyIndexEvent.spawnWeapon, new MyEventArgs() { sender = gameObject, myInt = countSpawnWeapon });
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
        GUIManager.Instance.SetPlayerScore(scoreLeftPlayer, scoreRightPlayer);
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

public enum Phase
{
    Zero,
    Uno,
    Due,
    Tre
}