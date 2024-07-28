using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideEffect : MonoBehaviour
{
    [SerializeField] float m_lifeTime = 1f;
    void Start()
    {
        Destroy(this.gameObject,m_lifeTime);
    }

}
