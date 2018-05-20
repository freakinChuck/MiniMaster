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
            AllJobs = new BindingList<TemplateJobViewModel>(Workspace.CurrentData.Jobs.Select(x => new TemplateJobViewModel(x)).OrderBy(x => x.Order).ToList());
            SelectedJob = null;
        }

        public BindingList<TemplateJobViewModel> AllJobs { get; set; }
        private TemplateJobViewModel selectedJob;

        public TemplateJobViewModel SelectedJob
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
            this.AllJobs.Add(new TemplateJobViewModel(JobModel.CreateNewJob()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllJobs.Count - 1;
        }

        public BindingCommand RemoveJobCommand
        {
            get { return new BindingCommand(x => RemoveJob()); }
        }
        private void RemoveJob()
        {
            var tmpJob = SelectedJob;
            this.AllJobs.Remove(tmpJob);
            tmpJob.RemoveJobFromModel();
        }

        public BindingCommand UpCommand
        {
            get {
                return new BindingCommand(x =>
                {
                    if (AllJobs.Any(j => j.Order < SelectedJob.Order))
                    {
                        var jobToSwitch = AllJobs.Where(j => j.Order < SelectedJob.Order).OrderByDescending(j => j.Order).FirstOrDefault();
                        var tmpOrder = SelectedJob.Order;
                        SelectedJob.Order = jobToSwitch.Order;
                        jobToSwitch.Order = tmpOrder;
                        AllJobs = new BindingList<TemplateJobViewModel>(AllJobs.OrderBy(j => j.Order).ToList());
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllJobs)));
                    }
                });
            }
        }
        public BindingCommand DownCommand
        {
            get
            {
                return new BindingCommand(x =>
                {
                    if (AllJobs.Any(j => j.Order > SelectedJob.Order))
                    {
                        var jobToSwitch = AllJobs.Where(j => j.Order > SelectedJob.Order).OrderBy(j => j.Order).FirstOrDefault();
                        var tmpOrder = SelectedJob.Order;
                        SelectedJob.Order = jobToSwitch.Order;
                        jobToSwitch.Order = tmpOrder;
                        AllJobs = new BindingList<TemplateJobViewModel>(AllJobs.OrderBy(j => j.Order).ToList());
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllJobs)));
                    }
                });
            }
        }

        public bool IsJobSelected => SelectedJob != null;
    }
}
