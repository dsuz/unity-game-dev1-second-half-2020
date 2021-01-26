using UnityEngine;

/// <summary>
/// 両手の IK を制御する。トリガーをオブジェクトに追加し、トリガーに接触すると両手の IK がアクティブになる。
/// </summary>
[RequireComponent(typeof(Animator))]
public class HandsIKController : MonoBehaviour
{
    /// <summary>右手のIK</summary>
    [SerializeField] Transform m_leftHandIkTarget;
    /// <summary>左手のIK</summary>
    [SerializeField] Transform m_rightHandIkTarget;

    // IK のウェイト
    [SerializeField, Range(0f, 1f)] float m_rightHandPositionWeight = 1f;
    [SerializeField, Range(0f, 1f)] float m_rightHandRotationWeight = 1f;
    [SerializeField, Range(0f, 1f)] float m_leftHandPositionWeight = 1f;
    [SerializeField, Range(0f, 1f)] float m_leftHandRotationWeight = 1f;

    /// <summary>IK がアクティブかどうかのフラグ</summary>
    bool m_isIkActive = false;
    Animator m_anim;

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!m_isIkActive) return;  // IK がアクティブでなければ何もしない

        // 両手の IK Position/Rotation をセットする
        m_anim.SetIKPositionWeight(AvatarIKGoal.RightHand, m_rightHandPositionWeight);
        m_anim.SetIKRotationWeight(AvatarIKGoal.RightHand, m_rightHandRotationWeight);
        m_anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, m_leftHandPositionWeight);
        m_anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, m_leftHandRotationWeight);
        m_anim.SetIKPosition(AvatarIKGoal.RightHand, m_rightHandIkTarget.position);
        m_anim.SetIKRotation(AvatarIKGoal.RightHand, m_rightHandIkTarget.rotation);
        m_anim.SetIKPosition(AvatarIKGoal.LeftHand, m_leftHandIkTarget.position);
        m_anim.SetIKRotation(AvatarIKGoal.LeftHand, m_leftHandIkTarget.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Push"))
        {
            // IK をアクティブにする
            m_isIkActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Push"))
        {
            // IK を非アクティブにする
            m_isIkActive = false;
        }
    }
}
