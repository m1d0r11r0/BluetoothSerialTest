using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ポート選択UIとシリアル管理の仲介役
/// </summary>
public class PortSelectPresenter : MonoBehaviour
{
    [SerializeField] SerialHandle _Serial;          // ポート選択を反映させるシリアル管理
    [SerializeField] PortSelector _PortSelect;      // ポート選択を行うUI

    // Start is called before the first frame update
    void Start()
    {
        _PortSelect.SelectPortName
                .Subscribe(x =>
                {
                    // 選択中のポート名が変更されたら開きなおすように登録
                    _Serial.UpdatePort(x);
                }).AddTo(this);
    }
}
