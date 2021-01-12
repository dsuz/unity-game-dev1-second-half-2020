using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を検出するコンポーネント。
/// Target Range より近くにいる敵（タグに EnemyTag が設定されたオブジェクト）のうち、最も近くにいる敵をロックオンする。
/// ロックオンしたオブジェクトは Target プロパティで取得できる。
/// </summary>
public class EnemyDetector : MonoBehaviour
{
    /// <summary>索敵範囲</summary>
    [SerializeField] float m_targetRange = 4f;
    /// <summary>敵の検出を行う間隔（単位: 秒）</summary>
    [SerializeField] float m_detectInterval = 1f;
    /// <summary>ロックオンしているオブジェクト</summary>
    GameObject m_target = null;
    float m_timer;

    /// <summary>
    /// ロックオンしている敵を取得する
    /// </summary>
    public GameObject Target
    {
        get { return m_target; }
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        // 一定間隔で検出を行う
        if (m_timer > m_detectInterval)
        {
            m_timer = 0;

            // シーン内の敵を全て取得する
            GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("EnemyTag");
            
            foreach (var enemy in enemyArray)
            {
                // 索敵範囲外の敵は処理しない
                float distance = Vector3.Distance(this.transform.position, enemy.transform.position);

                if (distance < m_targetRange)
                {
                    // ロックオンしている敵がいない場合は、enemy をロックオンする。現在の target より enemy が近くに居る場合は、enemy をロックオンする。
                    if (m_target == null || distance < Vector3.Distance(this.transform.position, m_target.transform.position))
                    {
                        m_target = enemy;
                    }
                }
            }
        }

        // ロックオンしているターゲットが索敵範囲外に出たらロックオンをやめる
        if (m_target)
        {
            if (m_targetRange < Vector3.Distance(this.transform.position, m_target.transform.position))
            {
                m_target = null;
            }
            else
            {
                // ロックオンしている敵まで線を引く
                Debug.DrawLine(this.gameObject.transform.position, m_target.transform.position, Color.red);
            }
        }
    }
}
