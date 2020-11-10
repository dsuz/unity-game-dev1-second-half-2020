using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// ワイヤー（グラップリングフック）を制御するコンポーネント
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class GrapplingManager : MonoBehaviour
{
    /// <summary>ワイヤーの射程距離</summary>
    [SerializeField] float m_targetRange = 5f;
    /// <summary>フックをひっかけるターゲットを示すカーソル</summary>
    [SerializeField] Image m_crosshair = null;
    /// <summary>GrapplingTarget にカーソルが当たった時の効果音</summary>
    [SerializeField] AudioClip m_cursorSfx = null;
    /// <summary>ワイヤーを飛ばす時の効果音</summary>
    [SerializeField] AudioClip m_wireSfx = null;
    /// <summary>フックをはずす時の効果音</summary>
    [SerializeField] AudioClip m_loseHookSfx = null;
    /// <summary>現在射程距離内にあり、かつ画面内の GrapplingTarget を入れるリスト</summary>
    List<GrapplingTargetController> m_targets = new List<GrapplingTargetController>();
    /// <summary>現在のターゲット</summary>
    GrapplingTargetController m_target;
    /// <summary>効果音出力コンポーネント</summary>
    AudioSource m_audio = null;
    /// <summary>ワイヤーを描く Renderer</summary>
    LineRenderer m_line = null;

    void Start()
    {
        m_audio = GetComponent<AudioSource>();
        m_line = GetComponent<LineRenderer>();
        // マウスカーソルを消す
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnApplicationQuit()
    {
        // マウスカーソルを復帰する
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;
        m_targets.Clear();

        // 現在のターゲットから画面から消えた、または射程距離外に外れたら、ターゲットを消す
        if (m_target && (!m_target.IsHookable || Vector3.Distance(m_target.transform.position, player.transform.position) > m_targetRange))
        {
            m_target = null;
            Debug.Log("target lost.");
        }

        // 画面に映っている、かつ射程距離内にある GrapplingTarget を全て取得し、リストに入れる
        GrapplingTargetController[] targets = transform.GetComponentsInChildren<GrapplingTargetController>();
        foreach(GrapplingTargetController t in targets)
        {
            if (t.IsHookable && Vector3.Distance(player.transform.position, t.transform.position) < m_targetRange)
            {
                m_targets.Add(t);
            }
        }

        // 現在のターゲットがない時は、一番近いものをターゲットとする
        if (!m_target && m_targets.Count > 0)
        {
            m_target = m_targets.OrderBy(target =>
            {
                return Vector3.Distance(player.transform.position, target.transform.position);
            }).First();
            m_crosshair.GetComponent<Animator>().Play("CrosshairTargeted"); // アニメーションを再生する
            if (m_cursorSfx)
            {
                m_audio.PlayOneShot(m_cursorSfx);
            }
            Debug.Log(m_target.name + " is current target");
        }

        // ボタンを押したらターゲットにフックをひっかける
        ConfigurableJoint joint = player.GetComponent<ConfigurableJoint>();
        if (Input.GetButtonDown("Fire2"))
        {
            if (joint.connectedBody)
            {
                LoseHook(joint);
            }
            else if (m_target)
            {
                Hook(joint, m_target);
            }
        }

        // 照準でターゲットを捉える
        if (m_target)
        {
            AimTarget(m_target.transform.position);
            if (joint.connectedBody)
            {
                DrawWire(joint.transform.position + joint.anchor, m_target.transform.position);
            }
        }
        else
        {
            HideCrosshair();
        }
    }

    /// <summary>
    /// ワイヤーを描く
    /// </summary>
    /// <param name="source">始点</param>
    /// <param name="destination">終点</param>
    void DrawWire(Vector3 source, Vector3 destination)
    {
        m_line.SetPosition(0, source);
        m_line.SetPosition(1, destination);
    }

    /// <summary>
    /// 照準を当てる
    /// </summary>
    /// <param name="position">照準を当てるワールド座標</param>
    void AimTarget(Vector3 position)
    {
        if (m_crosshair)
        {
            m_crosshair.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
        }
    }

    /// <summary>
    /// 照準を隠す（画面外に移動する）
    /// </summary>
    void HideCrosshair()
    {
        if (m_crosshair)
        {
            m_crosshair.rectTransform.position = -1 * m_crosshair.rectTransform.rect.size;
        }
    }

    /// <summary>
    /// フックをひっかける
    /// </summary>
    /// <param name="joint">フックとなる joint</param>
    /// <param name="target">フックをひっかける対象</param>
    void Hook(ConfigurableJoint joint, GrapplingTargetController target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb)
        {
            joint.connectedBody = rb;
            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;
            if (m_wireSfx)
            {
                m_audio.PlayOneShot(m_wireSfx);
            }
        }
        else
        {
            Debug.LogErrorFormat("{0} doesn't have Rigidbody.", target.name);
        }
    }

    /// <summary>
    /// フックをはずす
    /// </summary>
    /// <param name="joint">フックとなる joint</param>
    void LoseHook(ConfigurableJoint joint)
    {
        joint.connectedBody = null;
        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;
        // Wire を隠す
        DrawWire(Vector3.zero, Vector3.zero);

        if (m_loseHookSfx)
        {
            m_audio.PlayOneShot(m_loseHookSfx);
        }
    }
}
