using MiniMaster.Storage.Model;
using System.ComponentModel;
using System;
using MiniMaster.Storage;

namespace MiniMaster.Job
{
    public class TemplateJobViewModel : INotifyPropertyChanged
    {
        public TemplateJobViewModel(JobModel storageJob)
        {
            this.storageJob = storageJob;
            this.PropertyChanged += JobViewModel_PropertyChanged;
        }

        private void JobViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Workspace.RegisterDataChanged();
        }

        private readonly JobModel storageJob;

        public string Id => storageJob.Id;

        public string Text
        {
            get { return storageJob.Text; }
            set
            {
                storageJob.Text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        public string Comment
        {
            get { return storageJob.Comment; }
            set
            {
                storageJob.Comment = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Comment"));
            }
        }

        public int Order
        {
            get { return storageJob.Order; }
            set
            {
                storageJob.Order = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Order"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RemoveJobFromModel()
        {
            Workspace.CurrentData.Jobs.Remove(storageJob);
            Workspace.RegisterDataChanged();
        }
    }
}