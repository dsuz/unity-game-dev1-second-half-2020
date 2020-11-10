using UnityEngine;

/// <summary>
/// フックをひっかけられるオブジェクトを制御する
/// OnBecameVislble/OnBecameInvisible は「シーンビューのカメラ」も影響することに注意すること。
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GrapplingTargetController : MonoBehaviour
{
    bool m_isTargetable = false;

    /// <summary>
    /// オブジェクトが画面内にあるかどうかを返す
    /// </summary>
    public bool IsHookable
    {
        get { return m_isTargetable; }
    }

    private void OnBecameVisible()
    {
        m_isTargetable = true;
    }

    private void OnBecameInvisible()
    {
        m_isTargetable = false;
    }
}
