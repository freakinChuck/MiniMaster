using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;
using MiniMaster._Helper;

namespace MiniMaster.Acolyte
{
    public class AbsenceViewModel : INotifyPropertyChanged
    {
        public AbsenceViewModel(AbsenceModel storageAbsence)
        {
            this.storageAbsence = storageAbsence;
            this.PropertyChanged += AbsenceViewModell_PropertyChanged;
        }

        private void AbsenceViewModell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private readonly AbsenceModel storageAbsence;

        public string Id => storageAbsence.Id;

        public string AcolyteId
        {
            get { return storageAbsence.AcolyteId; }
            set
            {
                storageAbsence.AcolyteId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcolyteId"));
            }
        }

        public string DateString
        {
            get { return storageAbsence.DateAndTime.ToShortDateString(); }
            
        }
        public string TimeString
        {
            get { return storageAbsence.WholeDay ? "ganzer Tag" : storageAbsence.DateAndTime.ToString("HH:mm"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand RemoveAbsenceFromModelCommand
        {
            get { return new BindingCommand(x => RemoveAbsenceFromModel()); }
        }
        internal void RemoveAbsenceFromModel()
        {
            Workspace.CurrentData.Absences.Remove(storageAbsence);
            Workspace.RegisterDataChanged();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
        }
    }
}