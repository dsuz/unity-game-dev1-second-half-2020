using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaController : MonoBehaviour
{
    [SerializeField] Transform[] m_spawnPoints = default;
    [SerializeField] EnemyNinjaController m_enemyPrefab = null;
    [SerializeField] float m_generateInterval = 1.5f;
    [SerializeField] float m_destroyInterval = 0.15f;
    bool m_isPlayerIn = false;
    float m_timer = 0f;

    void Start()
    {
        
    }
        
    void Update()
    {
        if (m_isPlayerIn)
        {
            m_timer += Time.deltaTime;
            if (m_timer > m_generateInterval)
            {
                m_timer = 0;
                SpawnEnemies();
            }
        }
    }

    void SpawnEnemies()
    {
        foreach(var t in m_spawnPoints)
        {
            var go = Instantiate(m_enemyPrefab, t.position, Quaternion.identity);
            go.transform.SetParent(this.transform);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_isPlayerIn)
            {
                m_isPlayerIn = true;
                m_timer = 0;
                SpawnEnemies();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isPlayerIn = false;
            StartCoroutine(DestroyAllEnemies());
        }
    }

    IEnumerator DestroyAllEnemies()
    {
        var enemies = this.transform.GetComponentsInChildren<EnemyNinjaController>();

        foreach (var e in enemies)
        {
            Destroy(e.gameObject);
            yield return new WaitForSeconds(m_destroyInterval);
        }
    }
}
