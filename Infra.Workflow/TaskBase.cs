using System;

namespace Infra.WorkflowEngine
{
    public abstract class TaskBase
    {
        public abstract void DoAction(string actionCode, FinishTaskEventArgs e);
    }
}
