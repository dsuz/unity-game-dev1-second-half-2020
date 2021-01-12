using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾を発射するコンポーネント。弾のオブジェクトにアタッチして使う。
/// オブジェクトが生成されたら前（Z軸の正方向）に直進する
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    /// <summary>弾が飛ぶ速さ</summary>
    [SerializeField] float m_speed = 4f;
    /// <summary>弾が回転する速さう</summary>
    [SerializeField] float m_rotateSpeed = 5f;
    /// <summary>弾の生存期間（単位: 秒）</summary>
    [SerializeField] float m_lifetime = 1f;
    Rigidbody m_rb = null;

    void Start()
    {
        // 弾を飛ばし、生存期間を設定する
        m_rb = GetComponent<Rigidbody>();
        m_rb.velocity = this.transform.forward * m_speed;
        Destroy(this.gameObject, m_lifetime);
    }

    void FixedUpdate()
    {
        // 弾を回転させる
        this.transform.Rotate(Vector3.up, m_rotateSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        // 敵に当たったら消える
        if (other.gameObject.CompareTag("EnemyTag"))
        {
            Destroy(this.gameObject);
        }
    }
}
