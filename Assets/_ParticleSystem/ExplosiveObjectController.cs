using UnityEngine;

public class ExplosiveObjectController : MonoBehaviour
{
    [SerializeField] GameObject m_explosionPrefab = null;

    void OnDestroy()
    {
        if (m_explosionPrefab)
        {
            Instantiate(m_explosionPrefab, this.transform.position, Quaternion.identity);
        }
    }

    void OnApplicationQuit()
    {
        m_explosionPrefab = null;
    }
}
