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
    public partial class ChooseLocation : System.Web.UI.Page
    {
        EntityManager entityManager = new EntityManager();
        MemberManager memberManager = new MemberManager();
        private LocationManager _locationManager =new LocationManager();           
        private IList<Location> _locations = new List<Location>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAdminRadTreeView();
            }
        }
        protected void TreeViewLocationRad_NodeExpand(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.Nodes.Clear();

            var parentId = long.Parse(e.Node.Value);
            if (e.Node.Level <3)
            {
                var childNodes = _locationManager.GetQuery(p => p.IsActive && p.ParentLocationID == parentId && p.IsDeleted == false);

                var treeNodeExpandMode = e.Node.Level == 3 - 1 ? TreeNodeExpandMode.ClientSide : TreeNodeExpandMode.ServerSide;

                foreach (var item in childNodes)
                {
                    var node = new RadTreeNode
                    {
                        Text = item.Title,
                        Value = item.LocationID.ToString(),
                       ExpandMode = treeNodeExpandMode
                    };
                    var tmpParent = long.Parse(node.Value);
                    //node.ToolTip =
                    node.Category =
                        _locationManager.GetQuery(p => p.IsActive && p.ParentLocationID == tmpParent && p.IsDeleted == false).
                            Count().ToString();

                    e.Node.Nodes.Add(node);
                }
                e.Node.ExpandMode = TreeNodeExpandMode.ClientSide;
            }
            else
                e.Node.Expanded = false;
        }



        private void LoadAdminRadTreeView()
        {
            //Select From DB
            var rootLocations = _locationManager.GetQuery(p => p.ParentLocationID == null && p.IsActive && p.IsDeleted == false).Select(m => new { m.Title, m.LocationID });

            //Add First node to admintreeview
            var firstNode = new RadTreeNode("محل های فیزیکی");

            TreeViewLocationRad.Nodes.Add(firstNode);

            foreach (var item in rootLocations)
            {
                var node = new RadTreeNode();
                node.Text = item.Title;
                node.Value = item.LocationID.ToString();
                node.ExpandMode = TreeNodeExpandMode.ServerSide;
                var tmpParent = long.Parse(node.Value);
                node.ToolTip =
                    _locationManager.GetQuery(p => p.IsActive && p.ParentLocationID == tmpParent && p.IsDeleted == false).
                        Count().ToString();
                firstNode.Nodes.Add(node);
            }

        }

    }
}