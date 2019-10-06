using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public NamePlayer PlayerHitted;

    [SerializeField]
    private Rigidbody2D rbBall;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float startSpeed;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private AudioClip sfxWall, sfxPaddle, sfxScore;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private GameManager gameController;
    [SerializeField]
    private Vector3 positionOffsetPaddle;

    private bool isReadyToStart;
    private float currentTimer;
    private float vectorVelocityX;
    private float vectorVelocityY;
    private Transform currentTransformToFollow;
    private Vector3 currentPosition;
    private Vector3 scalePlayer;

    private List<Player> players;

    private void Awake()
    {
        rbBall = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.sword, OnSlowlyBall);
        players = FindObjectsOfType<Player>().ToList();
        scalePlayer = players.Select(p => p.transform.localScale).FirstOrDefault();
    }

    private void Update()
    {
        currentPosition = this.transform.position;
        if (isReadyToStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Shoot();
            }
            else
            {
                transform.position = transform.position.x > 0 ? currentTransformToFollow.position - positionOffsetPaddle : currentTransformToFollow.position + positionOffsetPaddle;
            }
        }
        else
        {
            vectorVelocityX = this.transform.position.x + rbBall.velocity.x;
            vectorVelocityY = this.transform.position.y + rbBall.velocity.y;
        }

        if (currentTimer > 0)
            currentTimer -= Time.deltaTime;
        else
            currentTimer = 0;
    }

    private void Shoot()
    {
        rbBall.velocity = (Vector3.zero - transform.position).normalized * speed;
        isReadyToStart = false;
    }

    public void InitializeBall(Transform paddleTransform)
    {
        rbBall.velocity = Vector2.zero;
        currentTransformToFollow = paddleTransform;
        isReadyToStart = true;
    }

    public void InitializeBall()
    {
        isReadyToStart = false;
        rbBall.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        int randomNumber = Random.Range(0, 2);
        StartCoroutine(PerformStartForce(randomNumber == 0 ? new Vector2(Random.Range(-10, -4), Random.Range(-10, 4)) : new Vector2(Random.Range(10, 4), Random.Range(10, 4))));
    }

    private IEnumerator PerformStartForce(Vector2 force)
    {
        yield return new WaitForSeconds(3f);
        rbBall.velocity = force.normalized * startSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 positionBall = this.transform.position;
        float scalePaddleX = collision.transform.localScale.x;
        float scalePaddleY = collision.transform.localScale.y;

        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            PlayerHitted = player.NamePlayer;
            Debug.Log("giocatore che ha colpito la palla: " + PlayerHitted);
            if (positionBall.y > scalePaddleY + positionBall.y && scalePaddleY + positionBall.y < positionBall.y)
            {
                rbBall.velocity = new Vector2(1, -1);
            }
        }
        else if (collision.collider.gameObject.CompareTag("Y_Wall"))
        {
            audioSource.PlayOneShot(sfxWall);
            float directionX = collision.transform.localScale.x;
            float directionY = collision.transform.localScale.y;
            if (positionBall.y >= directionY + positionBall.y && directionY + positionBall.y <= positionBall.y)
            {
                rbBall.velocity = new Vector2(1, -1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("X_Wall"))
        {
            EventManager.instance.CastEvent(MyIndexEvent.playerScored, new MyEventArgs() { sender = this.gameObject, myBool = transform.position.x > 0 });
        }
        Weapon powerUpHitted = collision.GetComponent<Weapon>();
        Debug.Log(powerUpHitted.typeWeapon);
        if (collision.CompareTag("Weapon"))
        {
            Vector3 positionPlayer = players.Where(p => p.NamePlayer == NamePlayer.PlayerLeft).FirstOrDefault().transform.position;
            switch (powerUpHitted.typeWeapon)
            {
                case TypeWeapon.Sword:
                    if (PlayerHitted == NamePlayer.PlayerLeft)
                    {
                        collision.gameObject.transform.position = new Vector3(positionPlayer.x + scalePlayer.x, positionPlayer.y, positionPlayer.z);
                        collision.gameObject.transform.rotation = new Quaternion(0, 0, -50, 0);
                    }
                    else if (PlayerHitted == NamePlayer.PlayerRight)
                    {
                        collision.gameObject.transform.position = new Vector3(positionPlayer.x - scalePlayer.x, positionPlayer.y, positionPlayer.z);
                        collision.gameObject.transform.rotation = new Quaternion(0, 0, 132, 0);
                    }
                    break;
                case TypeWeapon.Bomb:
                    if (PlayerHitted == NamePlayer.PlayerLeft)
                    {
                        collision.gameObject.transform.position = new Vector3(positionPlayer.x + scalePlayer.x, positionPlayer.y, positionPlayer.z);
                        collision.gameObject.transform.rotation = new Quaternion(0, 0, -50, 0);
                    }
                    else if (PlayerHitted == NamePlayer.PlayerRight)
                    {
                        collision.gameObject.transform.position = new Vector3(positionPlayer.x - scalePlayer.x, positionPlayer.y, positionPlayer.z);
                        collision.gameObject.transform.rotation = new Quaternion(0, 0, 132, 0);
                    }

                    //EventManager.instance.CastEvent(MyIndexEvent.bomb, new MyEventArgs(gameObject, powerUpHitted.WeaponData.GetCountBalls));
                    break;
                case TypeWeapon.Hammer:
                    //EventManager.instance.CastEvent(MyIndexEvent.hammer, new MyEventArgs(gameObject, playerHitted, powerUpHitted.WeaponData.GetScaleValue, powerUpHitted.WeaponData.GetDuration, powerUpHitted.WeaponData.GetReduceScale));
                    break;
                case TypeWeapon.Pill:
                    //EventManager.instance.CastEvent(MyIndexEvent.pill, new MyEventArgs(gameObject, powerUpHitted.WeaponData.GetHealValue));
                    powerUpHitted.ResetWeapon();
                    break;
                case TypeWeapon.Potion:
                    //EventManager.instance.CastEvent(MyIndexEvent.potion, new MyEventArgs(gameObject, powerUpHitted.WeaponData.GetSpeedPercentualPlayer, powerUpHitted.WeaponData.GetDuration));
                    powerUpHitted.ResetWeapon(); 
                    break;
                case TypeWeapon.Magnet:
                    //EventManager.instance.CastEvent(MyIndexEvent.magnet, new MyEventArgs(gameObject, playerHitted));
                    powerUpHitted.ResetWeapon();
                    break;
            }

        }
    }

    public void OnSlowlyBall(MyEventArgs e)
    {
        speed = e.myFloat;
        currentTimer = e.mySecondFloat;
        rbBall.velocity = rbBall.velocity.normalized * speed;
    }

    public void DuplicateBall(MyEventArgs e)
    {
        GameObject ballSpawned = this.gameObject;
        for (int i = 0; i < e.myInt; i++)
        {
            Vector2 direction = (this.transform.position - currentPosition).normalized;

            ballSpawned = Instantiate(ballSpawned, new Vector3(ballSpawned.transform.position.x + ballSpawned.transform.localScale.x + direction.x, ballSpawned.transform.position.y + ballSpawned.transform.localScale.y + direction.y), Quaternion.identity);
        }
    }

    public float CalculateAngle(Vector2 ball, Vector2 paddle, float paddleHeight)
    {
        return (ball.y - paddle.y) / paddleHeight;
    }
}
