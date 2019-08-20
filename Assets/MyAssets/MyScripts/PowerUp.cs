using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpData PowerUpData;
    public TypePowerUp typePowerUp;

    [SerializeField]
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


    public void ResetPowerUp()
    {
        isActived = false;
        Debug.Log("Reset powerup");
        Destroy(this.gameObject);
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
    Rope,
    Slowly
}
