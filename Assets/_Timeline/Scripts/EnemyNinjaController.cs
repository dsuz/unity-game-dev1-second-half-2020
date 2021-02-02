using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の挙動をコントロールするコンポーネント
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class EnemyNinjaController : MonoBehaviour
{
    /// <summary>動く時にかける力</summary>
    [SerializeField] float m_movePower = 30f;
    /// <summary>最大速度</summary>
    [SerializeField] float m_maxSpeed = 4f;
    /// <summary>消える時に残すエフェクトのプレハブ</summary>
    [SerializeField] GameObject m_destroyEffectPrefab = null;
    Rigidbody m_rb = null;
    
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // プレイヤーの方に移動させる
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player)
        {
            Vector3 dir = player.transform.position - this.transform.position;
            dir.y = 0;  // 上下方向は無視する
            this.transform.forward = dir;

            if (m_rb.velocity.magnitude < m_maxSpeed)
            {
                m_rb.AddForce(this.transform.forward * m_movePower);
            }
        }
        else
        {
            // プレイヤーが居なくなったら消える
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 手裏剣に当たったら破棄する
        if (other.CompareTag("ShurikenTag"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        // 破棄される時にエフェクトを生成する
        if (m_destroyEffectPrefab)
        {
            Instantiate(m_destroyEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnApplicationQuit()
    {
        // これをしないと停止時にうるさいのでやっておく
        m_destroyEffectPrefab = null;
    }
}
