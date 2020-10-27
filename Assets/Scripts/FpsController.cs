using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FPS での「ショット」を制御するコンポーネント
/// プレイヤーを操作するコンポーネントと同じスクリプトにこの処理を書いてもよいが、今回は分けた。
/// Fire1 を押すと、ターゲットを撃つ。
/// </summary>
public class FpsController : MonoBehaviour
{
    /// <summary>照準</summary>
    [SerializeField] Image m_crosshairUi = null;
    /// <summary>照準にターゲットが入っていない時の色</summary>
    [SerializeField] Color m_crosshairColorOnNoTarget = Color.white;
    /// <summary>照準にターゲットが入っている時の色</summary>
    [SerializeField] Color m_crosshairColorOnTargeted = Color.red;
    /// <summary>LineRenderer 兼 Line の出発点</summary>
    [SerializeField] LineRenderer m_line = null;
    /// <summary>射程距離</summary>
    [SerializeField] float m_shootRange = 15f;
    /// <summary>当たるレイヤー</summary>
    [SerializeField] LayerMask m_layerMask = 0;
    /// <summary>発射した時の音</summary>
    [SerializeField] AudioClip m_shootSound = null;
    /// <summary>命中した時の音</summary>
    [SerializeField] AudioClip m_hitSound = null;

    void Start()
    {
        // FPS なのでマウスカーソルを消す。ESC で表示される。
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // カメラから照準に向かって Ray を飛ばし、何かに当たっているか調べる
        Ray ray = Camera.main.ScreenPointToRay(m_crosshairUi.rectTransform.position);
        RaycastHit hit;
        Vector3 hitPosition = m_line.transform.position + m_line.transform.forward * m_shootRange;  // hitPosition は Ray が当たった場所。Line の終点となる。何にも当たっていない時は Muzzle から射程距離だけ前方にする。
        GameObject hitObject = null;    // Ray が当たったオブジェクト

        // Ray が何かに当たったか・当たっていないかで処理を分ける        
        if (Physics.Raycast(ray, out hit, m_shootRange, m_layerMask))
        {
            m_crosshairUi.color = m_crosshairColorOnTargeted;
            hitPosition = hit.point;    // Ray が当たった場所
            hitObject = hit.collider.gameObject;    // Ray が洗ったオブジェクト
        }
        else
        {
            m_crosshairUi.color = m_crosshairColorOnNoTarget;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            DrawLaser(hitPosition); // レーザーの終点は「Ray が当たっている時は当たった場所、当たっていない時は前方・射程距離ぶんの長さ」になる
            PlayShootSound(m_line.transform.position);  // レーザーの発射点で射撃音を鳴らす

            if (hitObject)
            {
                Hit(hitObject);
                PlayHitSound(hitPosition);  // レーザーが当たった場所でヒット音を鳴らす
            }
        }
        else
        {
            DrawLaser(m_line.transform.position);   // 撃っていない時は、Line の終点と始点を同じ位置にすることで Line を消す
        }
    }

    void OnDestroy()
    {
        // 消したマウスカーソルを元に戻す。
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// ショットがオブジェクトに当たった時に呼び出す。
    /// </summary>
    /// <param name="hitObject"></param>
    void Hit(GameObject hitObject)
    {
        // 今回は「当たったオブジェクトに Rigidbody コンポーネントがアタッチされていたら、メインカメラの方向に力を加える」処理をする
        Rigidbody rb = hitObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(Camera.main.transform.forward * 10, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 射撃音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayShootSound(Vector3 position)
    {
        if (m_shootSound)
        {
            AudioSource.PlayClipAtPoint(m_shootSound, position);
        }
    }

    /// <summary>
    /// ヒット音を鳴らす
    /// </summary>
    /// <param name="position">音を鳴らす場所</param>
    void PlayHitSound(Vector3 position)
    {
        if (m_hitSound)
        {
            AudioSource.PlayClipAtPoint(m_hitSound, position);
        }
    }

    /// <summary>
    /// Line Renderer を使ってレーザーを描く
    /// </summary>
    /// <param name="destination">レーザーの終点</param>
    void DrawLaser(Vector3 destination)
    {
        Vector3[] positions = { m_line.transform.position, destination };   // レーザーの始点は常に Muzzle にする
        m_line.positionCount = positions.Length;   // Line を終点と始点のみに制限する
        m_line.SetPositions(positions);
    }
}
