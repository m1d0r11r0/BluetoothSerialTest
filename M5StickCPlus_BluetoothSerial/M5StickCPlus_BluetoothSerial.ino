#include <M5StickCPlus.h>
#include "BluetoothSerial.h"

//#define _USE_UART

BluetoothSerial SerialBT;

// --------------------------------------------------------
// 変数・定数定義部
// --------------------------------------------------------
float gyro[3];                                                // 角速度測定値格納用（X、Y、Z）

float roll  = 0.0f;                                           // ロール格納用
float pitch = 0.0f;                                           // ピッチ格納用
float yaw   = 0.0f;                                           // ヨー格納用

char buf[64];                                                 // シリアル送信用文字列バッファ

const String dev_name = "M5StickCPlus_IMU_Data";              // Bluetoothのデバイス名として表示される名前

// --------------------------------------------------------
// 関数定義部
// --------------------------------------------------------

// 加速度、角速度　測定値取得用　関数
void ReadIMU(){
  M5.Imu.getAhrsData(&gyro[0], &gyro[1], &gyro[2]);
  roll  = gyro[0];
  pitch = gyro[1];
  yaw   = gyro[2];
}

void DrawDisplay(){
  M5.Lcd.setCursor(15, 80);
  M5.Lcd.printf(" %5.1f   %5.1f   %5.1f   ", roll, pitch, yaw);
}

void SendSerial(){
  sprintf(buf, "%2.1f,%2.1f,%2.1f", roll, pitch, yaw);
#ifdef _USE_UART
  Serial.println(buf);
#else
  SerialBT.println(buf);
#endif
}

void setup() {
  // M5StickCの初期化と動作設定　Initialization and operation settings of M5StickC.
  M5.begin(); // 開始
#ifdef _USE_UART
  Serial.begin(9600);
#else
  SerialBT.begin(dev_name);
#endif
  delay(500);

  // IMUの初期化
  M5.Imu.Init();

  // M5StickC Plusのディスプレイ上にデータ表示するために使用
  M5.Lcd.setRotation(3);  
  M5.Lcd.fillScreen(BLACK);
  M5.Lcd.setTextFont(2);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(15, 10);
  M5.Lcd.println(dev_name);
  M5.Lcd.setTextFont(4);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(15, 40);
  M5.Lcd.println("  Roll   Pitch    Yaw");

}

void loop() {
  M5.update(); // 開始
  ReadIMU();
  DrawDisplay();
  SendSerial();
  delay(10);

}
