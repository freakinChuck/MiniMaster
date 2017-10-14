using MiniMaster._Helper;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniMaster.Acolyte
{
    public class CreateAbsenceViewModel : INotifyPropertyChanged
    {
        public CreateAbsenceViewModel()
        {
            LoadServices();
        }
        public Action CloseAction { get; set; }

        private void LoadServices()
        {
            AllServicesForAbsences = new BindingList<AbsenceForServiceViewModel>(Workspace.CurrentData.Services
                //.Where(x => x.DateAndTime > DateTime.Today)
                .Select(x =>
                {
                    return new AbsenceForServiceViewModel
                    {
                        ServiceId = x.Id,
                        ServiceDateAndTime = x.DateAndTime
                    };
                })
                .OrderByDescending(x => x.ServiceDateAndTime)
                .ToList());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllServicesForAbsences)));
        }

        public BindingList<AbsenceForServiceViewModel> AllServicesForAbsences { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AbsenceByServiceCommand
        {
            get { return new BindingCommand(x => AbsenceByService()); }
        }

        public string AcolyteId { get; internal set; }

        private void AbsenceByService()
        {
            foreach (var selectedAbsence in AllServicesForAbsences.Where(x => x.HasAbsenceForServiceSelected))
            {
                var newAbsence = AbsenceModel.CreateNewAbsence(this.AcolyteId);
                newAbsence.DateAndTime = selectedAbsence.ServiceDateAndTime;
                newAbsence.WholeDay = false;
            }
            this.CloseAction?.Invoke();
        }



        public class AbsenceForServiceViewModel
        {
            public string ServiceId { get; set; }
            public DateTime ServiceDateAndTime { get; set; }
            public bool HasAbsenceForServiceSelected { get; set; }
            public string ServiceDateAndTimeString
            {
                get
                {
                    return this.ServiceDateAndTime.ToString("dd.MM.yyyy HH.mm");
                }
            }

        }
    }
}
