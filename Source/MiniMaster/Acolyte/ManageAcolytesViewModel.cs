using MiniMaster._Helper;
using MiniMaster.Storage;
using MiniMaster.Storage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniMaster.Acolyte
{
    public class ManageAcolyteViewModel : INotifyPropertyChanged
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

        public ManageAcolyteViewModel()
        {
            AllAcolytes = new BindingList<AcolyteViewModel>(Workspace.CurrentData.Acolytes.Select(x => new AcolyteViewModel(x)).OrderBy(x => x.DisplayName).ToList());
            SelectedAcolyte = null;
        }

        public BindingList<AcolyteViewModel> AllAcolytes { get; set; }
        private AcolyteViewModel selectedAcolyte;

        public AcolyteViewModel SelectedAcolyte
        {
            get { return selectedAcolyte; }
            set
            {
                selectedAcolyte = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAcolyte)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAcolyteSelected)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BindingCommand NewAcolyteCommand
        {
            get { return new BindingCommand(x => AddNewAcolyte()); }
        }
        private void AddNewAcolyte()
        {
            this.AllAcolytes.Add(new AcolyteViewModel(AcolyteModel.CreateNewAcolyte()));
            Workspace.RegisterDataChanged();
            SelectedIndex = this.AllAcolytes.Count - 1;
        }

        public BindingCommand RemoveAcolyteCommand
        {
            get { return new BindingCommand(x => RemoveAcolyte()); }
        }
        private void RemoveAcolyte()
        {
            var selectedAcolyte = SelectedAcolyte;
            this.AllAcolytes.Remove(selectedAcolyte);
            //SelectedIndex = 0;
            selectedAcolyte.RemoveAcolyteFromModel();
        }

        public bool IsAcolyteSelected => SelectedAcolyte != null;
    }
}
