using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    [NonSerialized]
    public MyEvent[] myEvents;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEvents();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEvents()
    {
        myEvents = new MyEvent[Enum.GetValues(typeof(MyIndexEvent)).Length];
        for (int i = 0; i < myEvents.Length; i++)
        {
            myEvents[i] = new MyEvent();
        }
    }

    public void CastEvent(MyIndexEvent eventToCast, MyEventArgs eventData)
    {
        myEvents[(int)eventToCast].Invoke(eventData);
    }

    public void AddListener(MyIndexEvent eventListener, UnityAction<MyEventArgs> call)
    {
        myEvents[(int)eventListener].AddListener(call);
    }

    public void RemoveListener(MyIndexEvent eventListener, UnityAction<MyEventArgs> call)
    {
        myEvents[(int)eventListener].RemoveListener(call);
    }

}

public class MyEvent : UnityEvent<MyEventArgs>
{

}

public class MyEventArgs
{
    public GameObject sender;
    public float myFloat;
    public float mySecondFloat;
    public int myInt;
    public int mySecondInt;
    public bool myBool;
    public bool mySecondBool;
    public string myString;

    public MyEventArgs()
    {
        this.sender = null;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.mySecondFloat = 0;
        this.myFloat = 0;
        this.myBool = false;
        this.mySecondBool = false;
        this.myString = string.Empty;
    }

    public MyEventArgs(GameObject sender)
    {
        this.sender = sender;
        this.myFloat = 0;
        this.mySecondFloat = 0;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myBool = false;
        this.mySecondBool = false;
        this.myString = string.Empty;
    }

    public MyEventArgs(GameObject sender, float myFloat)
    {
        this.sender = sender;
        this.myFloat = myFloat;
        this.mySecondFloat = 0;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myBool = false;
        this.mySecondBool = false;
        this.myString = string.Empty;
    }
    public MyEventArgs(GameObject sender, float myFloat, bool myBool)
    {
        this.sender = sender;
        this.myFloat = myFloat;
        this.mySecondFloat = 0;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myBool = myBool;
        this.mySecondBool = false;
        this.myString = string.Empty;
    }
    public MyEventArgs(GameObject sender, bool myBool, bool mySecondBool)
    {
        this.sender = sender;
        this.myBool = myBool;
        this.mySecondBool = mySecondBool;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myFloat = 0;
        this.mySecondFloat = 0;
        this.myString = string.Empty;
    }
    public MyEventArgs(GameObject sender, int myInt, bool myBool)
    {
        this.sender = sender;
        this.myBool = myBool;
        this.mySecondBool = false;
        this.myInt = myInt;
        this.mySecondInt = 0;
        this.myFloat = 0;
        this.mySecondFloat = 0;
        this.myString = string.Empty;
    }

    public MyEventArgs(GameObject sender, float myFloat, float mySecondFloat)
    {
        this.sender = sender;
        this.myBool = false;
        this.mySecondBool = false;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myFloat = myFloat;
        this.mySecondFloat = mySecondFloat;
        this.myString = string.Empty;
    }

    public MyEventArgs(GameObject sender, bool myBool, float myFloat, float mySecondFloat)
    {
        this.sender = sender;
        this.myBool = myBool;
        this.mySecondBool = false;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myFloat = myFloat;
        this.mySecondFloat = mySecondFloat;
        this.myString = string.Empty;
    }

    public MyEventArgs(GameObject sender, string myString, float myFloat, float mySecondFloat)
    {
        this.sender = sender;
        this.myBool = false;
        this.mySecondBool = false;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myFloat = myFloat;
        this.mySecondFloat = mySecondFloat;
        this.myString = myString;
    }

    public MyEventArgs(GameObject sender, string myString)
    {
        this.sender = sender;
        this.myBool = false;
        this.mySecondBool = false;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myFloat = 0;
        this.mySecondFloat = 0;
        this.myString = myString;
    }

    public MyEventArgs(GameObject sender, string myString, float myFloat, float mySecondFloat, bool myBool)
    {
        this.sender = sender;
        this.myBool = myBool;
        this.mySecondBool = false;
        this.myInt = 0;
        this.mySecondInt = 0;
        this.myFloat = myFloat;
        this.mySecondFloat = mySecondFloat;
        this.myString = myString;
    }
}

public enum MyIndexEvent
{
    playerHitted = 0,
    winner = 1,
    lost = 2,
    startToGame = 3,
    spawnPowerUp = 4,
    playerScored = 5,
    potion = 6,
    sword = 7,
    hammer = 8,
    bomb = 9,
    pill = 10,
    magnet = 11
}
