using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IMUのデータをGameObjectに反映させるクラス
/// </summary>
public class M5Gyro : MonoBehaviour
{
    /// <summary>
    /// 受信するデータの順番を示す列挙型
    /// </summary>
    enum DataOrder
    {
        Roll = 0,
        Pitch,
        Yaw,
        DataSize
    }

    [SerializeField] SerialHandle _Serial;      // シリアル通信の管理クラス

    private Vector3 _GyroRotate;                // シリアル経由で受信したIMUの回転量
    private float[] _ReceiveDataBuffer;         // シリアル経由で受信したデータ一覧
    private Rigidbody _Rb;                      // このGameObjectにアタッチされているRigidbody


    void Start()
    {
        // 受信イベントの登録
        _Serial.OnDataReceived += OnDataReceived;

        // 物理挙動に基づく回転をさせるためにRigidbodyを取得しておく
        _Rb = GetComponent<Rigidbody>();

        // 各値の初期化
        _ReceiveDataBuffer = new float[(int)DataOrder.DataSize];
        _GyroRotate = Vector3.zero;
    }

    /// <summary>
    /// データ受信時に呼び出される関数
    /// </summary>
    /// <param name="message">受け取ったデータ1行分</param>
    private void OnDataReceived(string message)
    {
        // 「,」区切りの文字列を分割
        string[] parsedMsgs = message.Split(",");

        // 受信したデータ数が想定よりも少なければ正しいデータが得られないのでリターン
        if (parsedMsgs.Length < (int)DataOrder.DataSize) return;

        // 受信した文字列をfloatに変換
        for (int i = 0; i < (int)DataOrder.DataSize; i++)
        {
            if (float.TryParse(parsedMsgs[i], out float value))
            {
                _ReceiveDataBuffer[i] = value;
            }
            else
            {
                // 変換に失敗したらデータが正しくないのでリターン
                return;
            }
        }

        // 受信したデータをもとに回転をGameObjectに反映
        _GyroRotate.x = -_ReceiveDataBuffer[(int)DataOrder.Pitch];
        _GyroRotate.y = -_ReceiveDataBuffer[(int)DataOrder.Yaw];
        _GyroRotate.z = -_ReceiveDataBuffer[(int)DataOrder.Roll];
        _Rb.MoveRotation(Quaternion.Euler(_GyroRotate));
    }
}
