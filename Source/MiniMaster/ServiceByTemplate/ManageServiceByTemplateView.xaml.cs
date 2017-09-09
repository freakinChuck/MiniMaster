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

namespace MiniMaster.ServiceByTemplate
{
    /// <summary>
    /// Interaction logic for ManageServiceByTemplateView.xaml
    /// </summary>
    public partial class ManageServiceByTemplateView : Page
    {
        public ManageServiceByTemplateView()
        {
            InitializeComponent();
            this.DataContext = new ManageServiceByTemplateViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as ServiceTemplate.ServiceTemplateGroupViewModel;
                ((ManageServiceByTemplateViewModel)this.DataContext).SelectedTemplate = newSelectedItem;
            }
            else
            {
                ((ManageServiceByTemplateViewModel)this.DataContext).SelectedTemplate = null;
            }
        }
    }
}
