using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    public static GUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GUIManager();
            }
            return _instance;
        }
    }

    [SerializeField]
    private GameObject barHealthPlayerLeft;
    [SerializeField]
    private GameObject barHealthPlayerRight;
    [SerializeField]
    private Text leftPlayerScoreText;
    [SerializeField]
    private Text rightPlayerScoreText;
    [SerializeField]
    private GameObject iconPlayerLeftRespawn;
    [SerializeField]
    private GameObject iconPlayerRightRespawn;
    [SerializeField]
    private Text textCountdown;

    private static GUIManager _instance;
    private Text textHealthPlayerLeft;
    private Text textHealthPlayerRight;
    private Image barLeftPlayer;
    private Image barRightPlayer;
    private Image barLeftPlayerRespawn;
    private Image barRightPlayerRespawn;

    private bool scoredPlayerLeft;
    private bool scoredPlayerRight;
    private float currentTimerPlayerLeftRespawn;
    private float currentTimerPlayerRightRespawn;

    private void Awake()
    {
        _instance = this;
        textHealthPlayerLeft = barHealthPlayerLeft.GetComponentInChildren<Text>();
        textHealthPlayerRight = barHealthPlayerRight.GetComponentInChildren<Text>();
        barLeftPlayer = barHealthPlayerRight.GetComponent<Image>();
        barRightPlayer = barHealthPlayerRight.GetComponent<Image>();
        barLeftPlayerRespawn = iconPlayerLeftRespawn.GetComponent<Image>();
        barRightPlayerRespawn = iconPlayerRightRespawn.GetComponent<Image>();
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.playerHitted, SetHealthPlayer);
        EventManager.instance.AddListener(MyIndexEvent.respawnPlayer, RespawnPlayer);
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
        textHealthPlayerLeft.text = e.myInt.ToString() + " %";
        textHealthPlayerRight.text = e.mySecondInt.ToString() + " %";
    }

    public void SetTextTimer(float time, bool isPhase1)
    {
        //Debug.Log(time);
        if (time > 0)
        {
            if (!isPhase1)
                textCountdown.text = string.Format("{0:#}", time);
            else
            {
                if (time >= 60)
                {
                    textCountdown.text = (int)(time / 60) + string.Format(":{0:#}", time);
                }
                else
                {
                    textCountdown.text = string.Format("0:{0:#}", time);
                }
                //textCountdown.gameObject.SetActive(true);

            }
        }
    }

    public void ActivateBarHealth()
    {
        barHealthPlayerLeft.SetActive(true);
        barHealthPlayerRight.SetActive(true);
    }

    public void RespawnPlayer(MyEventArgs e)
    {       
        if (e.namePlayer == NamePlayer.PlayerLeft)
        {
            barLeftPlayer.gameObject.SetActive(false);
            iconPlayerLeftRespawn.SetActive(true);
            currentTimerPlayerLeftRespawn = e.myFloat;
        }
        if (e.namePlayer == NamePlayer.PlayerRight)
        {
            currentTimerPlayerRightRespawn = e.myFloat;
            barRightPlayer.gameObject.SetActive(false);
            iconPlayerRightRespawn.SetActive(true);
        }
        DisableRespawn(e.myFloat, e.namePlayer);
    }

    private IEnumerator DisableRespawn(float timeToRespawn, NamePlayer namePlayer)
    {
        yield return new WaitForSeconds(timeToRespawn);
        if (namePlayer == NamePlayer.PlayerLeft)
        {
            barLeftPlayer.gameObject.SetActive(true);
            iconPlayerLeftRespawn.SetActive(false);
        }
        if (namePlayer == NamePlayer.PlayerRight)
        {
            barRightPlayer.gameObject.SetActive(true);
            iconPlayerRightRespawn.SetActive(false);
        }
        EventManager.instance.CastEvent(MyIndexEvent.spawnPlayer, new MyEventArgs() { sender = gameObject, namePlayer = namePlayer });
    }
}
