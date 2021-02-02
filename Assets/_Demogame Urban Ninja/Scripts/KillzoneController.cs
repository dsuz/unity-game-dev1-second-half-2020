using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    [SerializeField] Transform m_restartPoint = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.position = m_restartPoint.position;
        }
    }
}
