using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;
using MiniMaster.Storage.Model.ServiceTemplate;

namespace MiniMaster.ServiceTemplate
{
    public class ServiceTemplateViewModel : INotifyPropertyChanged
    {
        public ServiceTemplateViewModel(ServiceTemplateModel storageServiceTemplate)
        {
            this.storageServiceTemplate = storageServiceTemplate;
            this.PropertyChanged += ServiceTemplateViewModel_PropertyChanged;
        }

        private void ServiceTemplateViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private ServiceTemplateModel storageServiceTemplate;

        public string Id => storageServiceTemplate.Id;

        public DayOfWeek Day
        {
            get { return storageServiceTemplate.Day; }
            set
            {
                storageServiceTemplate.Day = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Day"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DayOfWeekString"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayString"));
            }
        }
        public string DayOfWeekString
        {
            get
            {
                var culture = new System.Globalization.CultureInfo("de-CH");
                return culture.DateTimeFormat.GetDayName(Day);
            }
        }

        public TimeSpan Time
        {
            get { return storageServiceTemplate.Time; }
            set
            {
                storageServiceTemplate.Time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayString"));
            }
        }

        public string DisplayString => string.Format("{0} {1:00}:{2:00}", DayOfWeekString, Time.Hours, Time.Minutes);

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveServiceTemplateFromModel()
        {
            Workspace.CurrentData.ServiceTemplates.Remove(storageServiceTemplate);
            Workspace.RegisterDataChanged();
        }
    }
}