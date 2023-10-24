using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �|�[�g�I��UI�ƃV���A���Ǘ��̒����
/// </summary>
public class PortSelectPresenter : MonoBehaviour
{
    [SerializeField] SerialHandle _Serial;          // �|�[�g�I���𔽉f������V���A���Ǘ�
    [SerializeField] PortSelector _PortSelect;      // �|�[�g�I�����s��UI

    // Start is called before the first frame update
    void Start()
    {
        _PortSelect.SelectPortName
                .Subscribe(x =>
                {
                    // �I�𒆂̃|�[�g�����ύX���ꂽ��J���Ȃ����悤�ɓo�^
                    _Serial.UpdatePort(x);
                }).AddTo(this);
    }
}
