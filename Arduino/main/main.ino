#include "src/Key.h"
#include <Keyboard.h>

#define SERIALDATABUFFER 5
#define NUMBEROFKEYS 15

Key keys[] = {Key(2, KEY_F10, 0, 0), Key(3, KEY_F11, 0, 0), Key(4, KEY_F12, 0, 0), Key(5, KEY_F13, 0, 0), Key(6, KEY_F14, 0, 0),
              Key(A3, KEY_F15, 0, 0), Key(A2, KEY_F16, 0, 0), Key(7, KEY_F17, 0, 0), Key(8, KEY_F18, 0, 0), Key(9, KEY_F19, 0, 0),
              Key(A1, KEY_F20, 0, 0), Key(A0, KEY_F21, 0, 0), Key(15, KEY_F22, 0, 0), Key(14, KEY_F23, 0, 0), Key(16, KEY_F24, 0, 0)};

byte serialData[SERIALDATABUFFER];

void setup()
{
    Serial.begin(9600);

    for (byte i = 0; i < NUMBEROFKEYS; i++)
    {
        if (EEPROM.read(0) == 69)
        {
            int eepromPointer = i * 3 + 1;
            keys[i].setKeys(EEPROM.read(eepromPointer), EEPROM.read(eepromPointer+1), EEPROM.read(eepromPointer+2));
        }
    }
}

void loop()
{
    readSerialData();
    for (byte i = 0; i < NUMBEROFKEYS; i++)
    {
        keys[i].update();
    }
}

void readSerialData()
{
    if (Serial.available() > 0)
    {
        Serial.readBytes(serialData, SERIALDATABUFFER);

        keys[serialData[0]].setKeys(serialData[1], serialData[2], serialData[3]);

        EEPROM.write(0, (byte)69);

        int eepromPointer = serialData[0] * 3 + 1;
        EEPROM.write(eepromPointer, (byte)serialData[1]);
        EEPROM.write(eepromPointer+1, (byte)serialData[2]);
        EEPROM.write(eepromPointer+2, (byte)serialData[3]);
    }
}