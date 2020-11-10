using UnityEngine;

/// <summary>
/// 敵を制御するコンポーネント
/// </summary>
public class EnemyController : MonoBehaviour
{
    /// <summary>やられた時に表示するプレハブ</summary>
    [SerializeField] GameObject m_deadBodyPrefab = null;
    /// <summary>やられた時の効果音</summary>
    [SerializeField] AudioClip m_attackedSfx = null;
    /// <summary>プレイヤーを参照する変数</summary>
    Transform m_player = null;

    void Start()
    {
        // プレイヤーの参照を取得する
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            m_player = player.transform;
        }
    }

    void Update()
    {
        // プレイヤーの方を向く
        if (m_player)
        {
            Vector3 playerPosition = m_player.position;
            playerPosition.y = this.transform.position.y;
            this.transform.LookAt(playerPosition);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 効果音を鳴らす
        if (m_attackedSfx)
        {
            AudioSource.PlayClipAtPoint(m_attackedSfx, this.transform.position);
        }
        // やられたオブジェクトを生成して自分は消える
        Instantiate(m_deadBodyPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
