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
        EventManager.instance.AddListener(MyIndexEvent.spawnWeapon, OnSpawnPowerUp);
        currentIndexTransform = 0;
        currentIndexGameObject = 0;
    }

    public void OnSpawnPowerUp(MyEventArgs e)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < e.myInt; i++)
        {
            int indexRandom = random.Next(0, powerUp.Length);
            Instantiate(powerUp[indexRandom], GetRandomPositionSpawn(), Quaternion.identity);
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