using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タンクを動かすコンポーネント
/// </summary>
public class TankController : MonoBehaviour
{
    [SerializeField] float m_moveSpeed = 1f;
    [SerializeField] float m_rotateSpeed = 1f;
    [SerializeField] Transform m_muzzle = null;
    [SerializeField] float m_maxFireDistance = 100f;
    ExplosiveObjectController m_explosive = null;
    Rigidbody m_rb = null;
    LineRenderer m_line = null;
    Vector3 m_rayCastHitPosition;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // レイキャストして「レーザーポインターがどこに当たっているか」を調べる
        Ray ray = new Ray(m_muzzle.position, this.transform.forward);   // muzzle から正面に ray を飛ばす
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            m_rayCastHitPosition = hit.point;
            m_explosive = hit.collider.gameObject.GetComponent<ExplosiveObjectController>();
        }
        else
        {
            m_rayCastHitPosition = m_muzzle.position + this.transform.forward * m_maxFireDistance;
            m_explosive = null;
        }

        // Line Renderer を使ってレーザーを描く
        m_line.SetPosition(0, m_muzzle.position);
        m_line.SetPosition(1, m_rayCastHitPosition);

        // クリックした時当たってたら爆発する
        if (Input.GetButtonDown("Fire1"))
        {
            Fire1();
        }

        // 以下、上下左右でタンクを動かす

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0)
        {
            this.transform.Rotate(Vector3.up, h * m_rotateSpeed);
        }

        if (v != 0)
        {
            m_rb.velocity = v * this.transform.forward * m_moveSpeed;
        }
    }

    void Fire1()
    {
        if (m_explosive)
        {
            Destroy(m_explosive.gameObject);
        }
    }
}
