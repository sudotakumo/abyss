using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // カメラが追従するターゲット（キャラクター）
    public Vector3 offset = new Vector3(0, 5, -10); // カメラのオフセット位置

    void LateUpdate()
    {
        if (target != null)
        {
            // ターゲットの位置にオフセットを追加してカメラの位置を設定
            transform.position = target.position + offset;
        }
    }
}
