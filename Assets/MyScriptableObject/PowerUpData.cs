using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpData", menuName = "CreatePowerUpData", order = 6)]
public class PowerUpData : ScriptableObject
{
    [SerializeField]
    private float scaleValue;
    [SerializeField]
    private bool reduceScale;
    [SerializeField]
    private float countBalls;
    [SerializeField]
    private int speedPercentualPlayer;
    [SerializeField]
    private float speedPercentualBall;
    [SerializeField]
    private float duration;
    [SerializeField]
    [Range(1, 100)]
    private int probabilitySpawn;
    [SerializeField]
    private float damageValue;
    [SerializeField]
    private float healValue;

    public float GetScaleValue
    {
        get { return scaleValue; }
    }

    public float GetCountBalls
    {
        get { return countBalls;  }
    }

    public int GetSpeedPercentualPlayer
    {
        get { return speedPercentualPlayer;  }
    }

    public float GetSpeedPercentualBall
    {
        get { return speedPercentualBall;  }
    }

    public float GetDuration
    {
        get { return duration;   }
    }

    public int GetProbabilySpawn
    {
        get { return probabilitySpawn; }
    }

    public float GetDamageValue
    {
        get { return damageValue;  }
    }

    public float GetHealValue
    {
        get { return healValue; }
    }

    public bool GetReduceScale
    {
        get { return reduceScale; }
    }
}
