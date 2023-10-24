using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UniRx;
using TMPro;

/// <summary>
/// シリアルポート選択UI
/// </summary>
public class PortSelector : MonoBehaviour
{
    // 選択ポート名
    public IReadOnlyReactiveProperty<string> SelectPortName => _SelectPortName;
    private readonly StringReactiveProperty _SelectPortName = new StringReactiveProperty();

    private TMP_Dropdown _SelectUI;             // ポート選択用ドロップダウンUI
    private List<string> _DropdownLabels;       // ドロップダウンに表示する文字列を羅列した配列

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
        // ポート未選択の場合は変更を行わない
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
