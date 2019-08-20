using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private Text leftPlayerScoreText;
    [SerializeField]
    private Text rightPlayerScoreText;
    [SerializeField]
    private Text leftPlayerHealth;
    [SerializeField]
    private Text rightPlayerHealth;
    [SerializeField]
    private Image barLeftPlayer;
    [SerializeField]
    private Image barRightPlayer;
    [SerializeField]
    private Text textCountdown;

    private bool scoredPlayerLeft;
    private bool scoredPlayerRight;

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.playerHitted, SetHealthPlayer);
        EventManager.instance.AddListener(MyIndexEvent.startToGame, StartGame);
    }


    public void SetPlayerScore(int scorePlayerLeft, int scorePlayerRight)
    {
        this.leftPlayerScoreText.text = "Giocatore 1 :     " + scorePlayerLeft;
        this.rightPlayerScoreText.text = "Giocatore 2 :     " + scorePlayerRight;
    }

    public void SetHealthPlayer(MyEventArgs e)
    {
        barLeftPlayer.fillAmount = (barLeftPlayer.fillAmount / 100) * e.myInt;
        barRightPlayer.fillAmount = (barRightPlayer.fillAmount / 100) * e.mySecondInt;
        leftPlayerHealth.text = e.myInt.ToString() + " %";
        rightPlayerHealth.text = e.mySecondInt.ToString() + " %";
    }

    public void StartGame(MyEventArgs e)
    {
        if (e.myFloat > 0)
            textCountdown.text = e.myFloat.ToString("0");
        else
            textCountdown.gameObject.SetActive(false);
    }
}
