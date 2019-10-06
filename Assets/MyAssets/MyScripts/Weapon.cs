using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData;
    public TypeWeapon typeWeapon;

    public const float PositioningWeapon = 0.7f;
    public const float RotationWeaponLeft = -50f;
    public const float RotationWeaponRight = 132f;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float speed;
    private float currentTimeDestruction;
    private Rigidbody2D rb;
    private bool isActived;
    private bool wasFired;
    private NamePlayer playerNameTaked;
    private Collider2D col;

    private const float positionReset = 6.5f;
    private const float boundY = -5.9f;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        col = this.GetComponent<Collider2D>();
        rb.isKinematic = true;
        isActived = false;
        playerNameTaked = NamePlayer.None;
    }

    private void Update()
    {
        if (isActived)
        {
            Vector2 direction = speed * (-Vector2.up);
            rb.velocity = direction;
            if (this.transform.position.y <= boundY)
            {
                ResetWeapon();
            }
        }
        if (currentTimeDestruction > 0 && isActived)
            currentTimeDestruction -= Time.deltaTime;
        else
        {
            currentTimeDestruction = 0;
            EventManager.instance.CastEvent(MyIndexEvent.playerHitted, new MyEventArgs() { sender = gameObject, myFloat = WeaponData.GetDamageValue, namePlayer = playerNameTaked });
            ResetWeapon();
        }
        if (playerNameTaked != NamePlayer.None)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !wasFired)
            {
                rb.velocity = (Vector3.zero - transform.position).normalized * WeaponData.GetSpeedFiring;
                wasFired = true;
                col.isTrigger = false;
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("Actived Weapon");
        isActived = true;
        rb.isKinematic = false;
    }

    private void OnTriggerEnter(Collider objectHitted)
    {
        if (objectHitted.gameObject.CompareTag("Y_Wall") || objectHitted.gameObject.CompareTag("X_Wall"))
        {
            objectHitted.gameObject.SetActive(false);
            objectHitted.transform.position = Vector3.zero;
        }
        if (objectHitted.gameObject.CompareTag("Ball"))
        {
            Ball playerHitted = objectHitted.GetComponent<Ball>();
            playerNameTaked = playerHitted.PlayerHitted;
            if (typeWeapon == TypeWeapon.Bomb)
                ActivateAutoDestruction(WeaponData.GetTimeAutoDestruction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.instance.CastEvent(MyIndexEvent.playerHitted, new MyEventArgs() { sender = gameObject, myFloat = WeaponData.GetDamageValue, namePlayer = playerNameTaked });
        }
        Destroy(this.gameObject);
    }

    public void ActivateAutoDestruction(float timerToDestroy)
    {
        rb.isKinematic = true;
        currentTimeDestruction = timerToDestroy;
        // ACTIVATE GUI AUTO DESTRUCTION
    }


    public void ResetWeapon()
    {
        Debug.Log("Reset Weapon");
        isActived = false;
        this.gameObject.SetActive(isActived);
        //this.transform.position = new Vector3(this.transform.position.x, positionReset, this.transform.position.z);
        Destroy(this.gameObject, 1f);
    }
}

public enum TypeWeapon
{
    None,
    Potion,
    Sword,
    Hammer,
    Bomb,
    Pill,
    Magnet,
    Slowly
}
