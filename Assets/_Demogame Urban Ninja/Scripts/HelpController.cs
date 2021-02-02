using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    [SerializeField] float m_interval = 5f;
    float m_timer = 0;
    bool m_isIdle = false;
    Animator m_anim = null;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || h != 0f || v != 0f)
        {
            if (m_isIdle)
            {
                m_timer = 0f;
                m_isIdle = false;
                m_anim.Play("Hide");
                Debug.Log("Active");
            }
        }
        else
        {
            m_timer += Time.deltaTime;
        }

        if (m_timer > m_interval)
        {
            if (!m_isIdle)
            {
                m_isIdle = true;
                m_anim.Play("Show");
                Debug.Log("Idle");
            }
        }
    }
}
