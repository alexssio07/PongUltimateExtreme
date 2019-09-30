using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData;
    public TypeWeapon typeWeapon;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float speed;

    private Rigidbody2D rb;
    private bool isActived;

    private const float positionReset = 6.5f;
    private const float boundY = -5.9f;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        isActived = false;
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
