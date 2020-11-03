using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の当たり判定に入った時の挙動を制御する
/// </summary>
[RequireComponent(typeof(Collider))]
public class PlayerAttackController : MonoBehaviour
{
    /// <summary>攻撃した時に力を加える方向</summary>
    [SerializeField] Vector3 m_direction = Vector3.up;
    /// <summary>攻撃した時に加える力の大きさ</summary>
    [SerializeField] float m_power = 10;

    void OnTriggerEnter(Collider other)
    {
        // 攻撃したものが Rigidbody だったら、設定した方向に力を加える
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(m_direction * m_power, ForceMode.Impulse);
        }
    }
}
