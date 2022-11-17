#ifndef ROTARYBUTTONDB_ROTARYBUTTON_H
#define ROTARYBUTTONDB_ROTARYBUTTON_H

#include <Keyboard.h>
#include <Bounce2.h>
#include <EEPROM.h>

class Key
{
public:
    Key(byte pin, byte key, byte keyMod1, byte keyMod2);

    void update();
    void setKeys(byte key, byte keyMod1, byte keyMod2);

private:
    Bounce _bounce;

    byte _key;
    byte _keyMod1;
    byte _keyMod2;
};

#endif
