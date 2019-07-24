using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpData PowerUpData;

    [SerializeField]
    public TypePowerUp typePowerUp;
    [SerializeField]
    private float probabilitySpawn;
    [SerializeField]
    private float speed;

    private Rigidbody2D rigidbody;
    private bool isActived = false;

    private const float positionReset = 6.5f;
    private const float boundY = 5.9f;

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isActived)
        {
            Vector2 direction = speed * (-Vector2.up);
            rigidbody.velocity = direction;
            if (this.transform.position.y < -boundY)
            {
                Reset();
            }
        }
    }

    private void OnEnable()
    {
        isActived = true;
    }

    private void OnTriggerEnter(Collider objectHitted)
    {
        if (objectHitted.gameObject.CompareTag("Y_Wall") || objectHitted.gameObject.CompareTag("X_Wall"))
        {
            objectHitted.gameObject.SetActive(false);
            objectHitted.transform.position = Vector3.zero;
        }
    }

    public void Reset()
    {
        isActived = false;
        this.gameObject.SetActive(isActived);
        this.transform.position = new Vector3(this.transform.position.x, positionReset, this.transform.position.z);
    }
}

public enum TypePowerUp
{
    None,
    Potion,
    Sword,
    Hammer,
    Bomb,
    Pill
}
