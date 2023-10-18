using System.Collections.Concurrent;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

/// <summary>
/// シリアル通信管理クラス
/// </summary>
public class SerialHandle : MonoBehaviour
{
    // データ受信時のイベント
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    [SerializeField] string PortName = "COMxx";         // ポート名
    [SerializeField] int BaudRate = 9600;               // ボーレート(BluetoothSerialでのボーレートの定義は不明)

    private ConcurrentQueue<string> _ReceivedQueue;     // 受信バッファ(Queueのスレッドセーフ版)

    private SerialPort _Serial;                         // シリアルポート管理
    private Thread _ReadThread;                         // 受信スレッド管理
    private bool _IsRunning = false;                    // 受信スレッドの生存確認フラグ

    void Awake()
    {
        _ReceivedQueue = new ConcurrentQueue<string>();
        Open();
    }

    void Update()
    {
        // 前フレームから受け取った分だけデータ受信イベントを発行
        while (_ReceivedQueue.TryDequeue(out string message))
        {
            OnDataReceived(message);
        }
    }

    void OnDestroy()
    {
        Close();
    }

    /// <summary>
    /// シリアルポートを開く
    /// </summary>
    private void Open()
    {
        // シリアルポートの初期化
        _Serial = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
        _Serial.Open();

        // スレッドの開始処理
        _IsRunning = true;
        _ReadThread = new Thread(Read);
        _ReadThread.Start();
    }

    /// <summary>
    /// シリアルポートを閉じる
    /// </summary>
    private void Close()
    {
        // スレッドの生存フラグを倒す
        _IsRunning = false;

        // 受信スレッドの終了待ち
        if (_ReadThread != null && _ReadThread.IsAlive)
        {
            _ReadThread.Join();
        }

        // シリアルポートのクローズ処理
        if (_Serial != null && _Serial.IsOpen)
        {
            _Serial.Close();
            _Serial.Dispose();
        }
    }

    /// <summary>
    /// シリアルポートからの読み出し（別スレッドでの動作）
    /// </summary>
    private void Read()
    {
        // 生存フラグが立っていて、シリアルポートが正常に動く限りループ
        while (_IsRunning && _Serial != null && _Serial.IsOpen)
        {
            try
            {
                // 受信データが溜まっていたら
                if (_Serial.BytesToRead > 0)
                {
                    // 改行文字まで読み込み、キューに積む
                    string recievedStr = _Serial.ReadLine();
                    _ReceivedQueue.Enqueue(recievedStr);
                }
            }
            catch (System.Exception e)
            {
                // 例外が発生したら出力
                Debug.LogWarning(e.Message);
            }
        }
    }

    /// <summary>
    /// シリアルポートへの書き込み
    /// </summary>
    /// <param name="message">送信する文字列</param>
    public void Write(string message)
    {
        try
        {
            _Serial.Write(message);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}
