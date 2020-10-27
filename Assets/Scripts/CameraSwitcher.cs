using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// カメラのスイッチ機能を提供する。
/// </summary>
[RequireComponent(typeof(Collider))]
public class CameraSwitcher : MonoBehaviour
{
    /// <summary>トリガーに接触した時、アクティブになるカメラ</summary>
    [SerializeField] CinemachineVirtualCameraBase m_camera = null;

    void OnTriggerEnter(Collider other)
    {
        // 同じ Priority を持つカメラのうち、このカメラを最優先にする
        m_camera.MoveToTopOfPrioritySubqueue();
    }
}
