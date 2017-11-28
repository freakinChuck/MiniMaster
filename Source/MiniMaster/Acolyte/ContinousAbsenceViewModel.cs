using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;
using MiniMaster._Helper;

namespace MiniMaster.Acolyte
{
    public class ContinousAbsenceViewModel : INotifyPropertyChanged
    {
        public ContinousAbsenceViewModel(ContinousAbsenceModel storageAbsence)
        {
            this.storageAbsence = storageAbsence;
            this.PropertyChanged += AbsenceViewModell_PropertyChanged;
        }

        private void AbsenceViewModell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private ContinousAbsenceModel storageAbsence;

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
            get { return GetTagname(storageAbsence.Day); }
            
        }
        public string TimeString
        {
            get { return string.Format("{0:00}:{1:00}", TimeSpan.Parse(string.IsNullOrEmpty(storageAbsence.Time) ? "00:00" : storageAbsence.Time).Hours, TimeSpan.Parse(string.IsNullOrEmpty(storageAbsence.Time) ? "00:00" : storageAbsence.Time).Minutes); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand RemoveAbsenceFromModelCommand
        {
            get { return new BindingCommand(x => RemoveAbsenceFromModel()); }
        }
        internal void RemoveAbsenceFromModel()
        {
            Workspace.CurrentData.ContinousAbsences.Remove(storageAbsence);
            Workspace.RegisterDataChanged();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
        }



        public static string GetTagname(DayOfWeek tag)
        {
            switch (tag)
            {
                case DayOfWeek.Friday:
                    return "Freitag";
                case DayOfWeek.Monday:
                    return "Montag";
                case DayOfWeek.Saturday:
                    return "Samstag";
                case DayOfWeek.Sunday:
                    return "Sonntag";
                case DayOfWeek.Thursday:
                    return "Donnerstag";
                case DayOfWeek.Tuesday:
                    return "Dienstag";
                case DayOfWeek.Wednesday:
                    return "Mittwoch";
            }

            return "";

        }
    }
}