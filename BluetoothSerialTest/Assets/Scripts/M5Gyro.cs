using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IMU�̃f�[�^��GameObject�ɔ��f������N���X
/// </summary>
public class M5Gyro : MonoBehaviour
{
    /// <summary>
    /// ��M����f�[�^�̏��Ԃ������񋓌^
    /// </summary>
    enum DataOrder
    {
        Roll = 0,
        Pitch,
        Yaw,
        DataSize
    }

    [SerializeField] SerialHandle _Serial;      // �V���A���ʐM�̊Ǘ��N���X

    private Vector3 _GyroRotate;                // �V���A���o�R�Ŏ�M����IMU�̉�]��
    private float[] _ReceiveDataBuffer;         // �V���A���o�R�Ŏ�M�����f�[�^�ꗗ
    private Rigidbody _Rb;                      // ����GameObject�ɃA�^�b�`����Ă���Rigidbody


    void Start()
    {
        // ��M�C�x���g�̓o�^
        _Serial.OnDataReceived += OnDataReceived;

        // ���������Ɋ�Â���]�������邽�߂�Rigidbody���擾���Ă���
        _Rb = GetComponent<Rigidbody>();

        // �e�l�̏�����
        _ReceiveDataBuffer = new float[(int)DataOrder.DataSize];
        _GyroRotate = Vector3.zero;
    }

    /// <summary>
    /// �f�[�^��M���ɌĂяo�����֐�
    /// </summary>
    /// <param name="message">�󂯎�����f�[�^1�s��</param>
    private void OnDataReceived(string message)
    {
        // �u,�v��؂�̕�����𕪊�
        string[] parsedMsgs = message.Split(",");

        // ��M�����f�[�^�����z��������Ȃ���ΐ������f�[�^�������Ȃ��̂Ń��^�[��
        if (parsedMsgs.Length < (int)DataOrder.DataSize) return;

        // ��M�����������float�ɕϊ�
        for (int i = 0; i < (int)DataOrder.DataSize; i++)
        {
            if (float.TryParse(parsedMsgs[i], out float value))
            {
                _ReceiveDataBuffer[i] = value;
            }
            else
            {
                // �ϊ��Ɏ��s������f�[�^���������Ȃ��̂Ń��^�[��
                return;
            }
        }

        // ��M�����f�[�^�����Ƃɉ�]��GameObject�ɔ��f
        _GyroRotate.x = -_ReceiveDataBuffer[(int)DataOrder.Pitch];
        _GyroRotate.y = -_ReceiveDataBuffer[(int)DataOrder.Yaw];
        _GyroRotate.z = -_ReceiveDataBuffer[(int)DataOrder.Roll];
        _Rb.MoveRotation(Quaternion.Euler(_GyroRotate));
    }
}
