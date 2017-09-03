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

namespace MiniMaster.ServiceTemplate
{
    /// <summary>
    /// Interaction logic for ManageServiceTemplateGroupView.xaml
    /// </summary>
    public partial class ManageServiceTemplateGroupView : Page
    {
        public ManageServiceTemplateGroupView()
        {
            InitializeComponent();
            this.DataContext = new ManageServiceTemplateGroupViewModel();
            dayComboBox.Items.Add(new { Text = "Sonntag", Value = DayOfWeek.Sunday });
            dayComboBox.Items.Add(new { Text = "Montag", Value = DayOfWeek.Monday });
            dayComboBox.Items.Add(new { Text = "Dienstag", Value = DayOfWeek.Tuesday });
            dayComboBox.Items.Add(new { Text = "Mittwoch", Value = DayOfWeek.Wednesday });
            dayComboBox.Items.Add(new { Text = "Donnerstag", Value = DayOfWeek.Thursday });
            dayComboBox.Items.Add(new { Text = "Freitag", Value = DayOfWeek.Friday });
            dayComboBox.Items.Add(new { Text = "Samstag", Value = DayOfWeek.Saturday });
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as ServiceTemplateGroupViewModel;
                ((ManageServiceTemplateGroupViewModel)this.DataContext).SelectedServiceTemplateGroup = newSelectedItem;
            }
            else
            {
                ((ManageServiceTemplateGroupViewModel)this.DataContext).SelectedServiceTemplateGroup = null;
            }
        }

        private void SubListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var newSelectedItem = e.AddedItems[0] as ServiceTemplateViewModel;
                ((ManageServiceTemplateGroupViewModel)this.DataContext).SelectedServiceTemplate = newSelectedItem;
            }
            else
            {
                ((ManageServiceTemplateGroupViewModel)this.DataContext).SelectedServiceTemplate = null;
            }
        }
    }
}
