# M5StickC Plus + Bluetooth + Unityテスト
## テスト環境
#### マイコン
- M5StickC Plus
  - アーキテクチャ : ESP32 Pico
  - Bluetooth : Bluetooth4.0(Bluetooth Classic)

#### 開発環境
- Windows10
- Unity 2021.3.24f1
- Arduino IDE 1.8.19

#### 使用ライブラリ
- M5StickCPlus(v0.0.8)
- BluetoothSerial
<br><br>
## テスト内容
M5StickC PlusとUnityの通信を無線化し、どれくらいの精度でデータを送れるかをテストした。

M5StickのIMUで計測した姿勢データをUnityのCubeにリアルタイム反映させる。

（ただし補正やキャリブレーションに関しては特に使用していないため、センサー精度としてはよろしくない。）
<br><br>
## テスト結果（環境もあるかと思われるので、参考程度）
BluetoothSerialではデータの遅延が発生し、カクつきが発生した。

そもそもBluetoothSerialはBluetooth4.xまでしか対応しておらず、Bluetooth5.x（いわゆるBLE）ではUnityと通信を行うためにDLLの作成などが必須になってくるため、少々ハードルが上がる。

どのみちESP32S3などのアーキテクチャになるとBLE対応になってくるため、無線通信を行うためには別途対応が必要になる。