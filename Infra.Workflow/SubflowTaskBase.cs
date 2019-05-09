using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infra.WorkflowEngine;
using Infra.Common;

namespace Infra.WorkflowEngine
{
    public abstract class SubflowTaskBase : TaskBase
    {
        public override void DoAction(string actionCode, FinishTaskEventArgs e)
        {
        }

        public void CreateSubflowInstance(FinishTaskEventArgs e)
        {
            if (e.IsBefore)
            {
                WorkflowService wfs = new WorkflowService();

                WorkflowInstance wfInstance = new WorkflowInstance();
                wfInstance.WorkflowInstanceID = Guid.NewGuid();
                wfInstance.EntityID = e.TaskInstance.EntityID;
                wfInstance.EntityUrl = e.TaskInstance.EntityName;
                wfInstance.ParentTaskInstanceID = e.TaskInstance.TaskInstanceID;
                wfInstance.ExtraInfo = e.TaskInstance.WorkflowInstance.ExtraInfo;
                OnBeforeCreateSubflowInstance(wfInstance, e);

                WorkflowBase wf = wfs.GetWorkflowBaseByWorkflowCode(e.TaskInstance.Task.SubflowWorkflowCode);
                wf.CreateWorkflowInstance(wfInstance, e.UserName, null, null,0);

                e.ToTask.SubflowWorkflowInstanceID = wfInstance.WorkflowInstanceID;
            }
        }


        public virtual void OnBeforeCreateSubflowInstance(WorkflowInstance wfInstance, FinishTaskEventArgs prevFinishEventArgs)
        {
            
        }

        public abstract string GetResultActionCode(FinishWorkflowEventArgs e);

    }
}
