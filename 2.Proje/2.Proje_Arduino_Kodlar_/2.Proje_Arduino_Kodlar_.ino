#include <StepperMotor.h>

StepperMotor motor(8,9,10,11);
String x;
char *y;
int z;


void setup() {
  Serial.begin(9600);
  motor.setStepDuration(1);

}

void loop() 
{
  if (Serial.available())
  {
    x = Serial.readString();
    int str_len = x.length() + 1;
    char char_array[str_len];
    x.toCharArray(char_array, str_len);
    z= atoi(char_array);
  }

     if (z == 3)
     {
      motor.step(40);
      delay(10);
     }
     if ( z == 1)
     {
      sag();
     }
     if ( z == 2)
     {
      digitalWrite(8, LOW);
      delay(10);
      digitalWrite(9, LOW);
      delay(10);
      digitalWrite(10, LOW);
      delay(10);
      digitalWrite(11, LOW);
      delay(10);
     }
  }
  void sag()
  {
    if (Serial.available())
    {
      x = Serial.readString();
      int str_len = x.length() + 1;
      char char_array[str_len];
      x.toCharArray(char_array, str_len);
      z = atoi(char_array);
    }
    if ( z == 1)
    {
      motor.step(-40);
      delay(10);
    }
  }
