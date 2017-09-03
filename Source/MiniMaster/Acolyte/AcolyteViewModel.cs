using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;

namespace MiniMaster.Acolyte
{
    public class AcolyteViewModel : INotifyPropertyChanged
    {
        public AcolyteViewModel(AcolyteModel storageAcolyte)
        {
            this.storageAcolyte = storageAcolyte;
            this.PropertyChanged += AcolyteViewModel_PropertyChanged;
        }

        private void AcolyteViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private AcolyteModel storageAcolyte;

        public string Id => storageAcolyte.Id;

        public string Name
        {
            get { return storageAcolyte.Name; }
            set
            {
                storageAcolyte.Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }

        public string Firstname
        {
            get { return storageAcolyte.Firstname; }
            set
            {
                storageAcolyte.Firstname = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Firstname"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayName"));
            }
        }
        public DateTime? Entry
        {
            get { return storageAcolyte.Entry; }
            set
            {
                storageAcolyte.Entry = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryYear"));
            }
        }

        public string DisplayName => $"{Name} {Firstname}";

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveAcolyteFromModel()
        {
            Workspace.CurrentData.Acolytes.Remove(storageAcolyte);
            Workspace.RegisterDataChanged();
        }
    }
}