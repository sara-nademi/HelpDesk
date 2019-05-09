using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using Infra.Common;

namespace Infra.WorkflowEngine
{
    public class WorkflowService : IDisposable
    {

        public HRMWFEntities ObjectContext;

        public WorkflowService()
        {
            ObjectContext = new HRMWFEntities();
        }

        bool isDisposing;

        ~WorkflowService()
        {
            if (isDisposing == false)
                if (ObjectContext != null)
                    ObjectContext.Dispose();
        }

        public void Dispose()
        {
            isDisposing = true;
            if (ObjectContext != null)
                ObjectContext.Dispose();
        }

        /// <summary>
        /// كلاس يك ورك فلو را از روي نام آن بر ميگرداند
        /// </summary>
        /// <param name="workflowCode">كد ورك فلو در ديتابيس</param>
        /// <returns></returns>
        public WorkflowBase GetWorkflowBaseByWorkflowCode(string workflowCode)
        {
            try
            {
                Workflow f = ObjectContext.Workflow.First(w => w.WorkflowCode == workflowCode);
                string wfTypeFullName = f.TypeFullName;
                if (string.IsNullOrEmpty(wfTypeFullName) == false)
                {
                    Type workflowType = Type.GetType(wfTypeFullName);
                    WorkflowBase w = (WorkflowBase)Activator.CreateInstance(workflowType);
                    w.WorkflowID = f.WorkflowID;
                    return w;
                }
                else
                    throw new Exception("Type for " + workflowCode + " is not exist");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// اطلاعات يك نمونه فعاليت را بر اساس شناسه آن ميدهد
        /// </summary>
        /// <param name="WorkflowID">شناسه نمونه فعاليت</param>
        /// <returns></returns>
        public Workflow GetWorkflowByID(int WorkflowID)
        {
            return (Workflow)GetObjectByID<Workflow>("Workflow", "Workflow", "WorkflowID", WorkflowID);
        }

        /// <summary>
        /// اطلاعات يك نمونه فعاليت را بر اساس شناسه آن ميدهد
        /// </summary>
        /// <param name="TaskInstanceID">شناسه نمونه فعاليت</param>
        /// <returns></returns>
        public TaskInstance GetTaskInstanceByID(Guid TaskInstanceID)
        {
            return (TaskInstance)GetObjectByID<TaskInstance>("TaskInstance", "TaskInstance", "TaskInstanceID", TaskInstanceID);
        }

        /// <summary>
        /// اكشن هاي يك فعاليت را بر ميگرداند
        /// </summary>
        /// <param name="taskID">شناسه فعاليت</param>
        /// <returns></returns>
        public List<TaskAction> GetActionsByTaskID(int taskID)
        {
            List<TaskAction> list = ObjectContext.TaskAction.Where(w => w.TaskID == taskID).ToList();
            return list;
        }

        /// <summary>
        /// اطلاعات مربوط به يك فعاليت را ميگيرد
        /// </summary>
        /// <param name="taskID">شناسه فعاليت</param>
        /// <returns></returns>
        public Task GetTaskByID(int taskID)
        {
            return (Task)GetObjectByID<Task>("Task", "Task", "TaskID", taskID);
        }

        /// <summary>
        /// يك اكشن را بر اساس شناسه آن بر ميگرداند
        /// </summary>
        /// <param name="taskActionID">شناسه اكشن</param>
        /// <returns></returns>
        public TaskAction GetTaskActionByID(int taskActionID)
        {
            return (TaskAction)GetObjectByID<TaskAction>("TaskAction", "TaskAction", "TaskActionID", taskActionID);
        }

        /// <summary>
        /// يك تسك اكشن را بر اساس كد آن ميگيرد
        /// </summary>
        /// <param name="workflowId">شناسه ورك فلو</param>
        /// <param name="taskActionCode">كد اكشن</param>
        /// <returns></returns>
        public TaskAction GetTaskActionByCode(int workflowId, string taskActionCode)
        {
            var q = ObjectContext.TaskAction.Where(w => w.TaskActionCode == taskActionCode && w.Task.WorkflowID == workflowId);
            return q.FirstOrDefault();
        }

        /// <summary>

        /// چك ميكند كه يك ورك فلو براي آيتمي قبلا ايجاد شده است يا خير
        /// </summary>
        /// <param name="workflowID">شناسه ورك فلو</param>
        /// <param name="entityID">شناسه رديف موجوديت</param>
        /// <returns></returns>
        public bool IsWorkflowCreatedBefore(int workflowID, string entityID)
        {
            if (ObjectContext.WorkflowInstance.Where(w => w.EntityID == entityID && w.WorkflowID == workflowID).Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// اطلاعات يك نمونه ورك فلو را بر اساس شناسه آن بر ميگرداند
        /// </summary>
        /// <param name="workflowInstanceId">اطلاعات نمونه ورك فلو</param>
        public WorkflowInstance GetWorkflowInstanceByID(Guid workflowInstanceId)
        {
            return (WorkflowInstance)GetObjectByID<WorkflowInstance>("WorkflowInstance", "WorkflowInstance", "WorkflowInstanceId", workflowInstanceId);
        }

        private object GetObjectByID<T>(string entityName, string entitySetName, string idFieldName, object id)
        {
            var l = GetObjectByForiegnKey<T>(entityName, entitySetName, idFieldName, id);
            if (l.Count > 0)
                return l[0];
            else
                return null;
        }

        private List<T> GetObjectByForiegnKey<T>(string entityName, string entitySetName, string fkeyName, object fkeyValue)
        {
            string sqls = string.Format("Select VALUE {0} From {1} As {0} Where {2} = @{3}", entityName, entitySetName, entityName + "." + fkeyName, fkeyName);
            ObjectParameter key = new ObjectParameter(fkeyName, fkeyValue);
            var l = ObjectContext.CreateQuery<T>(sqls, key).ToList<T>();
            return l;
        }

        /// <summary>
        /// يك ورك فلو را به طور كامل از ديتابيس حذف ميكند
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        public void WorkflowInstanceDelete(Guid workflowInstanceId)
        {
            WorkflowInstance wfI = GetWorkflowInstanceByID(workflowInstanceId);
            this.ObjectContext.DeleteObject(wfI); // cascade delete in db
            this.ObjectContext.SaveChanges();
        }

        #region TimeSheet

        /// <summary>
        /// تايم شيت را در ديتابيس آپديت ميكند
        /// </summary>
        /// <param name="TaskInstanceID">شناسه نمونه كار</param>
        /// <param name="personId">شناسه شخص انجام دهنده كار</param>
        public void StartPauseTimeSheet(Guid TaskInstanceID, int personId)
        {
            var q = from o in this.ObjectContext.TaskInstanceTimeSheet
                    where o.perID == personId && o.TaskInstanceID == TaskInstanceID && o.EndDateTime == null
                    orderby o.RowID descending
                    select o;

            TaskInstance ti = GetTaskInstanceByID(TaskInstanceID);

            TaskInstanceTimeSheet item = q.FirstOrDefault();
            if (item == null)
            {
                TaskInstanceTimeSheet timeobject = new TaskInstanceTimeSheet();
                timeobject.TaskInstanceID = TaskInstanceID;
                timeobject.perID = personId;
                timeobject.StartDateTime = DateTime.Now;
                timeobject.EndDateTime = null;
                ti.TimeSheetStart = true;
                this.ObjectContext.TaskInstanceTimeSheet.AddObject(timeobject);
                this.ObjectContext.SaveChanges();
            }
            else
            {
                item.EndDateTime = DateTime.Now;
                ti.TimeSheetStart = false;
                this.ObjectContext.SaveChanges();
            }

        }

        /// <summary>
        /// تايم شيت را در ديتابيس آپديت ميكند
        /// </summary>
        /// <param name="TaskInstanceID">شناسه نمونه كار</param>
        /// <param name="personId">شناسه شخص انجام دهنده كار</param>
        public void StopTimeSheet(Guid TaskInstanceID, int personId)
        {
            var q = from o in this.ObjectContext.TaskInstanceTimeSheet
                    where o.perID == personId && o.TaskInstanceID == TaskInstanceID && o.EndDateTime == null
                    orderby o.RowID descending
                    select o;

            TaskInstance ti = GetTaskInstanceByID(TaskInstanceID);
            TaskInstanceTimeSheet item = q.FirstOrDefault();
            if (item != null)
            {
                item.EndDateTime = DateTime.Now;
                ti.TimeSheetStart = false;
                this.ObjectContext.SaveChanges();
            }

        }

        #endregion

        public TaskAction TaskGetAssignTaskAction(int taskId)
        {
            var q = from o in this.ObjectContext.TaskAction
                    where o.TaskActionCode == "Assign" && o.TaskID == taskId
                    select o;
            var l = q.ToList();
            if (l.Count > 0)
                return l[0];
            else
                return null;
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
            WorkflowService wfs = new WorkflowService();
            using (wfs)
            {
                System.Linq.Expressions.Expression<Func<TaskInstance, bool>> filter = (w => w.WorkflowInstanceID == workflowInstanceId);
                if (fi != null)
                    System.Linq.Expressions.Expression.AndAlso(filter, fi);
                var q = wfs.ObjectContext.TaskInstance.Where(filter).OrderByDescending(w => w.InsertDate);
                return q.ToList();
            }
        }


        /// <summary>
        /// تاريخچه كارهاي يك فرآيند را برميگرداند
        /// </summary>
        /// <param name="WorkflowInstanceId">شناسه نمونه فرآيند</param>
        /// <param name="filter">فيلتر تاريخچه كار</param>
        /// <param name="sort">سورت تاريخچه كار</param>
        /// <returns></returns>
        public List<TaskInstance> TaskInstanceGetListByWorkflowInstanceByFilter(System.Linq.Expressions.Expression<Func<TaskInstance, bool>> fi)
        {
            var wfs = new WorkflowService();
            using (wfs)
            {
                var q = wfs.ObjectContext.TaskInstance.Where(fi).OrderByDescending(w => w.InsertDate);
                return q.ToList();
            }
        }

        /// <summary>
        /// آخرين فعاليت موجود در يك فرآيند را ميگيرد
        /// </summary>
        /// <param name="WorkflowInstanceId">شناسه نمونه فرآيند</param>
        /// <returns></returns>
        public TaskInstance TaskInstanceGetLastInWorkflowInstance(Guid workflowInstanceId)
        {
            var wfs = new WorkflowService();
            using (wfs)
            {
                var q = wfs.ObjectContext.TaskInstance
                    .Where(w => w.WorkflowInstanceID == workflowInstanceId)
                    .OrderByDescending(w => w.InsertDate);

                return q.FirstOrDefault();
            }
        }

        /// <summary>
        /// آخرين فعاليت موجود در هر انتیتی را ميگيرد
        /// </summary>
        /// <param name="entityIds">لیست سریال انتیتی ها</param>
        /// <returns></returns>
        public Dictionary<string, TaskInstance> TaskInstanceGetLastInWorkflowInstance(List<string> entityIds, int workflowID)
        {
            var wfs = new WorkflowService();
            using (wfs)
            {
                var entityIdListWithLastWorkflowInstance = from w in wfs.ObjectContext.WorkflowInstance
                                                           where entityIds.Contains(w.EntityID) && w.WorkflowID == workflowID
                                                           group w by w.EntityID
                                                               into wg
                                                               select new
                                                               {
                                                                   EntityID = wg.Key,
                                                                   LastWorkflowInstance = wg.OrderByDescending(o => o.InsertDate).FirstOrDefault()
                                                               };

                var workFlowInstanceIdListWithLastTaskInstance = from t in wfs.ObjectContext.TaskInstance
                                                                 group t by t.WorkflowInstanceID
                                                                     into tG
                                                                     select new
                                                                                {
                                                                                    WorkflowInstanceID = tG.Key,
                                                                                    LastTaskInstance = tG.OrderByDescending(ti => ti.InsertDate).FirstOrDefault()
                                                                                };
                var retVal = from e in entityIdListWithLastWorkflowInstance
                             join w in workFlowInstanceIdListWithLastTaskInstance on
                                 e.LastWorkflowInstance.WorkflowInstanceID equals w.WorkflowInstanceID
                             select new
                                        {
                                            e.EntityID,
                                            LastWorkflowInstanceID = w.WorkflowInstanceID,
                                            w.LastTaskInstance
                                        };

                return retVal.ToDictionary(r => r.EntityID, t => t.LastTaskInstance);
            }
        }

        /// <summary>
        /// نمونه فرآيند هاي ايجاد شده را بر اساس شناسه موجوديت ميگيرد
        /// </summary>
        /// <param name="entityID">شناسه موجوديت</param>
        /// <returns></returns>
        public List<WorkflowInstance> WorkflowInstanceGetByEntityID(string entityID, int workflowID)
        {
            var q = ObjectContext.WorkflowInstance
                .Where(w => w.EntityID == entityID && w.WorkflowID == workflowID)
                .OrderBy(w => w.InsertDate);

            return q.ToList();
        }

        /// <summary>
        /// آخرين نسخه فعال از يك ورك فلو را ميگيرد
        /// </summary>
        /// <param name="entityID">شناسه موجوديت</param>
        /// <param name="workflowID">شناسه ورك فلو</param>
        /// <returns></returns>
        public WorkflowInstance WorkflowInstanceGetLastActiveByEntityID(string entityID, int workflowID)
        {
            //int waiting = (int)WorkflowInstanceStatusEnum.Waiting;
            var q = ObjectContext.WorkflowInstance
                .Where(
                    w => w.EntityID == entityID && w.WorkflowID == workflowID)
                .OrderBy(w => w.InsertDate);
            return q.FirstOrDefault();


        }

        #region WorkflowInstanceParameter

        /// <summary>
        /// يك پارامتر نمونه ورك فلو را در ديتابيس ذخيره مي كند
        /// </summary>
        /// <param name="workflowInstanceID">شناسه نمونه ورك فلو</param>
        /// <param name="parameterName">نام پارامتر</param>
        /// <param name="parameterValue">مقدار پارامتر</param>
        public void WorkflowInstanceParameterSave(Guid workflowInstanceID, string parameterName, string parameterValue)
        {
            var q = ObjectContext.WorkflowInstanceParameter.Where(w => w.WorkflowInstanceID == workflowInstanceID && w.ParameterName == parameterName);
            var item = q.FirstOrDefault();
            if (item == null)
            {
                var p = new WorkflowInstanceParameter
                                                  {
                                                      WorkflowInstanceID = workflowInstanceID,
                                                      ParameterName = parameterName,
                                                      ParameterValue = parameterValue
                                                  };
                this.ObjectContext.WorkflowInstanceParameter.AddObject(p);
                this.ObjectContext.SaveChanges();
            }
            else
            {
                item.ParameterValue = parameterValue;
                this.ObjectContext.SaveChanges();
            }
        }

        /// <summary>
        /// يك پارامتر نمونه ورك فلو را ميگيرد
        /// </summary>
        /// <param name="workflowInstanceID">شناسه نمونه ورك فلو</param>
        /// <param name="parameterName">نام پارامتر</param>
        /// <returns></returns>
        public string WorkflowInstanceParameterGet(Guid workflowInstanceID, string parameterName)
        {
            var q = ObjectContext.WorkflowInstanceParameter.Where(w => w.WorkflowInstanceID == workflowInstanceID && w.ParameterName == parameterName);
            var item = q.FirstOrDefault();
            if (item == null)
                return null;
            else
                return item.ParameterValue;
        }

        #endregion

        /// <summary>
        /// بررسي مي كند كه آيا همه ساب فلو هاي يك اكتيويتي تمام شده اند يا نه
        /// براي جايي كه جواب همه ورك فلو ها بايد آمده باشد
        /// </summary>
        /// <param name="taskInstanceId">شناسه اكتيويتي</param>
        /// <returns></returns>
        public bool WorkflowInstanceIsAllSubflowInstancesFinished(Guid taskInstanceId)
        {
            var workflowCompletedStatus = (int)WorkflowInstanceStatusEnum.Complete;
            var q = this.ObjectContext.WorkflowInstance.Where(w => w.ParentTaskInstanceID == taskInstanceId && w.WorkflowInstanceStatusID == workflowCompletedStatus);
            var q2 = this.ObjectContext.WorkflowInstance.Where(w => w.ParentTaskInstanceID == taskInstanceId);
            if (q.Count() == q2.Count())
                return true;
            else
                return false;
        }

        /// <summary>
        /// ليست همه ساب فلو هاي يك فعاليت را ميگيرد
        /// </summary>
        /// <param name="taskInstanceId">شناسه فعاليت</param>
        /// <returns></returns>
        public List<WorkflowInstance> WorkflowInstanceGetSubflowByTaskInstanceID(Guid taskInstanceId)
        {
            int workflowCompletedStatus = (int)WorkflowInstanceStatusEnum.Complete;
            var q = this.ObjectContext.WorkflowInstance.Where(w => w.ParentTaskInstanceID == taskInstanceId && w.WorkflowInstanceStatusID == workflowCompletedStatus);
            return q.ToList();
        }

        #region by karamiq
        public List<Tuple<TaskInstance, TaskTypeEnum>> WorkflowInstanceGetLastStatus()
        {
            var taskinstanceTuples = new List<Tuple<TaskInstance, TaskTypeEnum>>();

            var wfs = new WorkflowService();
            using (wfs)
            {


                foreach (var wfi in wfs.ObjectContext.WorkflowInstance)
                {
                    var q = wfs.ObjectContext.TaskInstance.Where(w => w.WorkflowInstanceID == wfi.WorkflowInstanceID).OrderByDescending(w => w.InsertDate);

                    if (q.Any())
                    {
                        var ti = q.First();
                        taskinstanceTuples.Add(new Tuple<TaskInstance, TaskTypeEnum>(ti, (TaskTypeEnum)ti.Task.TaskTypeID));
                    }
                }

                return taskinstanceTuples;
            }
        }
        #endregion

        public TaskInstance GetFirstTaskInstance(Guid workflowInstanceId)
        {
            return ObjectContext.TaskInstance.Where(p => p.WorkflowInstanceID == workflowInstanceId).OrderBy(p => p.InsertDate)
                .FirstOrDefault();
        }
    }
}
