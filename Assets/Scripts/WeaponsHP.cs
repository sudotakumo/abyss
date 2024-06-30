using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHP : MonoBehaviour
{
    [Header("‘Ì—Í")]
    public float _hp = 5f;
    [Header("–³“G”»’è")]
    public bool IsInvincible;
    [Header("¶Ž€”»’è")]
    public bool IsAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
    }
    public void Damage(float damage)
    {
        if (!IsInvincible)
        {
            if (IsAlive)
            {
                _hp -= damage;
                if (_hp < 0)
                {
                    _hp = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}