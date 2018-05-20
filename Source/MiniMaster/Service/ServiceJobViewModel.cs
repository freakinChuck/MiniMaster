using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniMaster.Storage.Model;
using MiniMaster.Storage.Model.ServiceTemplate;
using System.ComponentModel;
using MiniMaster._Helper;
using MiniMaster.Storage;

namespace MiniMaster.Service
{
    public class ServiceJobViewModel : INotifyPropertyChanged
    {
        private readonly ServiceModel serviceParent;
        public ServiceJobViewModel(ServiceModel service, JobModel job)
        {
            this.JobId = job.Id;
            this.JobName = job.Text;
            this.serviceParent = service;
        }

        public string JobId { get; set; }
        public string JobName { get; set; }
        public int NumberOfJobs
        {
            get
            {
                return Workspace.CurrentData.ServiceJobs.Count(x => x.ServiceId == this.serviceParent.Id && x.JobId == this.JobId);
            }
        }

        public string NumberOfAcolytesString
        {
            get { return string.Format("{0} Ministrant(en)", NumberOfJobs); }
        }

        public BindingCommand AddAcolyteCommand
        {
            get
            {
                return new BindingCommand(
                    x =>
                    {
                        ServiceJobModel.CreateNewServiceJob(this.serviceParent.Id, this.JobId);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfJobs"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfAcolytesString"));
                    }
                );
            }
        }
        public BindingCommand SubtractAcolyteCommand
        {
            get
            {
                return new BindingCommand(
                    x =>
                    {
                        if (NumberOfJobs > 0)
                        {
                            var jobToDelete = Workspace.CurrentData.ServiceJobs.Where(j => j.ServiceId == this.serviceParent.Id && j.JobId == this.JobId).OrderByDescending(j => string.IsNullOrEmpty(j.AcolyteId)).FirstOrDefault();
                            if (jobToDelete != null)
                            {
                                Workspace.CurrentData.ServiceJobs.Remove(jobToDelete);
                            }
                        }
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfJobs"));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumberOfAcolytesString"));
                    }
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
