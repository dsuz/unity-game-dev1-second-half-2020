using UnityEngine;
using UnityEngine.Playables;    // Timeline をスクリプトからコントロールするために必要
using Cinemachine;              // Cinemachine の仮想カメラをスクリプトからコントロールするために必要

/// <summary>
/// ゲーム全体を管理するコンポーネント
/// カットシーンの再生をしてからゲームを開始する
/// </summary>
public class UrbanNinjaGameManager : MonoBehaviour
{
    /// <summary>プレイヤーのプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab = null;
    /// <summary>ゲーム中のメインの仮想カメラ</summary>
    [SerializeField] CinemachineVirtualCamera m_mainVirtualCamera = null;
    /// <summary>ゲーム開始時に再生する PlayableDirector</summary>
    [SerializeField] PlayableDirector m_openingCutScene = null;
    [SerializeField] AudioSource m_bgmAudio = null;
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
                    StartGame();
                }
                else if (!m_openingCutScene)
                {
                    StartGame();
                }
                break;
        }
    }

    void StartGame()
    {
        SpawnPlayer();
        m_state = GameState.InGame;

        if (m_bgmAudio)
            m_bgmAudio.Play();
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
}
