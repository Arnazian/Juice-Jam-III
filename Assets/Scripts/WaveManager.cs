using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private GameObject firstBoss;
    [SerializeField] private GameObject[] spawnPoints;

    [SerializeField] private Vector2 enemySpawnRange;
    [SerializeField] private List<WeightedGameObjectList> enemies;

    [Header("Debug, Remove Later")]
    public bool startOnAwake = true;
    
    private int _currentWaveNumber = 0;

    private int _enemiesKilled = 0;

    [SerializeField]private int _enemyCount = 5;

    public Action onBossStart;

    protected override void Awake()
    {
        base.Awake();
        if(startOnAwake)
            StartNextWave();
    }

    public void StartNextWave()
    {
        _enemiesKilled = 0;
        if (_currentWaveNumber >= enemies.Count)
        {
            onBossStart?.Invoke();
            StartFirstBoss();
            return;
        }

        _currentWaveNumber++;
        var weightedList = enemies[_currentWaveNumber - 1];
        for (int i = 0; i < _enemyCount; i++)
        {
            var enemy = Instantiate(weightedList.GetRandomObject(),
                new Vector3(Random.Range(-enemySpawnRange.x, enemySpawnRange.x),
                    Random.Range(-enemySpawnRange.y, enemySpawnRange.y), 0), Quaternion.identity);
        }
    }

    private void StartFirstBoss()
    {
        var bossHealthBar = UIManager.Instance.GetBossHealthBar;
        bossHealthBar.gameObject.SetActive(true);
        var boss = Instantiate(firstBoss, Vector3.zero, Quaternion.identity);
    }

    public void OnDeath()
    {
        _enemiesKilled++;
        if (_enemiesKilled >= _enemyCount)
            StartNextWave();
    }
}
