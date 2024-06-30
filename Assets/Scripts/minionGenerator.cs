using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionGenerator : MonoBehaviour
{
    [Header("敵のPrefab")]
    [SerializeField] private GameObject[] _minionObjects;
    [Header("スポーンのインターバル")]
    public float _interval = 1;
    public float _timer = 0;
    [Header("スポーンの中心点")]
    [SerializeField] private Vector2 _spawnFieldCentorPoint;
    [Header("スポーンの幅")]
    [SerializeField] private Vector2 _spawnField;
    public Transform _playerTransform;
    public SpawnArea spawnArea;
    [Header("インターバルを短くするまでの時間")]
    [SerializeField] float _spawnTime = 5;
    [SerializeField] float _plusSpawnTime = 5;
    [SerializeField] float _minusInterval = 0.3f;
    [SerializeField] float _minusAttackInterval = 0.1f;
    [SerializeField] float _intervalLine = 0.3f;
    [SerializeField] float _attackIntervalLine = 1;
    public enum SpawnArea
    {
        up, down, left, right
    }
    Vector2 spawnPoint;
    private void MinionSpawn()
    {
        System.Array values = System.Enum.GetValues(typeof(SpawnArea));
        int randomIndex = Random.Range(0, values.Length);
        spawnArea = (SpawnArea)values.GetValue(randomIndex);
        if (_playerTransform != null)
        {
            if (spawnArea == SpawnArea.up || spawnArea == SpawnArea.down)
            {
                spawnPoint.x = Random.Range(_playerTransform.position.x - _spawnField.x / 2, _playerTransform.position.x + _spawnField.x / 2);
                spawnPoint.y = Random.Range(_playerTransform.position.y - _spawnField.y / 2, _playerTransform.position.y + _spawnField.y / 2);
            }
            else if (spawnArea == SpawnArea.right || spawnArea == SpawnArea.left)
            {
                spawnPoint.y = Random.Range(_playerTransform.position.y - _spawnField.y / 2, _playerTransform.position.y + _spawnField.y / 2);
                spawnPoint.x = Random.Range(_playerTransform.position.x - _spawnField.x / 2, _playerTransform.position.x + _spawnField.x / 2);
            }
        }
        int randomMinionIndex = Random.Range(0, _minionObjects.Length);
        if (_playerTransform != null)
        {

            Instantiate(_minionObjects[randomMinionIndex], spawnPoint, Quaternion.identity);
        }
    }
    private void Update()
    {
        if (WeaponsWarTimeManager._timer >= _spawnTime)
        {
            if (_interval >= _intervalLine)
            {
                _spawnTime += _plusSpawnTime;
                _interval -= _minusInterval;
                BackMinion._interval -= _minusAttackInterval;
                FrontMinion._interval -= _minusAttackInterval;
                if (_interval <= _intervalLine)
                {
                    _interval = _intervalLine;
                }
                if (BackMinion._interval <= _attackIntervalLine)
                {
                    BackMinion._interval = _attackIntervalLine;
                }
                if (FrontMinion._interval <= _attackIntervalLine)
                {
                    FrontMinion._interval = _attackIntervalLine;
                }
            }
        }
        _timer += Time.deltaTime;
        if (_timer >= _interval)
        {
            MinionSpawn();
            _timer = 0;
        }
    }
}