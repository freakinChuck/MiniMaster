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

namespace MiniMaster.Acolyte
{
    /// <summary>
    /// Interaction logic for ManageAcolyteView.xaml
    /// </summary>
    public partial class ManageAcolyteView : Page
    {
        public ManageAcolyteView()
        {
            InitializeComponent();
            this.DataContext = new ManageAcolyteViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as AcolyteViewModel;
                ((ManageAcolyteViewModel)this.DataContext).SelectedAcolyte = newSelectedItem;
            }
            else
            {
                ((ManageAcolyteViewModel)this.DataContext).SelectedAcolyte = null;
            }
        }
    }
}
