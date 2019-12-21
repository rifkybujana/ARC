//mpu6050
#include<Wire.h>
const int MPU = 0x68;
int16_t AcX, AcY, AcZ, Tmp, GyX, GyY, GyZ;
//bluetooth module
#include <SoftwareSerial.h>        
SoftwareSerial BTSerial(10, 11); // RX | TX


//Flex Sensor
const int flexPin1 = A1; //jari tengah
const int flexPin2 = A2; //jari telunjuk 
const int flexPin3 = A3; //jari jempol

//Gyro sensor

int flexValue1;
int flexValue2;
int flexValue3;
int Hand;

void setup()
{
  Serial.begin(9600);
  BTSerial.begin(38400);

  Wire.begin();
  Wire.beginTransmission(MPU);
  Wire.write(0x6B);
  Wire.write(0);
  Wire.endTransmission(true);
  Serial.begin(9600);

}

void getHand() {
  int out1 = analogRead(flexPin1);
  flexValue1 = map(out1, 0, 200, 0, 100);
  flexValue1 = constrain(flexValue1, 0, 100);

  int out2 = analogRead(flexPin2);
  flexValue2 = map(out2, 0, 200, 0, 100);
  flexValue2 = constrain(flexValue2, 0, 100);

  int out3 = analogRead(flexPin3);
  flexValue3 = map(out3, 0, 200, 0, 100);
  flexValue3 = constrain(flexValue3, 0, 100);

  /*
   * 0 = nothing
   * 1 = start
   * 2 = move
   * 3 = grep
   * 4 = choose
   */

  if (flexValue3 > 75 && flexValue2 > 75 && flexValue1 < 75) {
    Hand = 1;
  } else if (flexValue3 < 75 && flexValue2 < 75 && flexValue1 < 75) {
    Hand = 2;
  } else if (flexValue3 < 75 && flexValue2 < 75 && flexValue1 > 75) {
    Hand = 3;
  } else if (flexValue3 < 75 && flexValue2 > 75 && flexValue1 < 75) {
    Hand = 4;
  } else {
    Hand = 0;
  }
}

void getPose() {

  Wire.beginTransmission(MPU);
  Wire.write(0x3B);
  Wire.endTransmission(false);
  Wire.requestFrom(MPU, 12, true);
  AcX = Wire.read() << 8 | Wire.read();
  AcX = map(AcX, -5000, 5000, -50, 50);
  AcY = Wire.read() << 8 | Wire.read();
  AcY = map(AcY, -5000, 5000, -50, 50);
  AcZ = Wire.read() << 8 | Wire.read();
  AcZ = map(AcZ, -5000, 5000, -50, 50); 
  GyX = Wire.read() << 8 | Wire.read();
  GyX = map(GyX, -5000, 5000, -50, 50); 
  GyY = Wire.read() << 8 | Wire.read();
  GyY = map(GyY, -5000, 5000, -50, 50); 
  GyZ = Wire.read() << 8 | Wire.read();
  GyZ = map(GyZ, -5000, 5000, -50, 50); 
}

void loop()
{
  //Flex sensor
  getHand();

  //gyro sensor
  getPose();

  Serial.print(AcX);
  Serial.print(",");
  Serial.print(AcY);
  Serial.print(",");
  Serial.print(AcZ);
  Serial.print(",");
  Serial.print(GyX);
  Serial.print(",");
  Serial.print(GyY);
  Serial.print(",");
  Serial.print(GyZ);
  Serial.print(",");
  //Serial.print(flexValue3);
  //Serial.print(",");
  //Serial.print(flexValue2);
  //Serial.print(",");
  //Serial.print(flexValue1);
  //Serial.print(",");
  Serial.println(Hand);

  
  delay(500);
}
