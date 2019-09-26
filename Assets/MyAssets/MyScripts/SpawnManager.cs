﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int minPercentage = 0;
    private int maxPercentage = 0;
    private Sprite spritesGenerated;

    private void Start()
    {
        EventManager.instance.AddListener(MyIndexEvent.spawnWeapon, OnSpawnPowerUp);
        currentIndexTransform = 0;
        currentIndexGameObject = 0;
        minPercentage = powerUp.Min(p => p.GetComponent<Weapon>().WeaponData.GetProbabilySpawn);
        maxPercentage = powerUp.Max(p => p.GetComponent<Weapon>().WeaponData.GetProbabilySpawn);
    }

    public void OnSpawnPowerUp(MyEventArgs e)
    {
        System.Random random = new System.Random();
        int numberCasual = 0;
        List<int> listNumberGenerated = new List<int>();
        int number1 = random.Next(minPercentage, maxPercentage);
        listNumberGenerated.Add(number1);
        for (int i = 0; i < e.myInt; i++)
        {
            number1 = random.Next(minPercentage, maxPercentage);
            while (listNumberGenerated.Contains(number1))
            {
                number1 = random.Next(minPercentage, maxPercentage);
            }
            listNumberGenerated.Add(number1);
        }


        for (int j = 0; j < powerUp.Length; j++)
        {
            Weapon powerUpSelected = powerUp[j].GetComponent<Weapon>();
            if (listNumberGenerated[j] >= powerUpSelected.WeaponData.GetProbabilySpawn)
            {
                Instantiate(powerUpSelected, GetRandomPositionSpawn(), Quaternion.identity);
            }
        }
    }
    private void OnDestroy()
    {
        EventManager.instance.RemoveListener(MyIndexEvent.spawnWeapon, OnSpawnPowerUp);
    }

    private Vector2 GetRandomPositionSpawn()
    {
        int randomNumber = Random.Range(0, points.Length);
        return points[randomNumber].position;
    }

}