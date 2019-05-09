using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infra.Common;


namespace Infra.WorkflowEngine
{


    public class FinishTaskEventArgs : EventArgs 
    {
        public bool IsBefore { get; set; }

        public TaskAction TaskAction;
        public TaskInstance TaskInstance;
        public TaskInstance ToTask;
        public string Description;
        public string UserName;
        public string NewPerformer;

        public bool Cancel { get; set; }
        public string CancelMessage { get; set; }

    }

    public class AssignTaskEventArgs : EventArgs
    {
        public bool IsBefore { get; set; }
        public TaskInstance TaskInstance;
        public string UserName;
        public string NewPerformer;
        public string Description;

        public bool Cancel { get; set; }
        public string CancelMessage { get; set; }
    }


}
