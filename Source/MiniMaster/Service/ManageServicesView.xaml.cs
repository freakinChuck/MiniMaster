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

namespace MiniMaster.Service
{
    /// <summary>
    /// Interaction logic for ManageServiceView.xaml
    /// </summary>
    public partial class ManageServiceView : Page
    {
        public ManageServiceView()
        {
            InitializeComponent();
            this.DataContext = new ManageServiceViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as ServiceViewModel;
                ((ManageServiceViewModel)this.DataContext).SelectedService = newSelectedItem;
            }
            else
            {
                ((ManageServiceViewModel)this.DataContext).SelectedService = null;
            }
        }
    }
}
