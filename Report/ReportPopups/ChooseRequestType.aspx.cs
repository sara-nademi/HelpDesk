using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Helpdesk.BLL;

namespace Helpdesk.WebUI.ReportPopups
{
    public partial class ChooseRequestType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRadTreeView();
            }
        }
        
        private void LoadRadTreeView()
        {
            //Add First node to admintreeview
            var firstNode = new RadTreeNode("انواع درخواست ها");
            firstNode.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
            RadTreeViewRequestType.Nodes.Add(firstNode);

            RadTreeViewRequestType.CollapseAllNodes();
        }

        protected void RadTreeViewRequestType_Expand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Node.Value) && e.Node.Level == 0)
            {
                var requestTypeManager = new RequestTypeManager();

                //Select From DB
                var q = from r in requestTypeManager.GetQuery()
                        where (r.ParentRequestType == null && r.RequestTypeID != 0)
                        orderby r.DisplayOrder
                        select new { r.Title, r.RequestTypeID };

                foreach (var item in q.ToList())
                {
                    if (item.RequestTypeID != 0)
                    {
                        var node = new RadTreeNode();
                        node.Text = item.Title;
                        node.Value = item.RequestTypeID.ToString();
                        node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                        node.Attributes.Add("Level", (e.Node.Level + 1).ToString());
                        node.Category = (e.Node.Level + 1).ToString();
                        e.Node.Nodes.Add(node);                    
                    }
                }
            }
            else
            {
                var parentId = long.Parse(e.Node.Value);
                if (e.Node.Level < 3)
                {
                    var requestTypeManager = new RequestTypeManager();

                    var childNodes = (from r in requestTypeManager.GetQuery()
                                      where (r.ParentRequestType == parentId)
                                      orderby r.DisplayOrder//this line add by ahmadpoor
                                      select r).ToList();// new { r.Title, r.RequestTypeID };

                    foreach (var item in childNodes)
                    {
                        var node = new RadTreeNode
                        {
                            Text = item.Title,
                            Value = item.RequestTypeID.ToString(),
                            ExpandMode = TreeNodeExpandMode.ServerSideCallBack
                        };
                        //node.ToolTip = (e.Node.Level + 1).ToString();
                        node.Category = (e.Node.Level + 1).ToString();
                        e.Node.Nodes.Add(node);                    
                    }
                    //e.Node.ExpandMode = TreeNodeExpandMode.ClientSide;
                    e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                }
                else
                    e.Node.Expanded = false;
            }
        }
    }
}