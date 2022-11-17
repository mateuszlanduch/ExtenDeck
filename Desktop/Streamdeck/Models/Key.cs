using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Streamdeck.Models
{
    public class Key : ObservableObject
    {
        private KeyCodePair mainKey;
        public KeyCodePair MainKey
        {
            get { return mainKey; }
            set
            {
                SetProperty(ref mainKey, value);
            }
        }

        private KeyCodePair modKey;
        public KeyCodePair ModKey
        {
            get { return modKey; }
            set
            {
                SetProperty(ref modKey, value);
            }
        }

        private KeyCodePair modKey2;
        public KeyCodePair ModKey2
        {
            get { return modKey2; }
            set
            {
                SetProperty(ref modKey2, value);
            }
        }

        public Key(string key, string modKey = null, string modKey2 = null)
        {
            MainKey = new KeyCodePair { Key = key };
            ModKey = new KeyCodePair { Key = modKey };
            ModKey2 = new KeyCodePair { Key = modKey2 };
        }

        public Key()
        {

        }
    }
}