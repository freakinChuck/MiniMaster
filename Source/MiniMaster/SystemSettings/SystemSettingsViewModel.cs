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

        private readonly SystemSettingsModel storageSettings;

        public bool ShowPastAbsences
        {
            get { return storageSettings.ShowPastAbsences; }
            set
            {
                storageSettings.ShowPastAbsences = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowPastAbsences)));
            }
        }
        public bool ShowPastServices
        {
            get { return storageSettings.ShowPastServices; }
            set
            {
                storageSettings.ShowPastServices = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowPastServices)));
            }
        }
        public bool ShowServicesWithoutJobsInAbsenceWindow
        {
            get { return storageSettings.ShowServicesWithoutJobsInAbsenceWindow; }
            set
            {
                storageSettings.ShowServicesWithoutJobsInAbsenceWindow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowServicesWithoutJobsInAbsenceWindow)));
            }
        }
        public bool ShowPastServicesInAbsenceWindow
        {
            get { return storageSettings.ShowPastServicesInAbsenceWindow; }
            set
            {
                storageSettings.ShowPastServicesInAbsenceWindow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowPastServicesInAbsenceWindow)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}