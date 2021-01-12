using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rigidbody を使ってプレイヤーを動かすコンポーネント
/// 入力を受け取り、それに従ってオブジェクトを動かす。
/// カメラの向きに応じて相対的に移動する。
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerNinjaController : MonoBehaviour
{
    /// <summary>動く速さ</summary>
    [SerializeField] float m_movingSpeed = 5f;
    /// <summary>ターンの速さ</summary>
    [SerializeField] float m_turnSpeed = 3f;
    /// <summary>ジャンプ力</summary>
    [SerializeField] float m_jumpPower = 5f;
    /// <summary>接地判定の際、コライダーの中心 (Center) からどれくらいの距離を「接地している」と判定するかの長さ</summary>
    [SerializeField] float m_isGroundedLength = 1.1f;
    /// <summary>地面を表す Layer</summary>
    [SerializeField] LayerMask m_groundLayer = ~0;
    /// <summary>何回まで接地せずにジャンプできるか。２段ジャンプする時は 2 に設定する</summary>
    [SerializeField] int m_maxJumpCount = 2;
    /// <summary>弾を発射する地点となるオブジェクト</summary>
    [SerializeField] Transform m_muzzle = null;
    /// <summary>弾のプレハブ</summary>
    [SerializeField] GameObject m_bulletPrefab = null;
    /// <summary>ジャンプした時に鳴らす効果音</summary>
    [SerializeField] AudioClip m_jumpSfx = null;

    /// <summary>ジャンプした回数。接地状態からジャンプした時に 1 になる。</summary>
    int m_jumpCount = 0;
    /// <summary>敵を検出するコンポーネント</summary>
    EnemyDetector m_enemyDetector = null;
    Rigidbody m_rb = null;
    CapsuleCollider m_collider = null;
    Animator m_anim = null;

    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_collider = GetComponent<CapsuleCollider>();
        m_anim = GetComponent<Animator>();
        m_enemyDetector = GetComponent<EnemyDetector>();
    }

    void Update()
    {
        // 方向の入力を取得し、方向を求める
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // 入力方向のベクトルを組み立てる
        Vector3 dir = Vector3.forward * v + Vector3.right * h;

        if (dir == Vector3.zero)
        {
            // 方向の入力がニュートラルの時は、y 軸方向の速度を保持するだけ
            m_rb.velocity = new Vector3(0f, m_rb.velocity.y, 0f);
        }
        else
        {
            // カメラを基準に入力が上下=奥/手前, 左右=左右にキャラクターを向ける
            dir = Camera.main.transform.TransformDirection(dir);    // メインカメラを基準に入力方向のベクトルを変換する
            dir.y = 0;  // y 軸方向はゼロにして水平方向のベクトルにする

            // 入力方向に滑らかに回転させる
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_turnSpeed);  // Slerp を使うのがポイント

            Vector3 velo = dir.normalized * m_movingSpeed; // 入力した方向に移動する
            velo.y = m_rb.velocity.y;   // ジャンプした時の y 軸方向の速度を保持する
            m_rb.velocity = velo;   // 計算した速度ベクトルをセットする
        }

        // ジャンプの入力を取得し、接地している場合はジャンプする
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // 接地している時のみ攻撃可能
        if (Input.GetButtonDown("Fire1") && IsGrounded())
        {
            Attack();
        }
    }

    void LateUpdate()
    {
        // アニメーションを操作する
        Vector3 velocity = m_rb.velocity;
        velocity.y = 0; // 上下方向の速度は無視する
        m_anim.SetFloat("Speed", velocity.magnitude);
        m_anim.SetBool("IsGrounded", IsGrounded());
    }

    /// <summary>
    /// ジャンプする時に呼び出す
    /// </summary>
    void Jump()
    {
        AudioSource.PlayClipAtPoint(m_jumpSfx, this.transform.position);
        m_rb.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
    }

    /// <summary>
    /// 攻撃する時に呼び出す
    /// </summary>
    void Attack()
    {
        // ロックオンしている敵がいる場合
        if (m_enemyDetector.Target)
        {
            // ロックオンしている敵の方を向く
            Vector3 dir = m_enemyDetector.Target.transform.position - this.transform.position;
            dir.y = 0;
            this.transform.forward = dir;
        }

        // 攻撃アニメーションを再生し、弾を発射する
        m_anim.SetTrigger("AttackTrigger");
        Instantiate(m_bulletPrefab, m_muzzle.position, m_muzzle.rotation);
    }

    /// <summary>
    /// 地面に接触しているか判定する
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        Vector3 start = this.transform.position + m_collider.center;   // start: コライダーの中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end, m_groundLayer); // 引いたラインに地面にぶつかっていたら true とする
        return isGrounded;
    }
}