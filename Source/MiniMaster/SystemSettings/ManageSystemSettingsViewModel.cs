using MiniMaster._Helper;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.SystemSettings
{
    public class ManageSystemSettingsViewModel : INotifyPropertyChanged
    {

        public ManageSystemSettingsViewModel()
        {
            Settings = new SystemSettingsViewModel(Workspace.CurrentData.Settings);
        }

        public SystemSettingsViewModel Settings { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
