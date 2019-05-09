using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Infra.Common;

namespace Helpdesk.WebUI.Report.ReportPopups
{
    public partial class ChooseOrganization : System.Web.UI.Page
    {
        //-----------------------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadRadTreeView();
            }
        }
        //-----------------------------------------------------------------------------------------
        private void LoadRadTreeView()
        {
            //Add First node to locationtreeview
            var firstNode = new RadTreeNode("ساختار سازمانی") { ExpandMode = TreeNodeExpandMode.ServerSideCallBack };
            TreeViewOrganizationRad.Nodes.Add(firstNode);

            firstNode.Expanded = false;
        }
        //-----------------------------------------------------------------------------------------
        protected void TreeViewOrganizationRad_NodeExpand(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            var organizationManager = new OrganizationChartManager();
            if (string.IsNullOrEmpty(e.Node.Value) && e.Node.Level == 0)
            {
                var q = organizationManager.GetLocationNullParent();

                foreach (var item in q.ToList())
                {
                    var radnode = new RadTreeNode(item.Title, item.OrganizationID.ToString())
                    {
                        ExpandMode = TreeNodeExpandMode.ServerSideCallBack,
                        //ToolTip = item.Weight.ToString()
                        Category = item.Weight.ToString() + "-" + item.DisregardPriority.ToString()
                    };
                    radnode.Text = radnode.Text;// +" - " + radnode.Category;
                    e.Node.Nodes.Add(radnode);
                }

            }
            else
            {
                var parentId = long.Parse(e.Node.Value);

                if (e.Node.Level < 5)
                {
                    var childLocation = new OrganizationChartManager().GetChildLocation(parentId).ToList();

                    TreeNodeExpandMode treeNodeExpandMode = e.Node.Level == 5 - 1 ? TreeNodeExpandMode.ClientSide : TreeNodeExpandMode.ServerSideCallBack;

                    foreach (var item in childLocation)
                    {
                        var radnode = new RadTreeNode(item.Title, item.OrganizationID.ToString()) { ExpandMode = treeNodeExpandMode, Category = item.Weight.ToString() + "-" + item.DisregardPriority.ToString() };
                        radnode.Text = radnode.Text;// +" - " + radnode.Category;
                        e.Node.Nodes.Add(radnode);
                    }
                    e.Node.ExpandMode = TreeNodeExpandMode.ServerSideCallBack;
                }
                else
                    e.Node.Expanded = false;
            }
        }
        //-----------------------------------------------------------------------------------------

    }
}