#include "Key.h"
#define KEYDELAY 3

Key::Key(byte pin, byte key, byte keyMod1, byte keyMod2)
{
    pinMode(pin, INPUT_PULLUP);
    _bounce = Bounce();
    _bounce.attach(pin);
    _bounce.interval(20);

    _key = key;
    _keyMod1 = keyMod1;
    _keyMod2 = keyMod2;
}

void Key::update()
{
    _bounce.update();
    if (_bounce.fell())
    {
        if (_keyMod1 != 0)
        {
            Keyboard.press(_keyMod1);
            delay(KEYDELAY);
        }
        if (_keyMod2 != 0)
        {
            Keyboard.press(_keyMod2);
            delay(KEYDELAY);
        }
        if (_key != 0)
        {
            Keyboard.press(_key);
            delay(KEYDELAY);
        }
        Keyboard.releaseAll();
    }
}

void Key::setKeys(byte key, byte keyMod1, byte keyMod2)
{
    _key = key;
    _keyMod1 = keyMod1;
    _keyMod2 = keyMod2;
}