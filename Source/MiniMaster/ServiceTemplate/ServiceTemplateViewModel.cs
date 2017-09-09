using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using System.Linq;
using MiniMaster.Storage;
using MiniMaster.Storage.Model.ServiceTemplate;
using System.Collections.Generic;

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

        internal ServiceTemplateModel storageServiceTemplate;

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

        public List<string> Jobs
        {
            get { return storageServiceTemplate.Jobs; }
        }
        public int JobCount(string jobId)
        {
            return this.Jobs.Count(x => x == jobId);
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