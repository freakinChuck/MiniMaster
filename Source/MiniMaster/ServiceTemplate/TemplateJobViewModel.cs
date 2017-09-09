using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniMaster.Storage.Model;
using MiniMaster.Storage.Model.ServiceTemplate;
using System.ComponentModel;
using MiniMaster._Helper;

namespace MiniMaster.ServiceTemplate
{
    public class TemplateJobViewModel : INotifyPropertyChanged
    {
        private ServiceTemplateModel serviceParent;
        public TemplateJobViewModel(ServiceTemplateModel service, JobModel job)
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
                return serviceParent.Jobs.Count(x => x == this.JobId);
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
                        serviceParent.Jobs.Add(this.JobId);
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
                            serviceParent.Jobs.Remove(this.JobId);
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
