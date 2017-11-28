using Microsoft.Win32;
using MiniMaster._Helper;
using MiniMaster.Storage;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiniMaster.RessourceScheduling
{
    public class RessourceSchedulingViewModel
    {
        public RessourceSchedulingViewModel()
        {
            ScheduleFromDate = DateTime.Today;
            ScheduleUntilDate = Workspace.CurrentData.Services.Max(x => (DateTime?)(x.DateAndTime.Date)) ?? DateTime.Today;
        }

        public DateTime ScheduleFromDate { get; set; }
        public DateTime ScheduleUntilDate { get; set; }

        public ICommand GenerateScheduleCommand
        {
            get { return new BindingCommand((x) => GenerateSchedule()); }
        }
        private void GenerateSchedule()
        {
            using (RessourceScheduleManager manager = new RessourceScheduleManager())
            {
                manager.GenerateScheduleForPeriod(ScheduleFromDate, ScheduleUntilDate);
            }
        }

        public ICommand ExportScheduleCommand
        {
            get { return new BindingCommand((x) => ExportSchedule()); }
        }
        private void ExportSchedule()
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Einsatzplan");

                using (RessourceScheduleManager manager = new RessourceScheduleManager())
                {
                    manager.ExportScheduleForPeriod(worksheet, ScheduleFromDate, ScheduleUntilDate);
                }

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.AddExtension = true;
                dialog.DefaultExt = "*.xlsx";
                dialog.Filter = string.Format("Excel (*{0})|*{0}", "*.xlsx");
                var result = dialog.ShowDialog();
                if (result ?? false)
                {
                    package.SaveAs(new FileInfo(dialog.FileName));
                }
            }
        }

        public ICommand ClearScheduleCommand
        {
            get { return new BindingCommand((x) => ClearSchedule()); }
        }
        private void ClearSchedule()
        {
            using (RessourceScheduleManager manager = new RessourceScheduleManager())
            {
                manager.ClearScheduleForPeriod(ScheduleFromDate, ScheduleUntilDate);
            }
        }

    }
}
