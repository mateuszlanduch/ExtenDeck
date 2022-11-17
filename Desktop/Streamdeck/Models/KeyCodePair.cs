using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Streamdeck.Models
{
    public class KeyCodePair : ObservableObject
    {
        private string key;

        public string Key
        {
            get { return key; }
            set
            {
                SetProperty(ref key, value);
            }
        }
        private byte code;

        public byte Code
        {
            get { return code; }
            set
            {
                SetProperty(ref code, value);
            }
        }
    }
}
