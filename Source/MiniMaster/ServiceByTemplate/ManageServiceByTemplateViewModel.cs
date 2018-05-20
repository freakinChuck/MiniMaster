using MiniMaster._Helper;
using MiniMaster.ServiceTemplate;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiniMaster.ServiceByTemplate
{
    public class ManageServiceByTemplateViewModel : INotifyPropertyChanged
    {
        public DateTime? DateCreationStart { get; set; }
        public DateTime? DateCreationEnd { get; set; }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public ManageServiceByTemplateViewModel()
        {
            AllTemplates = new BindingList<ServiceTemplateGroupViewModel>(Workspace.CurrentData.ServiceTemplateGroups.OrderBy(x => x.Text).Select(x => new ServiceTemplateGroupViewModel(x)).ToList());
            SelectedTemplate = null;
        }

        public BindingList<ServiceTemplateGroupViewModel> AllTemplates { get; set; }
        private ServiceTemplateGroupViewModel selectedTemplate;

        public ServiceTemplateGroupViewModel SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                selectedTemplate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTemplate)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTemplateSelected)));
            }
        }

        public BindingCommand CreateServicesCommand
        {
            get { return new BindingCommand(x => CreateServices()); }
        }
        private void CreateServices()
        {
            if (SelectedTemplate == null || !DateCreationStart.HasValue || !DateCreationEnd.HasValue)
                return;

            if (DateCreationStart.Value > DateCreationEnd.Value)
            {
                MessageBox.Show("Das Enddatum darf nicht vor dem Startdatum liegen.", "Fehleingabe", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var allExistingServices = Workspace.CurrentData.Services.Where(x => x.DateAndTime.Date >= this.DateCreationStart.Value && x.DateAndTime.Date <= this.DateCreationEnd.Value).ToList();

            if (allExistingServices.Any())
            {
                if (MessageBox.Show("Es sind bereits Gottesdienste in diesem Zeitraum vorhanden. Diese werden durch diese Aktion ersetzt. Möchten Sie fortfahren?", "Daten bereits vorhanden", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                allExistingServices.ForEach(x => {
                    Workspace.CurrentData.Services.Remove(x);
                    Workspace.CurrentData.ServiceJobs.Where(j => x.Id == j.ServiceId).ToList().ForEach(j => Workspace.CurrentData.ServiceJobs.Remove(j));
                });
                Workspace.RegisterDataChanged();
            }

            var templateServices = Workspace.CurrentData.ServiceTemplates.Where(x => x.GroupId == this.SelectedTemplate.Id).ToList();

            for (DateTime date = DateCreationStart.Value; date <= DateCreationEnd.Value; date = date.AddDays(1))
            {
                var servicesToCreate = templateServices.Where(x => x.Day == date.DayOfWeek).ToList();
                foreach (var serviceToCreate in servicesToCreate)
                {
                    var newService = ServiceModel.CreateNewService();
                    newService.DateAndTime = date + serviceToCreate.Time;
                    Workspace.RegisterDataChanged();

                    foreach (var job in serviceToCreate.Jobs)
                    {
                        ServiceJobModel.CreateNewServiceJob(newService.Id, job);
                    }
                }
            }

            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsTemplateSelected => SelectedTemplate != null;
    }
}
