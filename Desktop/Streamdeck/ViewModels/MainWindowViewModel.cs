using Microsoft.Toolkit.Mvvm.ComponentModel;
using Streamdeck.Models;
using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Streamdeck.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        static SerialPort _serialPort;

        private bool uiEnabled = false;
        public bool UIEnabled
        {
            get { return uiEnabled; }
            set { SetProperty(ref uiEnabled, value); }
        }

        private string configurationUpdateStatus;
        public string ConfigurationUpdateStatus
        {
            get { return configurationUpdateStatus; }
            set { SetProperty(ref configurationUpdateStatus, value); }
        }

        private string[] serialPorts;
        public string[] SerialPorts
        {
            get { return serialPorts; }
            set { SetProperty(ref serialPorts, value); }
        }

        private string selectedSerialPort;
        public string SelectedSerialPort
        {
            get { return selectedSerialPort; }
            set
            {
                try
                {
                    SetProperty(ref selectedSerialPort, value);

                    _serialPort.Close();
                    _serialPort.PortName = selectedSerialPort;
                    _serialPort.Open();

                    ConfigurationUpdateStatus = "Make sure you've chosen proper connection";
                }
                catch
                {
                    ConfigurationUpdateStatus = "COM connection error";
                }
            }
        }

        private ObservableCollection<Key> keys;
        public ObservableCollection<Key> Keys
        {
            get { return keys; }
            set
            {
                SetProperty(ref keys, value);
            }
        }

        private Key selectedKey;
        public Key SelectedKey
        {
            get { return selectedKey ?? Keys?.FirstOrDefault(); }
            set
            {
                SetProperty(ref selectedKey, value);
            }
        }

        public MainWindowViewModel()
        {
            InitSerial();
            InitKeysConfiguration();
        }

        private void InitKeysConfiguration()
        {
            try
            {
                UIEnabled = false;
                ConfigurationUpdateStatus = "Loading configuration file keysConfiguration.json";
                if (File.Exists("keysConfiguration.json"))
                {
                    var json = File.ReadAllText("keysConfiguration.json");
                    Keys = JsonConvert.DeserializeObject<ObservableCollection<Key>>(json);
                    ConfigurationUpdateStatus = "Configuration file keysConfiguration.json has been loaded";
                    UIEnabled = true;
                }
                else
                {
                    ConfigurationUpdateStatus = "There is no configuration file keysConfiguration.json.";
                }
            }
            catch
            {
                ConfigurationUpdateStatus = "Cannot load configuration file keysConfiguration.json because of an error.";
            }
        }

        public void RefreshSerialPorts()
        {
            SerialPorts = SerialPort.GetPortNames();
        }

        public void SelectKey(object sender, EventArgs args)
        {
            if (sender is ComboBox)
            {
                SelectedKey.MainKey = GetKey(sender, args) ?? SelectedKey.MainKey;
            }
        }

        public void SelectMod(object sender, EventArgs args)
        {
            if (sender is ComboBox)
            {
                SelectedKey.ModKey = GetKey(sender, args) ?? SelectedKey.ModKey;
            }
        }

        public void SelectMod2(object sender, EventArgs args)
        {
            if (sender is ComboBox)
            {
                SelectedKey.ModKey2 = GetKey(sender, args) ?? SelectedKey.ModKey2;
            }
        }

        public async void SaveKeys(object sender, EventArgs args)
        {
            await SaveConfigurationToFile();
            await SendConfigurationToDevice();
        }

        private async Task SaveConfigurationToFile()
        {
            try
            {
                UIEnabled = false;
                ConfigurationUpdateStatus = "Saving configuration to file keysConfiguration.json";
                await Task.Delay(200);
                var keysJson = JsonConvert.SerializeObject(Keys);
                await File.WriteAllTextAsync("keysConfiguration.json", keysJson);
                ConfigurationUpdateStatus = "Configuration has been successfully saved to the keysConfiguration.json file";
                await Task.Delay(200);
            }
            catch
            {
                ConfigurationUpdateStatus = "An error occured while saving configuration the keysConfiguration.json file";
            }
            finally
            {
                UIEnabled = true;
            }
        }

        private async Task SendConfigurationToDevice()
        {
            try
            {
                UIEnabled = false;
                ConfigurationUpdateStatus = "Sending configuration to device";
                await Task.Delay(200);
                byte index = 0;
                foreach (var key in Keys)
                {
                    byte[] serialData = { index, key.MainKey.Code, key.ModKey.Code, key.ModKey2.Code, (byte)'\0' };
                    _serialPort.Write(serialData, 0, serialData.Length);
                    index++;
                }
                ConfigurationUpdateStatus = "Configuration has been successfully saved to the device";
            }
            catch
            {
                ConfigurationUpdateStatus = "An error occured while sending configuration to the device";
            }
            finally
            {
                UIEnabled = true;
            }
        }

        private KeyCodePair GetKey(object sender, EventArgs args)
        {
            if (args is SelectionChangedEventArgs selectionChangedEventArgs)
            {
                if(selectionChangedEventArgs?.AddedItems.Count>0)
                {
                    var key = selectionChangedEventArgs?.AddedItems[0];

                    if (key is KeyCodePair kc)
                    {
                        return new KeyCodePair { Code = kc.Code, Key = kc.Key };
                    }
                } 
            }

            return null;
        }

        private void InitSerial()
        {
            SerialPorts = SerialPort.GetPortNames();
            _serialPort = new SerialPort()
            {
                BaudRate = 9600,
                DtrEnable = true
            };

            if (SerialPorts.Length > 0)
            {
                SelectedSerialPort = SerialPorts.First();
            }
            else
            {
                ConfigurationUpdateStatus = "Device has been not found";
            }
        }
    }
}
