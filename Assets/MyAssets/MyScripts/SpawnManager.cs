using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private GameObject[] powerUp;
    [SerializeField]
    private float startTime;
    [SerializeField]
    private float cooldown;

    [SerializeField]
    private float speed;

    private int currentIndexTransform;
    private int currentIndexGameObject;

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.spawnPowerUp, OnSpawnPowerUp);
        currentIndexTransform = 0;
        currentIndexGameObject = 0;
    }

    public void OnSpawnPowerUp(MyEventArgs e)
    {
        if (currentIndexGameObject < powerUp.Length)
        {
            if (currentIndexGameObject > 0) { }
 

            if (!powerUp[currentIndexGameObject].activeInHierarchy)
            {
                //PowerUp powerUpSelected = powerUp[currentIndexGameObject].GetComponent<PowerUp>();
                powerUp[currentIndexGameObject].SetActive(true);
                currentIndexGameObject++;
             }
        }
        else
        {
            currentIndexGameObject = 0;
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(MyIndexEvent.spawnPowerUp, OnSpawnPowerUp);
    }
}
