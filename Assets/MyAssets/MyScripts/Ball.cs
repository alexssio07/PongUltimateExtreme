using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidbody;
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
    private Transform currentTransformToFollow;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.sword, OnSlowlyBall);
    }

    private void Update()
    {
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
        if (currentTimer > 0)
            currentTimer -= Time.deltaTime;
        else
            currentTimer = 0;
    }

    private void Shoot()
    {
        rigidbody.velocity = (Vector3.zero - transform.position).normalized * speed;
        isReadyToStart = false;
    }

    public void InitializeBall(Transform paddleTransform)
    {
        rigidbody.velocity = Vector2.zero;
        currentTransformToFollow = paddleTransform;
        isReadyToStart = true;
    }

    public void InitializeBall()
    {
        isReadyToStart = false;
        rigidbody.velocity = Vector2.zero;
        transform.position = Vector3.zero;
        int randomNumber = Random.Range(0, 2);
        StartCoroutine(PerformStartForce(randomNumber == 0 ? new Vector2(Random.Range(-10,-4), Random.Range(-10,4)) : new Vector2(Random.Range(10,4), Random.Range(10,4))));
    }

    private IEnumerator PerformStartForce(Vector2 force)
    {
        yield return new WaitForSeconds(3f);
        rigidbody.velocity = force * startSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            playerHitted = player.GetPlayerName();
            Vector2 velocity;       
            velocity.x = rigidbody.velocity.x;
            velocity.y = (rigidbody.velocity.y / 2f) + (collision.collider.attachedRigidbody.velocity.y / 2f);
            rigidbody.velocity = velocity;
            audioSource.PlayOneShot(sfxPaddle);
        }
        else if (collision.collider.gameObject.CompareTag("Y_Wall"))
        {
            audioSource.PlayOneShot(sfxWall);
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
                //case TypePowerUp.Bomb:
                //    EventManager.instance.CastEvent(MyIndexEvent.bomb, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetCountBalls));
                //    break;
                case TypePowerUp.Hammer:
                    EventManager.instance.CastEvent(MyIndexEvent.hammer, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetScaleValue, powerUpHitted.PowerUpData.GetDuration));
                    break;
                case TypePowerUp.Pill:
                    EventManager.instance.CastEvent(MyIndexEvent.pill, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetHealValue));
                    break;
                case TypePowerUp.Potion:
                    EventManager.instance.CastEvent(MyIndexEvent.potion, new MyEventArgs(gameObject, powerUpHitted.PowerUpData.GetSpeedPercentualPlayer, powerUpHitted.PowerUpData.GetDuration));
                    break;
            }
            powerUpHitted.Reset();
        }
    }

    public void OnSlowlyBall(MyEventArgs e)
    {
        speed = e.myFloat;
        currentTimer = e.mySecondFloat;
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
    }
}
