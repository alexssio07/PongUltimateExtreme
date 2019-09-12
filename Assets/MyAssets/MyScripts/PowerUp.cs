using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpData PowerUpData;
    public TypePowerUp typePowerUp;

    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float speed;

    private Rigidbody2D rigidbody;
    private bool isActived;

    private const float positionReset = 6.5f;
    private const float boundY = -5.9f;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        isActived = false;
    }

    private void Update()
    {
        if (isActived)
        {
            Vector2 direction = speed * (-Vector2.up);
            rigidbody.velocity = direction;
            if (this.transform.position.y <= boundY)
            {
                ResetPowerUp();
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("Actived powerup");
        isActived = true;
        rigidbody.isKinematic = false;
    }

    private void OnTriggerEnter(Collider objectHitted)
    {
        if (objectHitted.gameObject.CompareTag("Y_Wall") || objectHitted.gameObject.CompareTag("X_Wall"))
        {
            objectHitted.gameObject.SetActive(false);
            objectHitted.transform.position = Vector3.zero;
        }
    }


    public void ResetPowerUp()
    {
        Debug.Log("Reset powerup");
        isActived = false;
        this.gameObject.SetActive(isActived);
        //this.transform.position = new Vector3(this.transform.position.x, positionReset, this.transform.position.z);
        Destroy(this.gameObject, 1f);
    }
}

public enum TypePowerUp
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
