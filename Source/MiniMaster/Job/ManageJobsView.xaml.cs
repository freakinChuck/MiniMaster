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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniMaster.Job
{
    /// <summary>
    /// Interaction logic for ManageJobView.xaml
    /// </summary>
    public partial class ManageJobView : Page
    {
        public ManageJobView()
        {
            InitializeComponent();
            this.DataContext = new ManageJobViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as TemplateJobViewModel;
                ((ManageJobViewModel)this.DataContext).SelectedJob = newSelectedItem;
            }
            else
            {
                ((ManageJobViewModel)this.DataContext).SelectedJob = null;
            }
        }
    }
}
