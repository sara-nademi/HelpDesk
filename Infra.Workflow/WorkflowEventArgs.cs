using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infra.Common;

namespace Infra.WorkflowEngine
{
    public class CreateWorkflowEventArgs : EventArgs
    {
        public WorkflowInstance WfInstance { get; set; }
        public TaskInstance CurrentTask { get; set; }
        public string UserName { get; set; }
        public bool Cancel { get; set; }
        public string CancelMessage { get; set; }

        public CreateWorkflowEventArgs()
        { 
        }

        public CreateWorkflowEventArgs(WorkflowInstance wfInstance, TaskInstance currentTask, string userName)
        {
            this.WfInstance = WfInstance;
            this.CurrentTask = currentTask;
            this.UserName = userName;
        }

    }


    public class FinishWorkflowEventArgs : CreateWorkflowEventArgs
    {
        public FinishWorkflowEventArgs()
        { 
        }

        public FinishWorkflowEventArgs(WorkflowInstance wfInstance, TaskInstance currentTask, string userName)
            : base(wfInstance, currentTask, userName)
        {
            
        }
    }


}
