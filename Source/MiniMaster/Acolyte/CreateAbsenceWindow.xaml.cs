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
    /// Interaction logic for CreateAbsenceWindow.xaml
    /// </summary>
    public partial class CreateAbsenceWindow : Window
    {
        public CreateAbsenceWindow(string acolyteId)
        {
            InitializeComponent();
            var vm = (CreateAbsenceViewModel)this.DataContext;
            vm.AcolyteId = acolyteId;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(this.Close);
        }
    }
}
