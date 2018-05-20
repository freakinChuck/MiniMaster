using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;
using MiniMaster.Storage.Model.ServiceTemplate;

namespace MiniMaster.ServiceTemplate
{
    public class ServiceTemplateGroupViewModel : INotifyPropertyChanged
    {
        public ServiceTemplateGroupViewModel(ServiceTemplateGroupModel storageServiceTemplateGroup)
        {
            this.storageServiceTemplateGroup = storageServiceTemplateGroup;
            this.PropertyChanged += ServiceTemplateGroupViewModel_PropertyChanged;
        }

        private void ServiceTemplateGroupViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private readonly ServiceTemplateGroupModel storageServiceTemplateGroup;

        public string Id => storageServiceTemplateGroup.Id;

        public string Text
        {
            get { return storageServiceTemplateGroup.Text; }
            set
            {
                storageServiceTemplateGroup.Text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public bool Default
        {
            get { return storageServiceTemplateGroup.Default; }
            set
            {
                storageServiceTemplateGroup.Default = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Default"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveServiceTemplateGroupFromModel()
        {
            Workspace.CurrentData.ServiceTemplateGroups.Remove(storageServiceTemplateGroup);
            Workspace.RegisterDataChanged();
        }
    }
}