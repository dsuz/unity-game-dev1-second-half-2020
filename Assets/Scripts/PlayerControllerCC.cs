using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Controller を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす
/// </summary>

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerCC : MonoBehaviour
{
    /// <summary>移動速度</summary>
    [SerializeField] float m_moveSpeed = 1f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>重力のスケール</summary>
    [SerializeField] float m_gravityScale = 2f;
    
    CharacterController m_cc;
    /// <summary>キャラクターの移動方向</summary>
    Vector3 m_moveDirection;

    void Start()
    {
        m_cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 方向の入力を取得し、入力方向の単位ベクトルを計算する
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;  // dir = 入力された方向（xz 平面）の単位ベクトル

        if (dir != Vector3.zero)
        {
            // キャラクターを入力された方向に向ける
            this.transform.forward = dir;   
        }

        m_moveDirection.x = dir.x * m_moveSpeed;
        m_moveDirection.z = dir.z * m_moveSpeed;

        if (m_cc.isGrounded)    // Character Controller コンポーネントには isGrounded プロパティがある
        {
            // 接地している時にジャンプボタンを押されたら、上方向に移動する
            if (Input.GetButtonDown("Jump"))
            {
                m_moveDirection.y = m_jumpPower;
            }
            else
            {
                m_moveDirection.y = 0;
            }
        }
        else
        {
            // 空中にいる時は、重力に従って下に移動する
            m_moveDirection += Physics.gravity * m_gravityScale * Time.deltaTime;
        }

        // Character Controller を使って移動する
        m_cc.Move(m_moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Character Controller の接触判定を行う関数
    /// Collider の時とは違うものを使うことに注意すること
    /// </summary>
    /// <param name="hit"></param>
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name + " と接触した");
    }
}
