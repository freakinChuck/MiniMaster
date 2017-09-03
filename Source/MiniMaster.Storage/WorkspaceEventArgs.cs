using System;

namespace MiniMaster.Storage
{
    public class WorkspaceEventArgs : EventArgs
    {
        public Workspace Workspace { get; private set; }
        public WorkspaceEventArgs(Workspace workspace)
            :base()
        {
            this.Workspace = workspace;
        }
    }
}