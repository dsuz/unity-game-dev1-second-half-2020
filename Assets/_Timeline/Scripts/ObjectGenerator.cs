using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interval おきにオブジェクト（プレハブ）を生成する
/// </summary>
public class ObjectGenerator : MonoBehaviour
{
    /// <summary>生成するオブジェクト（プレハブ）</summary>
    [SerializeField] GameObject m_generateObject = null;
    /// <summary>オブジェクトを生成する間隔（単位: 秒）</summary>
    [SerializeField] float m_interval = 0.25f;
    /// <summary>プレイヤーが居なくなったら動作を止める</summary>
    [SerializeField] bool m_stopWorkingOnNoPlayer = true;
    float m_timer;

    void Update()
    {
        // プレイヤーが居ない時には何もしない
        if (m_stopWorkingOnNoPlayer)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (!p)
            {
                return;
            }
        }

        // 〇秒おきに処理を実行するやり方。このパターンは使えるようになっておくこと。
        m_timer += Time.deltaTime;

        if (m_timer > m_interval)
        {
            m_timer = 0;
            Instantiate(m_generateObject, this.gameObject.transform.position, m_generateObject.transform.rotation);
        }
    }
}
