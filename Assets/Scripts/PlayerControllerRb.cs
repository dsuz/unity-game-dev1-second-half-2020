using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRb : MonoBehaviour
{
    /// <summary>動く力</summary>
    [SerializeField] float m_movePower = 12f;
    /// <summary>動く速さの最大値</summary>
    [SerializeField] float m_maxSpeed = 5f;
    /// <summary>動きを減速する係数</summary>
    [SerializeField] float m_coefficient = 0.95f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>接地判定の際、中心からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1.2f;
    /// <summary>空中制御可能か</summary>
    [SerializeField] bool m_moveInTheAir = false;
    /// <summary>入力されている方向</summary>
    Vector2 m_inputDirection;
    /// <summary>y軸方向にこれ以下の速度で移動していたら（落ちていたら）落下しているとみなす</summary>
    [SerializeField] float m_fallVelocity = -0.05f;
    Rigidbody m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 入力されている方向を保存する
        m_inputDirection.x = Input.GetAxisRaw("Horizontal");
        m_inputDirection.y = Input.GetAxisRaw("Vertical");

        // ジャンプの入力を取得し、接地している時に押されていたらジャンプする
        if (Input.GetButtonDown("Jump") && m_rb.velocity.y > m_fallVelocity && IsGrounded())
        {
            m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse); // 第２引数が ForceMode.Impulse であることに注意すること
        }
    }

    /// <summary>
    /// 物理挙動をリアルタイムで更新する
    /// </summary>
    void FixedUpdate()
    {
        // 入力された方向に従ってオブジェクトを動かす
        if (m_inputDirection == Vector2.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を維持しながら xy 軸平面上は減速する
            Vector3 v = new Vector3(m_rb.velocity.x * m_coefficient, m_rb.velocity.y, m_rb.velocity.z * m_coefficient);
            m_rb.velocity = v;
        }
        else
        {
            // 方向が入力されている時は、まず xz 平面上の速度を求める
            float speed = new Vector2(m_rb.velocity.x, m_rb.velocity.z).magnitude;

            // 入力されている方向（x, y 平面）を動く方向（x, z 平面）に変換する
            Vector3 dir = new Vector3(m_inputDirection.x, 0, m_inputDirection.y);

            // 入力されている方向にキャラクターを向ける
            this.transform.forward = dir;

            if (m_moveInTheAir || IsGrounded())
            {
                // xz 平面状の速度が設定した速度を超えていない時は、入力された方向に力を加える
                if (speed <= m_maxSpeed)
                {
                    // 入力されている方向に設定した力を加える
                    m_rb.AddForce(dir * m_movePower);
                }
            }
        }
    }

    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position;   // start: オブジェクトの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " と衝突した");
    }
}
