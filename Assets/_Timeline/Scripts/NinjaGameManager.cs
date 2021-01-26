using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;    // Timeline をスクリプトからコントロールするために必要
using Cinemachine;              // Cinemachine の仮想カメラをスクリプトからコントロールするために必要

/// <summary>
/// ゲーム全体を管理するコンポーネント
/// カットシーンの再生をしてからゲームを開始する
/// </summary>
public class NinjaGameManager : MonoBehaviour
{
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab = null;
    /// <summary>ゲーム中のメインの仮想カメラ</summary>
    [SerializeField] CinemachineVirtualCamera m_mainVirtualCamera = null;
    /// <summary>ゲーム開始時に再生する PlayableDirector</summary>
    [SerializeField] PlayableDirector m_openingCutScene = null;
    /// <summary>敵生成装置</summary>
    [SerializeField] ObjectGenerator[] m_enemyGenerator = null;
    /// <summary>ゲームの状態</summary>
    GameState m_state = GameState.None;
    
    void Update()
    {
        switch (m_state)
        {
            // オープニングを再生する
            case GameState.None:
                if (m_openingCutScene)
                {
                    m_openingCutScene.Play();
                }
                m_state = GameState.Opening;
                break;
            // オープニングの再生が終わったらゲームを開始する
            case GameState.Opening:
                if (m_openingCutScene && m_openingCutScene.state != PlayState.Playing)
                {
                    m_openingCutScene.gameObject.SetActive(false);
                    SpawnPlayer();
                    m_state = GameState.InGame;
                }
                else if (!m_openingCutScene)
                {
                    SpawnPlayer();
                    m_state = GameState.InGame;
                }
                break;
        }
    }

    /// <summary>
    /// Player を生成し、仮想カメラで追うように設定する
    /// </summary>
    void SpawnPlayer()
    {
        GameObject go = Instantiate(m_playerPrefab, Vector3.zero, m_playerPrefab.transform.rotation);
        m_mainVirtualCamera.LookAt = go.transform;
        m_mainVirtualCamera.Follow = go.transform;
    }

    /// <summary>
    /// 敵の生成を開始・停止する
    /// </summary>
    /// <param name="control"></param>
    public void EnemyGeneration(bool control)
    {
        foreach(var generator in m_enemyGenerator)
        {
            generator.gameObject.SetActive(control);
        }
    }
}

/// <summary>
/// ゲームの状態を表す
/// </summary>
public enum GameState
{
    /// <summary>ゲーム起動時など</summary>
    None,
    /// <summary>オープニングカットシーン再生中</summary>
    Opening,
    /// <summary>ゲーム中</summary>
    InGame,
}