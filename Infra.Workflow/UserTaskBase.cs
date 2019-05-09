using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infra.WorkflowEngine;

namespace Infra.WorkflowEngine
{
    public class UserTaskBase : TaskBase
    {
        /// <summary>
        /// فاكنكشن كاربر را براي قبل و بعد پيدا ميكند و اجرا ميكند
        /// </summary>
        /// <param name="actionCode">كد اكشن</param>
        /// <param name="e"></param>
        public override sealed void DoAction(string actionCode, FinishTaskEventArgs e)
        {
            if (e.TaskAction.HasProgramming)
            {
                if (string.IsNullOrEmpty(actionCode))
                    throw new Exception("actionCode should not be null or empty");

                string methodName = actionCode + "Before";
                if (e.IsBefore == false)
                    methodName = actionCode + "After";

                var m = this.GetType().GetMethod(methodName);
                if (m == null)
                    throw new Exception("Method " + methodName + " is not implemented in the class " + this.GetType().FullName);

                m.Invoke(this, new object[] {e} );
            }

        }

    }
}
