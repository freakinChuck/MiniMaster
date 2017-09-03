using MiniMaster._Helper;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Job
{
    public class ManageJobViewModel : INotifyPropertyChanged
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

        public ManageJobViewModel()
        {
            AllJobs = new BindingList<JobViewModel>(Workspace.CurrentData.Jobs.Select(x => new JobViewModel(x)).OrderBy(x => x.Text).ToList());
            SelectedJob = null;
        }

        public BindingList<JobViewModel> AllJobs { get; set; }
        private JobViewModel selectedJob;

        public JobViewModel SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedJob)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsJobSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewJobCommand
        {
            get { return new BindingCommand(x => AddNewJob()); }
        }
        private void AddNewJob()
        {
            this.AllJobs.Add(new JobViewModel(JobModel.CreateNewJob()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllJobs.Count - 1;
        }

        public BindingCommand RemoveJobCommand
        {
            get { return new BindingCommand(x => RemoveJob()); }
        }
        private void RemoveJob()
        {
            var selectedJob = SelectedJob;
            this.AllJobs.Remove(selectedJob);
            //SelectedIndex = 0;
            selectedJob.RemoveJobFromModel();
        }

        public bool IsJobSelected => SelectedJob != null;
    }
}
