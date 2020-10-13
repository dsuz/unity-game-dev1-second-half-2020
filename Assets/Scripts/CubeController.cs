using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CubeController : MonoBehaviour
{
    [SerializeField] float m_movePower = 1f;
    [SerializeField] float m_playerDetectDistance = 3f;
    Transform m_player = null;
    bool m_isPlayerFound = false;
    Animator m_anim = null;
    Rigidbody m_rb = null;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (!m_player)
        {
            Debug.LogWarning("Player タグが設定されたオブジェクトが見つかりませんでした。");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, m_player.position);

        if (distance <= m_playerDetectDistance && !m_isPlayerFound)
        {
            m_isPlayerFound = true;

            if (m_anim)
            {
                m_anim.Play("DetectPlayer");
            }
        }
        else if (distance > m_playerDetectDistance && m_isPlayerFound)
        {
            m_isPlayerFound = false;

            if (m_anim)
            {
                m_anim.Play("Default");
            }
        }
    }

    void FixedUpdate()
    {
        if (m_isPlayerFound)
        {
            Vector3 dir = m_player.position - this.transform.position;
            dir = dir.normalized * m_movePower;
            m_rb.AddForce(dir);
        }
    }
}
