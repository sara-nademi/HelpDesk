using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Infra.Common;

namespace Helpdesk.BLL.Convert
{
    public class OrganizationChartImport : IDataImport
    {
        private HRMWFEntities _db;

        private long _insertCount;

        public long InsertedCount
        {
            get { return _insertCount; }
        }

        private long _updateCount;

        public long UpdateCount
        {
            get { return _updateCount; }
        }

        private long _deleteCount;

        public long DeleteCount
        {
            get { return _deleteCount; }
        }

        public OrganizationChartImport()
        {
            _db = new HRMWFEntities();
            _insertCount = 0;
            _updateCount = 0;
        }

        public void Import(DataSet dataSet)
        {
            var parentDic = new Dictionary<string, OrganizationChart>();

            var allRecords = _db.OrganizationChart.Where(p => p.IsDeleted || !p.IsDeleted).ToList();
            allRecords.ForEach(p => p.IsDeleted = true);
            _db.SaveChanges();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var childCode = row["SETCODE"].ToString();
                var parentCode = row["PARENTSETCODE"].ToString();
                var title = row["SETDSCP"].ToString();

                var orgId = int.Parse(childCode);
                var org = _db.OrganizationChart.FirstOrDefault(p => p.OrganizationID == orgId);
                if (org == null)
                {
                    var organizationChart = new OrganizationChart
                    {
                        OrganizationID = long.Parse(childCode),
                        ParentOrganizatinID = long.Parse(parentCode),
                        Title = title,
                        IsActive = true,
                        IsDeleted = false,
                        OrganizationTypeID = 1,
                        Weight = 0,
                        InsertDate = DateTime.Now,
                        InsertUser = "convertor"

                    };
                    _db.OrganizationChart.AddObject(organizationChart);
                    _insertCount++;
                }
                else
                {
                    org.ParentOrganizatinID = long.Parse(parentCode);
                    org.Title = title;
                    org.IsDeleted = false;
                    _updateCount++;
                    _db.OrganizationChart.ApplyCurrentValues(org);
                }
            }
            _db.SaveChanges();

            var deletedObjects = _db.OrganizationChart.Where(p => p.IsDeleted);
            _deleteCount = deletedObjects.Count();
        }

        public void Import(string xmlDirectory)
        {
            var dsOrganization = new DataSet("GetOrganizationalStructure");
            dsOrganization.ReadXml(xmlDirectory);
            Import(dsOrganization);
        }

    }
}
