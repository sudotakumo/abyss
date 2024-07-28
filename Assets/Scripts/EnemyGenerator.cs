using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�𐶐�����B���̃I�u�W�F�N�g������ꏊ����G�𐶐�����B
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject m_enemyPrefab;
    [SerializeField, Range(0.1f, 1f)] float m_interval = 0.25f;
    float m_timer;

    void Update()
    {
        // �Z�b�����ɏ��������s��������B���̃p�^�[���͎g����悤�ɂȂ��Ă������ƁB
        m_timer += Time.deltaTime;

        if (m_timer > m_interval)
        {
            m_timer = 0;
            Instantiate(m_enemyPrefab, this.gameObject.transform.position, m_enemyPrefab.transform.rotation);
        }
    }
}
