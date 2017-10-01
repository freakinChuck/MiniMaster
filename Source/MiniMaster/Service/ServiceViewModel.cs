using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;

namespace MiniMaster.Service
{
    public class ServiceViewModel : INotifyPropertyChanged
    {
        public ServiceViewModel(ServiceModel storageService)
        {
            this.storageService = storageService;
            this.PropertyChanged += ServiceViewModel_PropertyChanged;
        }

        private void ServiceViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        internal ServiceModel storageService;

        public string Id => storageService.Id;

        public DateTime DateAndTime
        {
            get { return storageService.DateAndTime; }
            set
            {
                storageService.DateAndTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateAndTime"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Date"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateAndTimeString"));
            }
        }

        public string DateAndTimeString => this.DateAndTime.ToString("dd.MM.yyyy HH:mm");

        public DateTime Date
        {
            get { return storageService.DateAndTime.Date; }
            set
            {
                var time = this.DateAndTime.TimeOfDay;
                this.DateAndTime = value.Add(time);
            }
        }

        public TimeSpan Time
        {
            get { return storageService.DateAndTime.TimeOfDay; }
            set
            {
                var date = this.DateAndTime.Date;
                this.DateAndTime = date.Add(value);
            }
        }

        public bool OnlyOlderAcolytes
        {
            get { return storageService.OnlyOlderAcolytes; }
            set
            {
                this.storageService.OnlyOlderAcolytes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OnlyOlderAcolytes"));
            }
        }
        public bool TwoOlderAcolytes
        {
            get { return storageService.TwoOlderAcolytes; }
            set
            {
                this.storageService.TwoOlderAcolytes = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TwoOlderAcolytes"));
            }
        }


        

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveServiceFromModel()
        {
            Workspace.CurrentData.Services.Remove(storageService);
            Workspace.RegisterDataChanged();
        }
    }
}