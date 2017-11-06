using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniMaster.Acolyte
{
    /// <summary>
    /// Interaction logic for CreateContinousAbsenceWindow.xaml
    /// </summary>
    public partial class CreateContinousAbsenceWindow : Window
    {
        public CreateContinousAbsenceWindow(string acolyteId)
        {
            InitializeComponent();
            var vm = (CreateContinousAbsenceViewModel)this.DataContext;
            vm.AcolyteId = acolyteId;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        
            dayOfWeekComboBox.Items.Add(new { Name = "Sonntag", Value = DayOfWeek.Sunday });
            dayOfWeekComboBox.Items.Add(new { Name = "Montag", Value = DayOfWeek.Monday });
            dayOfWeekComboBox.Items.Add(new { Name = "Dienstag", Value = DayOfWeek.Tuesday});
            dayOfWeekComboBox.Items.Add(new { Name = "Mittwoch", Value = DayOfWeek.Wednesday });
            dayOfWeekComboBox.Items.Add(new { Name = "Donnerstag", Value = DayOfWeek.Thursday });
            dayOfWeekComboBox.Items.Add(new { Name = "Freitag", Value = DayOfWeek.Friday });
            dayOfWeekComboBox.Items.Add(new { Name = "Samstag", Value = DayOfWeek.Saturday });
        }
    }
}
