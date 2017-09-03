﻿using System;

namespace MiniMaster.Storage
{
    public class WorkspaceCancelEventArgs : WorkspaceEventArgs
    {
        public WorkspaceCancelEventArgs(Workspace workspace)
            :base(workspace)
        {

        }

        public bool Cancel { get; set; }
        public bool DoSave { get; set; }
    }
}