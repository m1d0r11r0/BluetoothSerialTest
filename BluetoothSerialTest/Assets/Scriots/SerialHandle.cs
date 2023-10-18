using System.Collections.Concurrent;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

/// <summary>
/// �V���A���ʐM�Ǘ��N���X
/// </summary>
public class SerialHandle : MonoBehaviour
{
    // �f�[�^��M���̃C�x���g
    public delegate void SerialDataReceivedEventHandler(string message);
    public event SerialDataReceivedEventHandler OnDataReceived;

    [SerializeField] string PortName = "COMxx";         // �|�[�g��
    [SerializeField] int BaudRate = 9600;               // �{�[���[�g(BluetoothSerial�ł̃{�[���[�g�̒�`�͕s��)

    private ConcurrentQueue<string> _ReceivedQueue;     // ��M�o�b�t�@(Queue�̃X���b�h�Z�[�t��)

    private SerialPort _Serial;                         // �V���A���|�[�g�Ǘ�
    private Thread _ReadThread;                         // ��M�X���b�h�Ǘ�
    private bool _IsRunning = false;                    // ��M�X���b�h�̐����m�F�t���O

    void Awake()
    {
        _ReceivedQueue = new ConcurrentQueue<string>();
        Open();
    }

    void Update()
    {
        // �O�t���[������󂯎�����������f�[�^��M�C�x���g�𔭍s
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
    /// �V���A���|�[�g���J��
    /// </summary>
    private void Open()
    {
        // �V���A���|�[�g�̏�����
        _Serial = new SerialPort(PortName, BaudRate, Parity.None, 8, StopBits.One);
        _Serial.Open();

        // �X���b�h�̊J�n����
        _IsRunning = true;
        _ReadThread = new Thread(Read);
        _ReadThread.Start();
    }

    /// <summary>
    /// �V���A���|�[�g�����
    /// </summary>
    private void Close()
    {
        // �X���b�h�̐����t���O��|��
        _IsRunning = false;

        // ��M�X���b�h�̏I���҂�
        if (_ReadThread != null && _ReadThread.IsAlive)
        {
            _ReadThread.Join();
        }

        // �V���A���|�[�g�̃N���[�Y����
        if (_Serial != null && _Serial.IsOpen)
        {
            _Serial.Close();
            _Serial.Dispose();
        }
    }

    /// <summary>
    /// �V���A���|�[�g����̓ǂݏo���i�ʃX���b�h�ł̓���j
    /// </summary>
    private void Read()
    {
        // �����t���O�������Ă��āA�V���A���|�[�g������ɓ������胋�[�v
        while (_IsRunning && _Serial != null && _Serial.IsOpen)
        {
            try
            {
                // ��M�f�[�^�����܂��Ă�����
                if (_Serial.BytesToRead > 0)
                {
                    // ���s�����܂œǂݍ��݁A�L���[�ɐς�
                    string recievedStr = _Serial.ReadLine();
                    _ReceivedQueue.Enqueue(recievedStr);
                }
            }
            catch (System.Exception e)
            {
                // ��O������������o��
                Debug.LogWarning(e.Message);
            }
        }
    }

    /// <summary>
    /// �V���A���|�[�g�ւ̏�������
    /// </summary>
    /// <param name="message">���M���镶����</param>
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
