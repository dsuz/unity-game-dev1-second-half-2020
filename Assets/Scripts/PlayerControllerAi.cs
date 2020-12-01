using UnityEngine;
using UnityEngine.AI;   // Navmesh Agent を使うために必要

/// <summary>
/// Navmesh Agent を使って経路探索を行い、移動するためのコンポーネント
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerControllerAi : MonoBehaviour
{
    /// <summary>移動先となる位置情報</summary>
    [SerializeField] Transform m_target;
    /// <summary>移動先座標を保存する変数</summary>
    Vector3 m_cachedTargetPosition;
    /// <summary>キャラクターなどのアニメーションするオブジェクトを指定する</summary>
    [SerializeField] Animator m_animator;
    NavMeshAgent m_agent;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_cachedTargetPosition = m_target.position; // 初期位置を保存する（※）
    }

    /*
     * （※）m_cachedTargetPosition を使って座標を保存しているのは、以下の Update() 内で「毎フレーム座標をセットする」という処理を避け、負荷を下げるためである。
     * 毎フレーム座標をセットすることで経路の計算を毎フレームしてしまうことを避けるため、「Target が移動した時のみ」目的地をセットして経路の計算を行わせている。
     */

    void Update()
    {
        // m_target が移動したら Navmesh Agent を使って移動させる
        if (Vector3.Distance(m_cachedTargetPosition, m_target.position) > 0.1f) // m_target が 10cm 以上移動したら
        {
            m_cachedTargetPosition = m_target.position; // 移動先の座標を保存する
            m_agent.SetDestination(m_cachedTargetPosition); // Navmesh Agent に目的地をセットする（Vector3 で座標を設定していることに注意。Transform でも GameObject でもなく、Vector3 で目的地を指定する）
        }

        // m_animator がアサインされていたら Animator Controller にパラメーターを設定する
        if (m_animator)
        {
            m_animator.SetFloat("Speed", m_agent.velocity.magnitude);
        }
    }
}
