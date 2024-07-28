using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // �J�������Ǐ]����^�[�Q�b�g�i�L�����N�^�[�j
    public Vector3 offset = new Vector3(0, 5, -10); // �J�����̃I�t�Z�b�g�ʒu

    void LateUpdate()
    {
        if (target != null)
        {
            // �^�[�Q�b�g�̈ʒu�ɃI�t�Z�b�g��ǉ����ăJ�����̈ʒu��ݒ�
            transform.position = target.position + offset;
        }
    }
}
