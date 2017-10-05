using MiniMaster._Helper;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Service
{
    public class ManageServiceViewModel : INotifyPropertyChanged
    {
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set {
                selectedIndex = value;
                LoadJobs();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public BindingList<ServiceJobViewModel> AllJobs
        {
            get; set;
        }

        public List<object> JobsWithAssignments
        {
            get
            {
                var data = Workspace.CurrentData.ServiceJobs.Where(x => x.ServiceId == SelectedService.Id)
                    .Select(x =>
                    new
                    {
                        JobId = x.JobId,
                        JobName = Workspace.CurrentData.Jobs.FirstOrDefault(j => j.Id == x.JobId)?.Text ?? "unbekannte Aufgabe",
                        JobOrder = Workspace.CurrentData.Jobs.FirstOrDefault(j => j.Id == x.JobId)?.Order ?? int.MaxValue,
                        ServiceId = x.ServiceId,
                        AcolyteId = x.AcolyteId,
                        AcolyteName = Workspace.CurrentData.Acolytes.FirstOrDefault(a => a.Id == x.AcolyteId)?.Name,
                        AcolyteFirstname = Workspace.CurrentData.Acolytes.FirstOrDefault(a => a.Id == x.AcolyteId)?.Firstname,
                    })
                    .OrderBy(x => x.JobOrder)
                    .ThenBy(x => string.IsNullOrEmpty(x.AcolyteId));
                return
                    (data.Select(x => 
                            (object)new
                            {
                                Text = string.Format(
                                                "{0}: {1}", 
                                                x.JobName, 
                                                string.IsNullOrEmpty(x.AcolyteId) ? 
                                                    "keine Zuteilung" : 
                                                    (string.IsNullOrEmpty(x.AcolyteFirstname) ? 
                                                        "unbekannter Ministrant" : 
                                                        string.Format("{0} {1}", x.AcolyteFirstname, x.AcolyteName)
                                                    )
                                        )
                            }).ToList()
                    );
            }
        }

        private void LoadJobs()
        {
            var template = this.SelectedService;
            if (template == null)
            {
                this.AllJobs = new BindingList<ServiceJobViewModel>();
            }
            else
            {
                this.AllJobs = new BindingList<ServiceJobViewModel>(Workspace.CurrentData.Jobs.OrderBy(x => x.Order).Select(x => new ServiceJobViewModel(template.storageService, x)).ToList());
                foreach (var job in AllJobs)
                {
                    job.PropertyChanged += Job_PropertyChanged;
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllJobs)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JobsWithAssignments)));
        }

        private void Job_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(JobsWithAssignments)));
        }

        public ManageServiceViewModel()
        {
            AllServices = new BindingList<ServiceViewModel>(Workspace.CurrentData.Services.Select(x => new ServiceViewModel(x)).OrderBy(x => x.DateAndTime).ToList());
            SelectedService = null;
        }

        public BindingList<ServiceViewModel> AllServices { get; set; }
        private ServiceViewModel selectedService;

        public ServiceViewModel SelectedService
        {
            get { return selectedService; }
            set
            {
                selectedService = value;
                LoadJobs();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedService)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsServiceSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewServiceCommand
        {
            get { return new BindingCommand(x => AddNewService()); }
        }
        private void AddNewService()
        {
            this.AllServices.Add(new ServiceViewModel(ServiceModel.CreateNewService()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllServices.Count - 1;
        }

        public BindingCommand RemoveServiceCommand
        {
            get { return new BindingCommand(x => RemoveService()); }
        }
        private void RemoveService()
        {
            var selectedService = SelectedService;
            this.AllServices.Remove(selectedService);
            //SelectedIndex = 0;
            selectedService.RemoveServiceFromModel();
        }

        public bool IsServiceSelected => SelectedService != null;
    }
}
