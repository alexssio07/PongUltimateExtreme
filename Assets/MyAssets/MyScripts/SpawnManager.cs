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
    private PowerUp powerUpSelected;

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.spawnPowerUp, OnSpawnPowerUp);
        currentIndexTransform = 0;
        currentIndexGameObject = 0;
    }

    public void OnSpawnPowerUp(MyEventArgs e)
    {
        int countToSpawn = e.myInt;
        if (countToSpawn <= powerUp.Length)
        {
            for (int j = 0; j < countToSpawn; j++)
            {
                SpawnPowerUp(j);
            }
        }
        else
        {
            int difference = countToSpawn - powerUp.Length;
            for (int i = 0; i < powerUp.Length; i++)
            {
                SpawnPowerUp(i);
            }
            for (int k = 0; k < difference; k++)
            {
                SpawnPowerUp(k);
            }
        }

    }

    private void SpawnPowerUp(int index)
    {
        powerUpSelected = powerUp[index].GetComponent<PowerUp>();
        int randomNumber = Random.Range(1, 100);
        if (randomNumber >= powerUpSelected.PowerUpData.GetMinProbabilySpawn && randomNumber <= powerUpSelected.PowerUpData.GetMaxProbabilySpawn)
        {
            Debug.Log("SpawnPowerUp");
            Instantiate(powerUp[index], GetRandomPositionSpawn(), Quaternion.identity);
        }
    }


    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(MyIndexEvent.spawnPowerUp, OnSpawnPowerUp);
    }

    private Vector2 GetRandomPositionSpawn()
    {
        int randomNumber = Random.Range(0, points.Length);
        return points[randomNumber].position;
    }
}