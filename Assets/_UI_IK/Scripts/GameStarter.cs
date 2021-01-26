using UnityEngine;

/// <summary>
/// ゲームを開始する。
/// </summary>
public class GameStarter : MonoBehaviour
{
    // プレイヤーがトリガーから出ていったらゲームを開始する
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NinjaGameManager manager = GameObject.FindObjectOfType<NinjaGameManager>();
            manager.EnemyGeneration(true);
        }
    }
}
