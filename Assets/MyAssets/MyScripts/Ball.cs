using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
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
    private string playerHitted;
    [SerializeField]
    private Vector3 positionOffsetPaddle;

    private bool isReadyToStart;
    private float currentTimer;
    private float vectorVelocityX;
    private float vectorVelocityY;
    private Transform currentTransformToFollow;
    private Vector3 currentPosition;


    private void Awake()
    {
        rbBall = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.sword, OnSlowlyBall);
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
        rbBall.velocity = force * startSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 positionBall = this.transform.position;
        float scalePaddleX = collision.transform.localScale.x / 2;
        float scalePaddleY = collision.transform.localScale.y / 2;


        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            playerHitted = player.GetPlayerName();
            if (positionBall.x >= scalePaddleX || positionBall.y >= scalePaddleY)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, -rbBall.velocity.y);
            }
            if (positionBall.x < scalePaddleX || positionBall.y < scalePaddleY)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, rbBall.velocity.y);
            }
            if (positionBall.x > scalePaddleX || positionBall.y < scalePaddleY)
            {
                rbBall.velocity = new Vector2(-rbBall.velocity.x, -rbBall.velocity.y);
            }
            if (positionBall.x < scalePaddleX || positionBall.y > scalePaddleY)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, rbBall.velocity.y);
            }
            if (positionBall.x == scalePaddleX || positionBall.y == scalePaddleY)
            {
                rbBall.velocity = new Vector2(-rbBall.velocity.x, -rbBall.velocity.y);
            }
        }
        else if (collision.collider.gameObject.CompareTag("Y_Wall"))
        {
            audioSource.PlayOneShot(sfxWall);
            float directionX = rbBall.velocity.x % 2;
            float directionY = rbBall.velocity.y % 2;
            if (directionX == 0f && directionY == 0f)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, -rbBall.velocity.y);
            }
            if (directionX == 0f && directionY == 1f)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, -rbBall.velocity.y);
            }
            if (directionX == 1f && directionY == 0f)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, -rbBall.velocity.y);
            }
            if (directionX == 1f && directionY == 1f)
            {
                rbBall.velocity = new Vector2(rbBall.velocity.x, -rbBall.velocity.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("X_Wall"))
        {
            EventManager.instance.CastEvent(MyIndexEvent.playerScored, new MyEventArgs() { sender = this.gameObject, myBool = transform.position.x > 0 });
        }
        PowerUp powerUpHitted = collision.GetComponent<PowerUp>();
        if (collision.CompareTag("PowerUp"))
        {
            switch (powerUpHitted.typePowerUp)
            {
                case TypePowerUp.Sword:
                    EventManager.instance.CastEvent(MyIndexEvent.sword, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetSpeedPercentualBall, powerUpHitted.PowerUpData.GetDuration));
                    break;
                case TypePowerUp.Bomb:
                    EventManager.instance.CastEvent(MyIndexEvent.bomb, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetCountBalls));
                    break;
                case TypePowerUp.Hammer:
                    EventManager.instance.CastEvent(MyIndexEvent.hammer, new MyEventArgs(gameObject, playerHitted, powerUpHitted.PowerUpData.GetScaleValue, powerUpHitted.PowerUpData.GetDuration, powerUpHitted.PowerUpData.GetReduceScale));
                    break;
                case TypePowerUp.Pill:
                    EventManager.instance.CastEvent(MyIndexEvent.pill, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetHealValue));
                    break;
                case TypePowerUp.Potion:
                    EventManager.instance.CastEvent(MyIndexEvent.potion, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetHealValue));
                    break;
                case TypePowerUp.Rope:
                    EventManager.instance.CastEvent(MyIndexEvent.rope, new MyEventArgs(gameObject, playerHitted));
                    break;
                case TypePowerUp.Slowly:
                    EventManager.instance.CastEvent(MyIndexEvent.slowly, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetSpeedPercentualPlayer, powerUpHitted.PowerUpData.GetDuration));
                    break;
            }
            powerUpHitted.ResetPowerUp();
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
        for (int i = 0; i < e.myInt; i++)
        {
            Vector2 direction = (this.transform.position - currentPosition).normalized;
            //Instantiate(this.gameObject, )
        }
    }

    public float CalculateAngle(Vector2 ball, Vector2 paddle, float paddleHeight)
    {
        return (ball.y - paddle.y) / paddleHeight;
    }
}
