using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を生成する。このオブジェクトがある場所から敵を生成する。
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject m_enemyPrefab;
    [SerializeField, Range(0.1f, 1f)] float m_interval = 0.25f;
    float m_timer;

    void Update()
    {
        // 〇秒おきに処理を実行するやり方。このパターンは使えるようになっておくこと。
        m_timer += Time.deltaTime;

        if (m_timer > m_interval)
        {
            m_timer = 0;
            Instantiate(m_enemyPrefab, this.gameObject.transform.position, m_enemyPrefab.transform.rotation);
        }
    }
}
