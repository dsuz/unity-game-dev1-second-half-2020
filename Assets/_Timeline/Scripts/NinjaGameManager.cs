using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;    // Timeline をスクリプトからコントロールするために必要

/// <summary>
/// ゲーム全体を管理するコンポーネント
/// カットシーンの再生をしてからゲームを開始する
/// </summary>
public class NinjaGameManager : MonoBehaviour
{
    [SerializeField] GameObject m_playerPrefab = null;
    /// <summary>ゲーム開始時に再生する PlayableDirector</summary>
    [SerializeField] PlayableDirector m_openingCutScene = null;
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
                    Instantiate(m_playerPrefab, Vector3.zero, m_playerPrefab.transform.rotation);
                    m_state = GameState.InGame;
                }
                else if (!m_openingCutScene)
                {
                    Instantiate(m_playerPrefab, Vector3.zero, m_playerPrefab.transform.rotation);
                    m_state = GameState.InGame;
                }
                break;
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