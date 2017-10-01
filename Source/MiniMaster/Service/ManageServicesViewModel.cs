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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
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
