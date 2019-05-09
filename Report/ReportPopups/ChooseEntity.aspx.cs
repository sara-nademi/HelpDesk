using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infra.Common;
using Helpdesk.Common;
using Telerik.Web.UI;
using System.Threading;
namespace Helpdesk.WebUI.Report.ReportPopups
{
    public partial class ChooseEntity : System.Web.UI.Page
    {
        private EntityManager _entityManager = new EntityManager();
        private MemberManager _memberManager = new MemberManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FullDropDownListRole();
            }
        }

        private void FullDropDownListRole()
        {
            try
            {
                var rols = _entityManager.GetQuery(c => c.EntityTypeID == 7);

                foreach (var ra in rols)
                {
                    var li = new RadComboBoxItem
                    {
                        Text = ra.Title,
                        Value = ra.EntityID.ToString()
                    };

                    RadComboBoxRole.Items.Add(li);
                }

            }
            catch { }
        }


        protected void RadComboBoxRole_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var value = RadComboBoxRole.SelectedItem.Value.ToString();
                if (value != "0000")
                {
                    RadComboBoxGroup.Items.Clear();
                    var li2 = new RadComboBoxItem { Text = "همه گروه ها", Value = "000" };
                    li2.Selected = true;
                    RadComboBoxGroup.Items.Add(li2);   

                    RadComboBoxUser.Items.Clear();
                    var li23 = new RadComboBoxItem { Text = "همه اشخاص", Value = "00" };
                    li23.Selected = true;
                    RadComboBoxUser.Items.Add(li23);

                    var rols = _memberManager.GetMemberOfEntity(long.Parse(value));
                    foreach (var role in rols)
                    {
                        var li = new RadComboBoxItem { Text = role.Title, Value = role.EntityID.ToString() };
                        RadComboBoxGroup.Items.Add(li);
                    }
                }
                else
                {
                    RadComboBoxGroup.Items.Clear();
                    var li2 = new RadComboBoxItem { Text = "همه گروه ها", Value = "000" };
                    li2.Selected = true;
                    RadComboBoxGroup.Items.Add(li2);
                    RadComboBoxUser.Items.Clear();
                    var li23 = new RadComboBoxItem { Text = "همه اشخاص", Value = "00" };
                    li23.Selected = true;
                    RadComboBoxUser.Items.Add(li23);
                }
            }
            catch { }
        }

        protected void RadComboBoxGroup_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var value = RadComboBoxGroup.SelectedItem.Value.ToString();
                if (value != "000")
                {
                    RadComboBoxUser.Items.Clear();
                    var li2 = new RadComboBoxItem { Text = "همه اشخاص", Value = "00" };
                    RadComboBoxUser.Items.Add(li2);
                    var rols = _memberManager.GetMemberOfEntity(long.Parse(value)).Select(m => new { EntityTypeID = m.EntityTypeID, Title = m.Title, EntityID = m.EntityID });
                    foreach (var role in rols)
                    {
                        if (role.EntityTypeID == 2)
                        {
                            var li = new RadComboBoxItem { Text = role.Title, Value = role.EntityID.ToString() };
                            RadComboBoxUser.Items.Add(li);
                        }
                    }
                }
                else
                {
                    RadComboBoxUser.Items.Clear();

                    var li2 = new RadComboBoxItem { Text = "همه اشخاص", Value = "00" };
                    RadComboBoxUser.Items.Add(li2);
                }
            }
            catch { }
        }

    }
}