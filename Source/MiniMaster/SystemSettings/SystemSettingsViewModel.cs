using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;

namespace MiniMaster.SystemSettings
{
    public class SystemSettingsViewModel : INotifyPropertyChanged
    {
        public SystemSettingsViewModel(SystemSettingsModel storageSettings)
        {
            this.storageSettings = storageSettings;
            this.PropertyChanged += SystemSettingsViewModel_PropertyChanged;
        }

        private void SystemSettingsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private SystemSettingsModel storageSettings;

        public int AgeToCountAsOldAcolyte
        {
            get { return storageSettings.AgeToCountAsOldAcolyte; }
            set
            {
                storageSettings.AgeToCountAsOldAcolyte = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AgeToCountAsOldAcolyte"));
            }
        }
        public bool HerzjesuActive
        {
            get { return storageSettings.HerzjesuActive; }
            set
            {
                storageSettings.HerzjesuActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HerzjesuActive"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}