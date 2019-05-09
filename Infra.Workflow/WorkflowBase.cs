using System;
using System.Collections.Generic;
using System.Linq;
using Infra.Common;

namespace Infra.WorkflowEngine
{
    public enum FinishTaskUIEventsEnum
    {
        FT_OnInit = 1,
        FT_OnActionComboSelectedChange = 2,
        FT_OnBeforeActionButtonClick = 3,
        FT_OnAfterActionButtonClick = 4,
        FT_OnLoad = 5
    }

    public enum TaskTypeEnum
    {
        Start = 0,
        User = 1,
        End = 2,
        Subflow = 3,
        AutoActivity = 4,
        LongDelay = 5
    }

    public enum TaskInstanceStatusEnum
    {
        Waiting = 0,
        Complete = 1,
        Cancel = 2,
        Error = 3,
        Refer = 4
    }

    public enum NotificationStatusEnum
    {
        Waiting = 0,
        Complete = 1,
        NoNeed = 2
    }

    public enum WorkflowInstanceStatusEnum
    {
        Waiting = 0,
        Complete = 1,
        Cancel = 2,
        Error = 3
    }

    public class WorkflowBase : IDisposable
    {
        public int WorkflowID { get; set; }

        public delegate void FinishTaskActionDelegate(object sender, FinishTaskEventArgs e);
        public event FinishTaskActionDelegate BeforeFinishTask;
        public event FinishTaskActionDelegate AfterFinishTask;

        public delegate void AssignTaskActionDelegate(object sender, AssignTaskEventArgs e);
        public event AssignTaskActionDelegate BeforeAssignTask;
        public event AssignTaskActionDelegate AfterAssignTask;

        public delegate void CreateWorkflowDelegate(object sender, CreateWorkflowEventArgs e);
        public event CreateWorkflowDelegate BeforeCreateWorkflow;
        public event CreateWorkflowDelegate AfterCreateWorkflow;

        public delegate void FinishWorkflowDelegate(object sender, FinishWorkflowEventArgs e);
        public event FinishWorkflowDelegate BeforeFinishWorkflow;
        public event FinishWorkflowDelegate AfterFinishWorkflow;


        private WorkflowService _wfs;
        public WorkflowService wfs
        {
            get
            {
                if (_wfs == null)
                    _wfs = new WorkflowService();
                return _wfs;
            }
        }

        bool isDisposing;

        ~WorkflowBase()
        {
            if (isDisposing == false)
                if (_wfs != null)
                    _wfs.Dispose();
        }

        public void Dispose()
        {
            isDisposing = true;
            if (_wfs != null)
                _wfs.Dispose();
        }

        #region Finish Task Function
        public void FinishTask(Guid taskInstanceID, int taskActionId, string userName, string newPerformerID, string comment)
        {
            TaskAction action = wfs.GetTaskActionByID(taskActionId);
            TaskInstance ts = wfs.GetTaskInstanceByID(taskInstanceID);
            FinishTask(ts, action, userName, newPerformerID, comment, null);
        }

        /// <summary>
        /// يك فعاليت را به اتمام ميرساند
        /// </summary>
        /// <param name="WorkflowHistoryID">شناسه نمونه فعاليت</param>
        /// <param name="ActionID">شناسه اكشني كه بايد روي فعاليت انجام گيردر</param>
        /// <param name="Description">توضيحات</param>
        /// <param name="UserName">نام كاربري</param>
        /// <param name="NewPerformer">اقدام كننده بعدي</param>
        /// <param name="ds">ديتاست</param>
        /// <param name="dueDate"></param>
        public TaskInstance FinishTask(TaskInstance taskInstance, TaskAction Action, string UserName, string NewPerformer, string Description, DateTime? dueDate)
        {
            try
            {
                if (taskInstance.UpdateDate.HasValue)
                    throw new WFUserException("اين كار قبلا به اتمام رسيده است");

                Task toTask = Action.Task1;

                TaskInstance ti = new TaskInstance();
                ti.TaskInstanceID = Guid.NewGuid();
                ti.WorkflowInstanceID = taskInstance.WorkflowInstanceID;
                ti.TaskID = toTask.TaskID;
                ti.TaskCode = toTask.TaskCode;
                ti.TaskTitle = toTask.TaskTitle;
                ti.PerformerID = NewPerformer;
                ti.EntityName = taskInstance.EntityName;
                ti.EntityID = taskInstance.EntityID;
                ti.EntityTitle = taskInstance.EntityTitle;
                ti.EntityUrl = taskInstance.EntityUrl;
                ti.EntityDateTime = taskInstance.EntityDateTime;
                ti.PreviousTaskInstanceID = taskInstance.TaskInstanceID;
                ti.InsertUser = UserName;
                ti.InsertDate = DateTime.Now;
                ti.TaskInstanceStatusID = (int)TaskInstanceStatusEnum.Waiting; // در حال انتظار
                ti.TaskDueDate = dueDate;
                ti.PriorityID = taskInstance.PriorityID;
                ti.NotificationStatusID = (int)NotificationStatusEnum.Waiting;
                ti.ExtraInt = taskInstance.ExtraInt;
                ti.ExtraInt2 = taskInstance.ExtraInt2;
                ti.ExtraDateTime = taskInstance.ExtraDateTime;
                ti.ExtraVarchar = taskInstance.ExtraVarchar;
                ti.ExtraDouble = taskInstance.ExtraDouble;

                // calling events
                FinishWorkflowEventArgs ewfi = null; // براي اتمام ورك فلو
                FinishTaskEventArgs taskEventArg = new FinishTaskEventArgs() { TaskInstance = taskInstance, TaskAction = Action, UserName = UserName, NewPerformer = NewPerformer, IsBefore = true, Description = Description, ToTask = ti };
                OnBeforeFinishTask(taskEventArg);

                // setting previous task changes
                TaskInstance prevTask = wfs.GetTaskInstanceByID(taskInstance.TaskInstanceID);
                prevTask.UpdateUser = UserName;
                prevTask.UpdateDate = DateTime.Now;
                prevTask.CompletedActionID = Action.TaskActionID;
                prevTask.CompletedActionCode = Action.TaskActionCode;
                prevTask.TaskInstanceStatusID = (int)TaskInstanceStatusEnum.Complete; // تمام شده
                prevTask.Comment = Description;

                // stoping the timesheet if started before
                wfs.StopTimeSheet(prevTask.TaskInstanceID, Convert.ToInt32(UserName));

                // Saving Parameters
                wfs.WorkflowInstanceParameterSave(taskInstance.WorkflowInstanceID, "LastPerformerID", ti.PerformerID);
                wfs.WorkflowInstanceParameterSave(taskInstance.WorkflowInstanceID, "LastComment", prevTask.Comment);
                string lastDueDateString = null;
                if (ti.TaskDueDate.HasValue)
                    lastDueDateString = ti.TaskDueDate.ToString();
                wfs.WorkflowInstanceParameterSave(taskInstance.WorkflowInstanceID, "LastDueDate", lastDueDateString);

                // check end tasks
                if (toTask.TaskTypeID == (int)TaskTypeEnum.End) // اگر فعاليت بعدي پاياني بود
                {
                    ti.UpdateDate = DateTime.Now;
                    ti.UpdateUser = ti.InsertUser;
                    ti.TaskInstanceStatusID = (int)TaskInstanceStatusEnum.Complete; // تمام شده

                    var wfi = wfs.GetWorkflowInstanceByID(ti.WorkflowInstanceID);
                    wfi.UpdateUser = prevTask.UpdateUser;
                    wfi.UpdateDate = prevTask.UpdateDate;
                    wfi.WorkflowInstanceStatusID = (int)WorkflowInstanceStatusEnum.Complete; // تكميل شده

                    ewfi = new FinishWorkflowEventArgs(wfi, prevTask, UserName);
                    OnBeforeFinishWorkflowInstance(ewfi);
                }
                wfs.ObjectContext.TaskInstance.AddObject(ti);


                taskEventArg.IsBefore = false;
                OnAfterFinishTask(taskEventArg);

                if (ewfi != null) // اگر ورك فلو هم اتمام شده بود
                    OnAfterFinishWorkflowInstance(ewfi);

                wfs.ObjectContext.SaveChanges();
                return ti;
                //TaskInstanceService service = (TaskInstanceService)EntityFactory.GetServiceByEntityName("TaskInstance", "");
                //service.FinishTask(WorkflowHistory.WorkflowHistoryID, Action.ActionID, Description, UserName, NewPerformer, ds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// قبل از اينكه تسك به اتمام برسد
        /// </summary>
        /// <param name="taskEventArg"></param>
        protected virtual void OnBeforeFinishTask(FinishTaskEventArgs taskEventArg)
        {
            if (BeforeFinishTask != null)
                BeforeFinishTask(this, taskEventArg);
            else
                DoAction(this, taskEventArg);

            if (taskEventArg.Cancel == true)
                throw new WFUserException(taskEventArg.CancelMessage);
        }

        /// <summary>
        /// بعد از اينكه تسك به اتمام رسيد
        /// </summary>
        /// <param name="taskEventArg"></param>
        protected virtual void OnAfterFinishTask(FinishTaskEventArgs taskEventArg)
        {
            if (taskEventArg.TaskAction.Task1.TaskTypeID == (int)TaskTypeEnum.Subflow)
            {
                SubflowTaskBase act = (SubflowTaskBase)GetTaskClassByTaskCode(taskEventArg.TaskAction.Task1.TaskCode);
                act.CreateSubflowInstance(taskEventArg);
            }

            if (AfterFinishTask != null)
                AfterFinishTask(this, taskEventArg);
            else
                DoAction(this, taskEventArg);
        }

        /// <summary>
        /// انجام كار به صورت پيش فرض
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoAction(object sender, FinishTaskEventArgs e)
        {
            string taskCode = e.TaskInstance.TaskCode;
            string actionCode = e.TaskAction.TaskActionCode;

            if (e.TaskAction.HasProgramming)
            {
                TaskBase act = GetTaskClassByTaskCode(taskCode);
                if (act == null)
                    throw new Exception("ActivityCode " + taskCode.ToString() + ".HasProgarmming = true, but class not found in the application.");
                act.DoAction(actionCode, e);
            }

        }
        #endregion

        #region AssignTask

        /// <summary>
        /// يك فعاليت را به شخص ديگري انتصاب مي دهد
        /// </summary>
        /// <param name="WorkflowHistory">شناسه نمونه فعاليت</param>
        /// <param name="Description">توضيحات</param>
        /// <param name="UserName">نام كاربري</param>
        /// <param name="NewPerformer">نام شخصي كه بايد فعاليت به آن انتصاب يابد</param>
        /// <param name="ds">ديتاست</param>
        public void AssignTask(Guid taskInstanceID, string UserName, string NewPerformer, string Description)
        {
            try
            {
                TaskInstance TaskInstance = wfs.GetTaskInstanceByID(taskInstanceID);

                if (TaskInstance.TaskInstanceStatusID != 0)
                    throw new WFUserException("وضعيت اين كار در حال انتظار نيست و بنابراين قابل انتقال نيست.");

                TaskAction ta = wfs.TaskGetAssignTaskAction(TaskInstance.TaskID);
                if (ta == null)
                    throw new WFUserException("اين كار قابل انتقال نيست.");

                TaskInstance prevTask = wfs.GetTaskInstanceByID(TaskInstance.TaskInstanceID);
                prevTask.UpdateUser = UserName;
                prevTask.UpdateDate = DateTime.Now;
                prevTask.CompletedActionID = ta.TaskActionID;
                prevTask.CompletedActionCode = "Assign";
                prevTask.TaskInstanceStatusID = (int)TaskInstanceStatusEnum.Complete; // تمام شده


                //TaskInstanceService service = (TaskInstanceService)EntityFactory.GetServiceByEntityName("TaskInstance", "");
                //service.AssignTask(WorkflowHistory.WorkflowHistoryID, Description, UserName, NewPerformer, ds);
                TaskInstance ti = new TaskInstance();
                ti.TaskInstanceID = Guid.NewGuid();
                ti.WorkflowInstanceID = TaskInstance.WorkflowInstanceID;
                ti.TaskID = TaskInstance.TaskID;
                ti.TaskCode = TaskInstance.TaskCode;
                ti.TaskTitle = TaskInstance.TaskTitle;
                ti.PerformerID = NewPerformer;
                ti.EntityName = TaskInstance.EntityName;
                ti.EntityID = TaskInstance.EntityID;
                ti.EntityTitle = TaskInstance.EntityTitle;
                ti.EntityUrl = TaskInstance.EntityUrl;
                ti.EntityDateTime = TaskInstance.EntityDateTime;
                ti.PreviousTaskInstanceID = TaskInstance.TaskInstanceID;
                ti.InsertUser = UserName;
                ti.InsertDate = DateTime.Now;
                ti.TaskInstanceStatusID = (int)TaskInstanceStatusEnum.Waiting; // در حال انتظار
                ti.TaskDueDate = TaskInstance.TaskDueDate;
                ti.PriorityID = TaskInstance.PriorityID;
                ti.Comment = Description;
                ti.NotificationStatusID = (int)NotificationStatusEnum.Waiting;
                ti.ExtraInt = TaskInstance.ExtraInt;
                ti.ExtraInt2 = TaskInstance.ExtraInt2;
                ti.ExtraDateTime = TaskInstance.ExtraDateTime;
                ti.ExtraVarchar = TaskInstance.ExtraVarchar;
                ti.ExtraDouble = ti.ExtraDouble;

                AssignTaskEventArgs taskEventArg = new AssignTaskEventArgs() { TaskInstance = TaskInstance, Description = Description, UserName = UserName, NewPerformer = NewPerformer, IsBefore = true };
                OnBeforeAssignTask(taskEventArg);

                wfs.ObjectContext.TaskInstance.AddObject(ti);

                taskEventArg.IsBefore = true;
                OnAfterAssignTask(taskEventArg);

                wfs.ObjectContext.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// قبل از اساين تسك
        /// </summary>
        /// <param name="taskEventArg"></param>
        protected virtual void OnBeforeAssignTask(AssignTaskEventArgs taskEventArg)
        {
            if (BeforeAssignTask != null)
                BeforeAssignTask(this, taskEventArg);

            if (taskEventArg.Cancel == true)
                throw new WFUserException(taskEventArg.CancelMessage);
        }

        /// <summary>
        /// بعد از اساين تسك
        /// </summary>
        /// <param name="taskEventArg"></param>
        private void OnAfterAssignTask(AssignTaskEventArgs taskEventArg)
        {
            if (AfterAssignTask != null)
                AfterAssignTask(this, taskEventArg);
        }


        #endregion

        #region Create Workflow
        /// <summary>
        /// يك فرآيند جديد را آغاز ميكند
        /// </summary>
        /// <param name="wfInstanceId">شناسه فرآيند</param>
        /// <param name="EntityID">شناسه موجوديت</param>
        /// <param name="userName">نام كاربر اجرا كننده</param>
        /// <param name="firstPerformer">اولين كسي كه بايد كار را انجام دهيد</param>
        public void CreateWorkflowInstance(Guid wfInstanceId, string EntityID, string userName, string firstPerformer, string extraInfo, string entityUrl, int priorityNo)
        {
            WorkflowInstance wfInstance = new WorkflowInstance();
            wfInstance.WorkflowInstanceID = wfInstanceId;
            wfInstance.EntityID = EntityID;
            wfInstance.EntityUrl = entityUrl;
            CreateWorkflowInstance(wfInstance, userName, firstPerformer, extraInfo, priorityNo);
        }

        /// <summary>
        /// يك فرآيند جديد را آغاز ميكند
        /// </summary>
        /// <param name="InstanceID">شناسه فرآيند جديد</param>
        /// <param name="WorkflowCode">كدفرآيند</param>
        /// <param name="EntityID">شناسه موجوديت</param>
        /// <param name="ExtraInfo">اطلاعات اضافه</param>
        /// <param name="UserName">نام كاربري ايجاد كننده</param>
        /// <param name="ds">ديتاست</param>
        public void CreateWorkflowInstance(WorkflowInstance wfInstance, string userName, string firstPerformer, string extraInfo, int priorityNo, bool isAuthoforward=false)
        {
            try
            {
                //if (wfs.IsWorkflowCreatedBefore(this.WorkflowID, wfInstance.EntityID))
                //    throw new WFUserException("براي اين موجوديت قبلا ورك فلو ثبت شده است.");

                Workflow wf = wfs.GetWorkflowByID(this.WorkflowID);

                wfInstance.InsertUser = userName;
                wfInstance.InsertDate = DateTime.Now;
                wfInstance.WorkflowID = this.WorkflowID;
                wfInstance.WorkflowInstanceStatusID = (int)WorkflowInstanceStatusEnum.Waiting;
                wfInstance.ExtraInfo = extraInfo;

                if (string.IsNullOrEmpty(wfInstance.WorkflowInstanceTitle))
                    wfInstance.WorkflowInstanceTitle = wf.WorkflowTitle;
                if (string.IsNullOrEmpty(wfInstance.EntityName))
                    wfInstance.EntityName = wf.EntityName;
                if (string.IsNullOrEmpty(wfInstance.EntityTitle))
                    wfInstance.EntityTitle = wf.EntityTitle;
                if (string.IsNullOrEmpty(wfInstance.EntityUrl))
                    wfInstance.EntityUrl = wf.EntityUrl;

                // inserting to database
                var ObjectContext = wfs.ObjectContext;
                var task = (from o in ObjectContext.Task
                            where (o.TaskTypeID == 0 && o.WorkflowID == this.WorkflowID) // فعاليت آغازين
                            select o).First();

                TaskInstance ti = new TaskInstance();
                ti.TaskInstanceID = Guid.NewGuid();
                ti.PriorityID = priorityNo;
                ti.WorkflowInstanceID = wfInstance.WorkflowInstanceID;
                ti.TaskID = task.TaskID;
                ti.TaskCode = task.TaskCode;
                ti.TaskTitle = task.TaskTitle;
                if (string.IsNullOrEmpty(firstPerformer) == false)
                    ti.PerformerID = firstPerformer;
                else if (string.IsNullOrEmpty(task.PerformerID) == false)
                    ti.PerformerID = task.PerformerID;
                //else
                //    throw new Exception("PerformerID is not selected");
                ti.EntityName = wfInstance.EntityName;
                ti.EntityID = wfInstance.EntityID;
                ti.EntityTitle = wfInstance.EntityTitle;
                ti.EntityUrl = wfInstance.EntityUrl;
                //ti.EntityDateTime = wfInstance.entitydat
                ti.InsertUser = userName;
                ti.InsertDate = DateTime.Now;
                ti.TaskInstanceStatusID = (int)TaskInstanceStatusEnum.Waiting; // در حال انتظار
                ti.NotificationStatusID = (int)NotificationStatusEnum.Waiting; // در حال انتظار
                //ti.ExtraInt = isauthoforward;
                ti.ExtraInt2 = isAuthoforward ? 1 : 0;
                CreateWorkflowEventArgs WorkflowEventArg = new CreateWorkflowEventArgs(wfInstance, ti, userName);
                OnBeforeCreateWorkflow(WorkflowEventArg);

                ObjectContext.WorkflowInstance.AddObject(wfInstance);
                ObjectContext.TaskInstance.AddObject(ti);

                OnAfterCreateWorkflow(WorkflowEventArg);

                ObjectContext.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// بعد از ايجاد ورك فلو
        /// </summary>
        /// <param name="WorkflowEventArg"></param>
        protected virtual void OnAfterCreateWorkflow(CreateWorkflowEventArgs WorkflowEventArg)
        {
            if (AfterCreateWorkflow != null)
                AfterCreateWorkflow(this, WorkflowEventArg);
        }

        /// <summary>
        /// قبل از ايجاد ورك فلو
        /// </summary>
        /// <param name="WorkflowEventArg"></param>
        protected virtual void OnBeforeCreateWorkflow(CreateWorkflowEventArgs WorkflowEventArg)
        {
            if (BeforeCreateWorkflow != null)
                BeforeCreateWorkflow(this, (CreateWorkflowEventArgs)WorkflowEventArg);

            if (WorkflowEventArg.Cancel == true)
                throw new WFUserException(WorkflowEventArg.CancelMessage);
        }

        /// <summary>
        /// قبل از اتمام ورك فلو
        /// </summary>
        /// <param name="WorkflowEventArg"></param>
        protected virtual void OnBeforeFinishWorkflowInstance(FinishWorkflowEventArgs e)
        {
            if (e.WfInstance != null && e.WfInstance.ParentTaskInstanceID.HasValue) // اگر اين ورك فلو يك ساب فلو بود
            {
                TaskInstance ti = wfs.GetTaskInstanceByID(e.WfInstance.ParentTaskInstanceID.Value);
                SubflowTaskBase sfBase = (SubflowTaskBase)GetTaskClassByTaskCode(ti.TaskCode);
                string actionCode = sfBase.GetResultActionCode(e);
                TaskAction ta = wfs.GetTaskActionByCode(ti.WorkflowInstance.WorkflowID, actionCode);
                FinishTask(ti, ta, e.UserName, e.CurrentTask.PerformerID, e.CurrentTask.Comment, e.CurrentTask.TaskDueDate);
            }

            if (BeforeFinishWorkflow != null)
                BeforeFinishWorkflow(this, e);

            if (e.Cancel == true)
                throw new WFUserException(e.CancelMessage);
        }

        /// <summary>
        /// بعد از اتمام ورك فلو
        /// </summary>
        /// <param name="WorkflowEventArg"></param>
        protected virtual void OnAfterFinishWorkflowInstance(FinishWorkflowEventArgs WorkflowEventArg)
        {
            if (AfterFinishWorkflow != null)
                AfterFinishWorkflow(this, WorkflowEventArg);
        }
        #endregion

        /// <summary>
        /// اقدام كننده بعدي را ميگيرد
        /// </summary>
        /// <param name="taskInstance">فعاليت جاري</param>
        /// <param name="taskAction">اكشن</param>
        /// <returns></returns>
        public virtual string GetNextPerformerID(TaskInstance taskInstance, TaskAction taskAction)
        {
            return taskAction.Task1.PerformerID;
        }

        /// <summary>   
        /// صفحه مربوط به اتمام كار را براي فرمت كردن به ورك فلو ميفرستد
        /// </summary>
        /// <param name="page">صفحه</param>
        /// <param name="eventName">وضعيت صفحه</param>
        /// <param name="taskActionIDComboBox">كامبوي انتخاب اكشن</param>
        /// <param name="performerIDComboBox">كامبوي انتخاب اقدام كننده بعدي</param>
        /// <param name="taskInstance">نمونه فعاليت باز شده</param>
        /// <param name="taskAction">عملي كه قرار است انجام شود</param>
        public virtual void FormatFinishTaskPage(object page, FinishTaskUIEventsEnum eventName, object taskActionIDComboBox, object performerIDComboBox, TaskInstance taskInstance, TaskAction taskAction)
        {
            return;
        }

        /// <summary>

        /// چك ميكند كه يك ورك فلو براي آيتمي قبلا ايجاد شده است يا خير
        /// </summary>
        /// <param name="WorkflowCode">كد ورك فلو</param>
        /// <param name="entityID">شناسه رديف موجوديت</param>
        /// <returns></returns>
        public bool IsWorkflowCreatedBefore(string entityID)
        {
            return wfs.IsWorkflowCreatedBefore(this.WorkflowID, entityID);
        }

        /// <summary>
        /// تاريخچه كارهاي يك فرآيند را برميگرداند
        /// </summary>
        /// <param name="WorkflowInstanceId">شناسه نمونه فرآيند</param>
        /// <param name="filter">فيلتر تاريخچه كار</param>
        /// <param name="sort">سورت تاريخچه كار</param>
        /// <returns></returns>
        public List<TaskInstance> TaskInstanceGetListByWorkflowInstanceByFilter(Guid workflowInstanceId, System.Linq.Expressions.Expression<Func<TaskInstance, bool>> fi)
        {
            return wfs.TaskInstanceGetListByWorkflowInstanceByFilter(workflowInstanceId, fi);
        }

        /// <summary>
        /// آخرين فعاليت موجود در يك فرآيند را ميگيرد
        /// </summary>
        /// <param name="WorkflowInstanceId">شناسه نمونه فرآيند</param>
        /// <returns></returns>
        public TaskInstance TaskInstanceGetLastInWorkflowInstance(Guid workflowInstanceId)
        {
            WorkflowService wfs = new WorkflowService();
            using (wfs) { return wfs.TaskInstanceGetLastInWorkflowInstance(workflowInstanceId); }
        }

        /// <summary>
        /// نمونه فرآيند هاي ايجاد شده را بر اساس شناسه موجوديت ميگيرد
        /// </summary>
        /// <param name="entityID">شناسه موجوديت</param>
        /// <returns></returns>
        public List<WorkflowInstance> WorkflowInstanceGetByEntityID(string entityID)
        {
            return wfs.WorkflowInstanceGetByEntityID(entityID, this.WorkflowID);
        }

        /// <summary>
        /// آخرین فعالیت مربوط به هر انتیتی را بر میگرداند
        /// </summary>
        /// <param name="entityIDs"></param>
        /// <returns></returns>
        public Dictionary<string, TaskInstance> LastTaskInstanceGetByEntityID(List<string> entityIDs)
        {
            return wfs.TaskInstanceGetLastInWorkflowInstance(entityIDs, this.WorkflowID);
        }

        /// <summary>
        /// آخرين نسخه فعال از يك ورك فلو را ميگيرد
        /// </summary>
        /// <param name="entityID">شناسه موجوديت</param>
        /// <param name="workflowID">شناسه ورك فلو</param>
        /// <returns></returns>
        public WorkflowInstance WorkflowInstanceGetLastActiveByEntityID(string entityID)
        {
            return wfs.WorkflowInstanceGetLastActiveByEntityID(entityID, this.WorkflowID);
        }

        /// <summary>
        /// كلاس مربوط به يك فعاليت را مي گيرد
        /// </summary>
        /// <returns></returns>
        public TaskBase GetTaskClassByTaskCode(string taskCode)
        {
            try
            {
                string typeName = this.GetType().Namespace + ".Acts." + taskCode;
                Type factoryType = this.GetType().Assembly.GetType(typeName);
                if (factoryType == null)
                    throw new Exception("workflow " + taskCode + " is not exists");
                return (TaskBase)Activator.CreateInstance(factoryType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// تسک های یک فرآیند را برمیگرداند
        /// </summary>
        /// <returns></returns>
        public IQueryable<Task> GetWorkflowTasks()
        {
            return wfs.ObjectContext.Task.Where(t => t.WorkflowID == this.WorkflowID);
        }
    }
}
