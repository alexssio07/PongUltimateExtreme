using System.Collections;
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
        EventManager.instance.AddListener(MyIndexEvent.spawnPowerUp, OnSpawnPowerUp);
        currentIndexTransform = 0;
        currentIndexGameObject = 0;
        minPercentage = powerUp.Min(p => p.GetComponent<PowerUp>().PowerUpData.GetProbabilySpawn);
        maxPercentage = powerUp.Max(p => p.GetComponent<PowerUp>().PowerUpData.GetProbabilySpawn);
    }

    public void OnSpawnPowerUp(MyEventArgs e)
    {
        System.Random random = new System.Random();
        int numberCasual = 0;
        int[] listNumberGenerated = new int[e.myInt];
        int number1 = random.Next(minPercentage, maxPercentage);
        int number2 = 0;
        for (int i = 0; i < listNumberGenerated.Length; i++)
        {
            number1 = random.Next(minPercentage, maxPercentage);
            while (number1 == number2)
            {
                number1 = random.Next(minPercentage, maxPercentage);
            }
            listNumberGenerated[i] = number1;
            number2 = number1;
        }


        for (int j = 0; j < powerUp.Length; j++)
        {
            PowerUp powerUpSelected = powerUp[j].GetComponent<PowerUp>();
            //int prova = listNumberGenerated.ToList().ForEach(n => );
            if (listNumberGenerated[j] >= powerUpSelected.PowerUpData.GetProbabilySpawn)
            {
                //spriteSelected = powerUpSelected.GetComponent<SpriteRenderer>().sprite;
                Instantiate(powerUpSelected, GetRandomPositionSpawn(), Quaternion.identity);
                //counterRandom++;
            }
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