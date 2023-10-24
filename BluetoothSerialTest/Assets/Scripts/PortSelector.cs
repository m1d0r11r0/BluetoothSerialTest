using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UniRx;
using TMPro;

/// <summary>
/// �V���A���|�[�g�I��UI
/// </summary>
public class PortSelector : MonoBehaviour
{
    // �I���|�[�g��
    public IReadOnlyReactiveProperty<string> SelectPortName => _SelectPortName;
    private readonly StringReactiveProperty _SelectPortName = new StringReactiveProperty();

    private TMP_Dropdown _SelectUI;             // �|�[�g�I��p�h���b�v�_�E��UI
    private List<string> _DropdownLabels;       // �h���b�v�_�E���ɕ\�����镶����𗅗񂵂��z��

    private void Awake()
    {
        _DropdownLabels = new List<string>();
        _SelectUI = GetComponent<TMP_Dropdown>();
        InitDropdown();
        _SelectUI.onValueChanged.AddListener(SelectPortChanged);
    }

    private void OnDestroy()
    {
        _SelectPortName.Dispose();
    }

    private void SelectPortChanged(int idx)
    {
        // �|�[�g���I���̏ꍇ�͕ύX���s��Ȃ�
        if (idx == 0) return;

        _SelectPortName.Value = _SelectUI.options[idx].text;
    }

    private void InitDropdown()
    {
        _SelectUI.ClearOptions();

        string[] ports = SerialPort.GetPortNames();
        _DropdownLabels.Clear();
        _DropdownLabels.Add("Select Port");
        for (int i = 0; i < ports.Length; i++)
        {
            _DropdownLabels.Add(ports[i]);
        }
        _SelectUI.AddOptions(_DropdownLabels);
    }
}
