using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NinjaGameManager manager = GameObject.FindObjectOfType<NinjaGameManager>();
            manager.EnemyGeneration(true);
        }
    }
}
