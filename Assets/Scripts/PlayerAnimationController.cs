using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator m_anim = null;
    Rigidbody m_rb = null;
    bool m_isAttacking;

    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_anim.SetTrigger("Attack");
            BeginAttack();
        }

        if (m_isAttacking)
        {
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }
    }

    void LateUpdate()
    {
        Vector2 horizontalVelocity = new Vector2(m_rb.velocity.x, m_rb.velocity.z);
        m_anim.SetFloat("RunSpeed", horizontalVelocity.magnitude);
        m_anim.SetFloat("FallSpeed", m_rb.velocity.y);
    }

    void BeginAttack()
    {
        m_isAttacking = true;
    }
}
