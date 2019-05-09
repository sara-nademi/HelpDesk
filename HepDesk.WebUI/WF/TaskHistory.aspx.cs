using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infra.Common;
using Infra.Common.WebUI;
using Infra.WorkflowEngine;
using Helpdesk.BLL;
using Telerik.Web.UI;

namespace Helpdesk.WebUI.WF
{
    public partial class TaskHistory : System.Web.UI.Page
    {
        public class DesciptionHistory
        {
            public string Sender { get; set; }

            public string SenderName
            {
                get
                {
                    var entityManager = new EntityManager();
                    return entityManager.GetQuery(p => p.PersonalCardNo == Sender).Select(p => p.EntityFirstName + " " + p.EntityLastName).FirstOrDefault();
                }
            }

            public string Reciever { get; set; }

            public string RecieverName
            {
                get
                {
                    Guid? g = new Guid("00000000-0000-0000-0000-000000000000");
                    if (TmpTaskInstanceId == g)
                    {
                        return ResiverName2;
                    }
                    TaskInstance taskInstance;
                    using (var db = new HRMWFEntities())
                    {
                        taskInstance =
                            db.TaskInstance.Where(p => p.PreviousTaskInstanceID == TmpTaskInstanceId).FirstOrDefault();

                    }
                    if (taskInstance != null)
                    {
                        Reciever = taskInstance.PerformerID;
                        var entityManager = new EntityManager();
                        return entityManager.GetQuery(p => p.PersonalCardNo == Reciever).Select(p => p.EntityFirstName + " " + p.EntityLastName).FirstOrDefault();
                    }
                    return "";
                }
                
            }

            public string TaskTitle { get; set; }
            public int Row { get; set; }
            public string ResiverName2 { get; set; }
            public string Date2 { get; set; }
            public string Date
            {
                get
                {
                    Guid? g = new Guid("00000000-0000-0000-0000-000000000000");
                    if (TmpTaskInstanceId == g)
                    {
                        return Date2;
                    }
                    TaskInstance taskInstance;
                    using (var db = new HRMWFEntities())
                    {
                        taskInstance =
                            db.TaskInstance.Where(p => p.PreviousTaskInstanceID == TmpTaskInstanceId).FirstOrDefault();
                    } 
                    if (taskInstance != null)
                    {

                        return UIUtils.ToPersianDate(taskInstance.InsertDate).ToPersinDigit();
                    }
                    return "";
                }
                
            }

            public string Description { get; set; }

            public Guid? TmpTaskInstanceId { get; set; }
        }

        public Guid _WorkflowInstanceID
        {
            get
            {
                return Request.QueryString["ParentID"] != null ? new Guid(Request.QueryString["ParentID"]) : Guid.Empty;
            }
        }
        protected override void OnInit(EventArgs e)
        {
           // IsAuthenticate();
           // base.OnInit(e);
          ///  if (!IsPostBack)
           // {
                PopulateHistory();
           // }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private List<DesciptionHistory> PopulateHistory()
        {
            int row = 2;
            using (var db = new HRMWFEntities())
            {
                var dataSource =
                    db.TaskInstance.Include("WorkflowInstance")
                                    .Include("TaskAction")
                                    .Include("TaskInstanceStatus")
                                    .Where(ti => ti.WorkflowInstanceID == _WorkflowInstanceID)// && ti.TaskInstanceStatusID == (int)TaskInstanceStatusEnum.Complete
                    //.ToList()
                                    .OrderBy(p => p.InsertDate)
                                    .ToList();
                var list = new List<DesciptionHistory>();
                if (dataSource.Count > 0)
                {
                    list.AddRange(dataSource.Select(item => new DesciptionHistory
                    {
                        Sender = item.PerformerID, //Reciever = item.PerformerID,
                       // Date =item.InsertDate != ""? UIUtils.ToPersianDate(item.InsertDate).ToPersinDigit(),
                        Description = item.Comment,
                        TaskTitle = item.TaskAction != null ? item.TaskAction.TaskActionTitle : "",
                        Row=row++,
                        TmpTaskInstanceId = item.TaskInstanceID
                    }));

                    var _entityID = dataSource.FirstOrDefault().EntityID;
                    if (_entityID != null)
                    {
                        long _eID=long.Parse( _entityID);
                        var _Request = new RequestManager().GetQuery(c => c.RequestID ==_eID).FirstOrDefault();
                        if (_Request != null)
                        {
                             var entityManager = new EntityManager();
                             var user= entityManager.GetQuery(p => p.EntityID == _Request.RegisteredByEntityID).FirstOrDefault();

                           DesciptionHistory des=new DesciptionHistory();
                           des.Sender = user.PersonalCardNo;
                            des.Date2= UIUtils.ToPersianDate(_Request.InsertDate).ToPersinDigit();
                            des.Description = _Request.Comment;
                            des.TmpTaskInstanceId = new Guid("00000000-0000-0000-0000-000000000000");
                            var id = dataSource.FirstOrDefault().PerformerID;
                            var resiver = entityManager.GetQuery(p => p.PersonalCardNo == id).FirstOrDefault();
                            des.ResiverName2 = resiver.Title; ;
                            des.Row = 1;
                             des.TaskTitle="ارسال خرابی";
                            list.Add(des);
                        }
                    }
                }
               
                return list.OrderBy(c => c.Row).ToList();
            }
        }
       
      
        protected void TaskInstanceRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
           
                TaskInstanceRadGrid.DataSource = PopulateHistory();
                //TaskInstanceRadGrid.DataBind();
            
        }

    }
}