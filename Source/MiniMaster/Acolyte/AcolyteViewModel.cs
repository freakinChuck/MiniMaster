﻿using MiniMaster.Storage.Model;
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

        private readonly AcolyteModel storageAcolyte;

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
                return this.GetAbsences();
            }
        }
        private List<AbsenceViewModel> GetAbsences()
        {
            bool showPast = Workspace.CurrentData.Settings.ShowPastAbsences;
            return Workspace.CurrentData.Absences.Where(x => x.AcolyteId == this.Id).Where(x => showPast || x.DateAndTime >= DateTime.Today).OrderByDescending(x => x.DateAndTime)
                .Select(x => { var model = new AbsenceViewModel(x); model.PropertyChanged += Model_PropertyChanged; return model; }).ToList();
        }

        public List<ContinousAbsenceViewModel> ContinousAbsences
        {
            get
            {
                return this.GetContinousAbsences();
            }
        }
        private List<ContinousAbsenceViewModel> GetContinousAbsences()
        {
            return Workspace.CurrentData.ContinousAbsences.Where(x => x.AcolyteId == this.Id).OrderBy(x => x.Day).ThenBy(x => x.Time)
                .Select(x => { var model = new ContinousAbsenceViewModel(x); model.PropertyChanged += Model_PropertyChanged; return model; }).ToList();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Absences)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContinousAbsences)));
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