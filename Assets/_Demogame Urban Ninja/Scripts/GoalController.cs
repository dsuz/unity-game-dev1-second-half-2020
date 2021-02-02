using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    [SerializeField] string m_sceneNameToBeLoaded = "";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_sceneNameToBeLoaded.Length > 0)
            {
                SceneManager.LoadScene(m_sceneNameToBeLoaded);
            }
        }
    }
}
