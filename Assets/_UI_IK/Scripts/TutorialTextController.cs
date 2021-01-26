using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーが侵入した時に画面にテキストを表示させるコンポーネント。
/// 表示・非表示の切り替えはアニメーションで行うため、Animator に依存する。
/// </summary>
[RequireComponent(typeof(Animator))]
public class TutorialTextController : MonoBehaviour
{
    /// <summary>子オブジェクトの Canvas</summary>
    [SerializeField] Canvas m_canvas = null;
    /// <summary>画面に表示する Text オブジェクト。m_canvas の子オブジェクトであること</summary>
    [SerializeField] Text m_textUi = null;
    /// <summary>画面に表示する文字列</summary>
    [SerializeField, TextArea(1, 6)] string m_textString;
    /// <summary>表示する時の効果音</summary>
    [SerializeField] AudioClip m_showSfx = null;
    /// <summary>表示を消す時の効果音</summary>
    [SerializeField] AudioClip m_hideSfx = null;
    Animator m_anim = null;
    AudioSource m_audio = null;

    void Start()
    {
        // 初期化処理
        m_textUi.text = m_textString;
        m_canvas.gameObject.SetActive(false);   // Canvas が表示されたままの時は、初期化時に消す
        m_anim = GetComponent<Animator>();
        m_audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player が接触したらテキストを表示して音を鳴らす
        if (other.gameObject.CompareTag("Player"))
        {
            m_anim.Play("Show");

            if (m_showSfx && m_audio)
            {
                m_audio.PlayOneShot(m_showSfx);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player が接触したら音を鳴らしてテキストを消す
        if (other.gameObject.CompareTag("Player"))
        {
            m_anim.Play("Hide");

            if (m_hideSfx && m_hideSfx)
            {
                m_audio.PlayOneShot(m_hideSfx);
            }
        }
    }
}
