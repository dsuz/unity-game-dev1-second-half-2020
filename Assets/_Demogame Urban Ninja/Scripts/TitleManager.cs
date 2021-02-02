using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトル画面を管理するコンポーネント
/// クリックした時に Timeline 再生中だったらスキップする。
/// Timeline 再生後にクリックしたらゲーム用シーンをロードする。
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>Timeline を制御しているオブジェクト</summary>
    [SerializeField] PlayableDirector m_director = null;
    /// <summary>スタートボタンのアニメーション。スタートする時にアニメーションを切り替える</summary>
    [SerializeField] Animator m_startButton = null;
    /// <summary>BGM を流している AudioSource。スタートする時に BGM を止めてゲームスタート用の効果音を鳴らす。</summary>
    [SerializeField] AudioSource m_audio = null;
    /// <summary>ゲームスタート用の効果音</summary>
    [SerializeField] AudioClip m_gameStartSfx = null;
    /// <summary>ゲームスタートした時にロードするシーン名</summary>
    [SerializeField] string m_sceneToBeLoaded = "";
    /// <summary>フェード用のパネル</summary>
    [SerializeField] Image m_fadePanel = null;
    /// <summary>フェードにかける時間（秒）</summary>
    [SerializeField] float m_fadeTime = 1.5f;
    bool m_isStarted = false;

    void Update()
    {
        // Timeline 再生中ならばスキップし、再生が終わっていたらゲームを開始する。
        if (Input.GetMouseButtonDown(0))
        {
            if (m_director.state == PlayState.Playing)
            {
                Skip();
            }
            else if (!m_isStarted)
            {
                StartGame();
            }
        }
    }

    /// <summary>
    /// Timeline の再生をスキップする。
    /// </summary>
    void Skip()
    {
        m_director.playableGraph.GetRootPlayable(0).SetSpeed(300);
    }

    /// <summary>
    /// ゲームを開始する。
    /// </summary>
    void StartGame()
    {
        m_isStarted = true;
        m_startButton.Play("Start");
        m_audio.clip = m_gameStartSfx;
        m_audio.Play();
        StartCoroutine(LoadSceneWithFade(m_fadeTime));
    }

    /// <summary>
    /// フェードしながらロードする。
    /// </summary>
    /// <param name="fadeTime">フェードにかける時間（秒）</param>
    /// <returns></returns>
    IEnumerator LoadSceneWithFade(float fadeTime)
    {
        float timer = 0;
        Color panelColor = m_fadePanel.color;
        float alpha = 0;

        // フェード処理
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            alpha += Time.deltaTime / m_fadeTime;
            panelColor.a = alpha;
            m_fadePanel.color = panelColor;
            yield return new WaitForEndOfFrame();
        }

        if (m_sceneToBeLoaded.Length > 0)
        {
            SceneManager.LoadSceneAsync(m_sceneToBeLoaded);
        }
    }
}