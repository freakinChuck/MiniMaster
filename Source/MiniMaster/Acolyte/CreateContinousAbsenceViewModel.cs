using MiniMaster._Helper;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniMaster.Acolyte
{
    public class CreateContinousAbsenceViewModel
    {
        public CreateContinousAbsenceViewModel()
        {
            SelectedDayOfWeek = DayOfWeek.Sunday;
        }

        public string AcolyteId { get; set; }
        public Action CloseAction { get; set; }

        public DayOfWeek SelectedDayOfWeek { get; set; }
        public TimeSpan SelectedTime { get; set; }

        public ICommand AddContinousAbsenceCommand
        {
            get { return new BindingCommand(x => AddContinousAbsence()); }
        }

        private void AddContinousAbsence()
        {
            var newAbsence = ContinousAbsenceModel.CreateNewContinousAbsence(this.AcolyteId);
            newAbsence.Day = SelectedDayOfWeek;
            newAbsence.Time = string.Format("{0:00}:{1:00}", SelectedTime.Hours, SelectedTime.Minutes);
            this.CloseAction?.Invoke();
        }

    }
}
