using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextController : MonoBehaviour
{
    [SerializeField] Canvas m_canvas = null;
    [SerializeField] Text m_textUi = null;
    [SerializeField, TextArea(1, 6)] string m_textString;
    [SerializeField] AudioClip m_showSfx = null;
    [SerializeField] AudioClip m_hideSfx = null;
    Animator m_anim = null;
    AudioSource m_audio = null;

    void Start()
    {
        m_textUi.text = m_textString;
        m_canvas.gameObject.SetActive(false);
        m_anim = GetComponent<Animator>();
        m_audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_anim.Play("Show");
            m_audio.PlayOneShot(m_showSfx);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_anim.Play("Hide");
            m_audio.PlayOneShot(m_hideSfx);
        }

    }
}
