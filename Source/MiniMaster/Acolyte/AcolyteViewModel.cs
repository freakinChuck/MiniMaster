using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using System.Linq;
using MiniMaster.Storage;
using System.Collections.Generic;

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
        public string FamilyKey
        {
            get { return storageAcolyte.FamilyKey; }
            set
            {
                storageAcolyte.FamilyKey = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FamilyKey)));
            }
        }
        public string DisplayName => $"{Name} {Firstname}";

        public List<AbsenceViewModel> Absences
        {
            get
            {
                return Workspace.CurrentData.Absences.Where(x => x.AcolyteId == this.Id).OrderByDescending(x => x.DateAndTime)
                    .Select(x => { var model = new AbsenceViewModel(x); model.PropertyChanged += Model_PropertyChanged; return model; }).ToList();
            }
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Absences)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveAcolyteFromModel()
        {
            Workspace.CurrentData.Acolytes.Remove(storageAcolyte);
            Workspace.RegisterDataChanged();
        }
    }
}