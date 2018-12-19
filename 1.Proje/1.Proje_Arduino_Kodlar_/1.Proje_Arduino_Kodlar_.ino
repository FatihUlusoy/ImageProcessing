void setup() {
  pinMode(2, OUTPUT);
  pinMode(3, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(5, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(10, OUTPUT);

  Serial.begin(9600);

}

String x;
char *y;
int z;

void loop() {
  if (Serial.available())
  {
    x = Serial.readString();
    int str_len = x.length() + 1;
    char char_array[str_len];
    x.toCharArray(char_array, str_len);
    z = atoi(char_array);
    digitalWrite(z,HIGH);
    delay (50);
    for (int a = 2; a < 11; a++)
    {
    if  (a != z)
    {
      digitalWrite(a,LOW);
      Serial.println(a);
      }
    }
  }

}
