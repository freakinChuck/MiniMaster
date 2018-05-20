using MiniMaster._Helper;
using MiniMaster.Job;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using MiniMaster.Storage.Model.ServiceTemplate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.ServiceTemplate
{
    public class ManageServiceTemplateGroupViewModel : INotifyPropertyChanged
    {
        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        private int selectedSubIndex;

        public int SelectedSubIndex
        {
            get { return selectedSubIndex; }
            set
            {
                selectedSubIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSubIndex)));
            }
        }
        
        public ManageServiceTemplateGroupViewModel()
        {
            AllServiceTemplateGroups = new BindingList<ServiceTemplateGroupViewModel>(Workspace.CurrentData.ServiceTemplateGroups.Select(x => new ServiceTemplateGroupViewModel(x)).OrderBy(x => x.Text).ToList());
            SelectedServiceTemplateGroup = null;
        }

        public BindingList<TemplateJobViewModel> AllJobs
        {
            get; set;
        }

        public BindingList<ServiceTemplateGroupViewModel> AllServiceTemplateGroups { get; set; }
        private ServiceTemplateGroupViewModel selectedServiceTemplateGroup;

        public ServiceTemplateGroupViewModel SelectedServiceTemplateGroup
        {
            get { return selectedServiceTemplateGroup; }
            set
            {
                selectedServiceTemplateGroup = value;
                if (value != null)
                {
                    this.AllServiceTemplates = new BindingList<ServiceTemplateViewModel>(Workspace.CurrentData.ServiceTemplates.Where(x => x.GroupId == value.Id).Select(x => new ServiceTemplateViewModel(x)).OrderBy(x => x.Day).ThenBy(x => x.Time).ToList());
                }
                else
                {
                    this.AllServiceTemplates = new BindingList<ServiceTemplateViewModel>();
                }
                SelectedSubIndex = 0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedServiceTemplateGroup)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsServiceTemplateGroupSelected)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllServiceTemplates)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSubIndex)));
            }
        }

        public BindingList<ServiceTemplateViewModel> AllServiceTemplates { get; set; }

        private ServiceTemplateViewModel selectedServiceTemplate;
        public ServiceTemplateViewModel SelectedServiceTemplate
        {
            get { return selectedServiceTemplate; }
            set
            {
                selectedServiceTemplate = value;
                LoadJobs();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedServiceTemplate)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsServiceTemplateGroupSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewServiceTemplateGroupCommand
        {
            get { return new BindingCommand(x => AddNewServiceTemplateGroup()); }
        }
        private void AddNewServiceTemplateGroup()
        {
            this.AllServiceTemplateGroups.Add(new ServiceTemplateGroupViewModel(ServiceTemplateGroupModel.CreateNewServiceTemplateGroup()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllServiceTemplateGroups.Count - 1;
        }

        public BindingCommand RemoveServiceTemplateGroupCommand
        {
            get { return new BindingCommand(x => RemoveServiceTemplateGroup()); }
        }
        private void RemoveServiceTemplateGroup()
        {
            var tmpServiceTemplateGroup = SelectedServiceTemplateGroup;
            this.AllServiceTemplateGroups.Remove(tmpServiceTemplateGroup);
            tmpServiceTemplateGroup.RemoveServiceTemplateGroupFromModel();
        }


        public BindingCommand NewServiceTemplateCommand
        {
            get { return new BindingCommand(x => AddNewServiceTemplate()); }
        }
        private void AddNewServiceTemplate()
        {
            this.AllServiceTemplates.Add(new ServiceTemplateViewModel(ServiceTemplateModel.CreateNewServiceTemplate(SelectedServiceTemplateGroup.Id)));
            Workspace.RegisterDataChanged();
            SelectedSubIndex = this.AllServiceTemplates.Count - 1;
        }

        public BindingCommand RemoveServiceTemplateCommand
        {
            get { return new BindingCommand(x => RemoveServiceTemplate()); }
        }
        private void RemoveServiceTemplate()
        {
            var tmpServiceTemplate = SelectedServiceTemplate;
            this.AllServiceTemplates.Remove(tmpServiceTemplate);
            tmpServiceTemplate.RemoveServiceTemplateFromModel();
        }

        public BindingCommand AddAcolyteCommand
        {
            get
            {
                return new BindingCommand((jobId) =>
                {
                    this.SelectedServiceTemplate.Jobs.Add((string)jobId);
                    LoadJobs();
                });
            }
        }

        public BindingCommand SubtractAcolyteCommand
        {
            get
            {
                return new BindingCommand((jobId) =>
                {
                    if (this.SelectedServiceTemplate.Jobs.Contains((string)jobId))
                    {
                        this.SelectedServiceTemplate.Jobs.Remove((string)jobId);
                    }
                    LoadJobs();
                });
            }
        }

        public bool IsServiceTemplateGroupSelected => SelectedServiceTemplateGroup != null;
        public bool IsServiceTemplateSelected => SelectedServiceTemplate != null;

        private void LoadJobs()
        {
            var template = this.SelectedServiceTemplate;
            if (template == null)
            {
                this.AllJobs = new BindingList<TemplateJobViewModel>();
            }
            else
            {
                this.AllJobs = new BindingList<TemplateJobViewModel>(Workspace.CurrentData.Jobs.OrderBy(x => x.Order).Select(x => new TemplateJobViewModel(template.storageServiceTemplate, x)).ToList());
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllJobs)));
        }

    }
}
