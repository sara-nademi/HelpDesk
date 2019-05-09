using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Infra.Common;
using Helpdesk.Common;

namespace Helpdesk.BLL.Convert
{
    public class PersonalListImport : IDataImport
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

        public PersonalListImport()
        {
            _db = new HRMWFEntities();
            _insertCount = 0;
            _updateCount = 0;
            _deleteCount = 0;
        }

        public void Import(string xmlDirectory)
        {
            var dsPersonnel = new DataSet("GetPersonnelList");
            dsPersonnel.ReadXml(xmlDirectory);
            Import(dsPersonnel);
        }

        public void Import(DataSet dataSet)
        {
            _db.ExecuteStoreCommand("UPDATE [InfraDB].[SM].[Entity] SET [IsDeleted] = 1 WHERE EntityTypeID = 2");
            _db.SaveChanges();
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var name = row["PERNAME"].ToString().Split(' ');
                var title = row["PERNAME"].ToString();
                var firstName = name[0];
                var lastName = name[1] + " " + (name.Length > 2 ? name[2] : "");
                var personalCardNo = row["CardNo"].ToString();
                var nationalCode = row["MelliCode"].ToString();
                var code = row["INFPERCODE"].ToString();
                var organizationId = GetOrganization(row["SETCODE"].ToString());
                var entityTypeid = "2";

                InsertOrDefault(personalCardNo, title, code, firstName, lastName, nationalCode, organizationId);
                _db.SaveChanges();
            }

            var deleteObject = _db.Entity.Where(p => p.IsDeleted && p.EntityTypeID == 2);
            _deleteCount = deleteObject.Count();
        }

        private long GetOrganization(string orgCode)
        {
            if (string.IsNullOrEmpty(orgCode)) return 0;
            var id = long.Parse(orgCode);
            var organization = _db.OrganizationChart.FirstOrDefault(p => p.OrganizationID == id);
            return organization == null ? 0 : organization.OrganizationID;
        }

        private void InsertOrDefault(string personalCardNo, string title, string code, string firstName, string lastName, string nationalCode,long organizationId)
        {
            var entity = _db.Entity.FirstOrDefault(p => p.PersonalCardNo == personalCardNo);

            if (entity == null)
            {
                entity = new Entity();
                entity.PersonalCode = code;
                entity.PersonalCardNo = personalCardNo;
                entity.Title = title;
                entity.EntityFirstName = firstName;
                entity.EntityLastName = lastName;
                entity.IsActive = true;
                entity.IsDeleted = false;
                entity.NationalCode = nationalCode;
                entity.InsertDate = DateTime.Now;
                entity.InsertUser = "convertor";
                entity.EntityTypeID = 2;
                if (organizationId != 0)
                    entity.OrganizationID1 = organizationId;

                _db.Entity.AddObject(entity);
                _insertCount++;
            }
            else
            {
                entity.PersonalCode = code;
                entity.Title = title;
                entity.EntityFirstName = firstName;
                entity.EntityLastName = lastName;
                entity.IsDeleted = false;
                entity.NationalCode = nationalCode;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUser = "convertor";
                entity.EntityTypeID = 2;
                if (organizationId != 0)
                    entity.OrganizationID1 = organizationId;

                _db.Entity.ApplyCurrentValues(entity);
                _updateCount++;
            }
        }

        ~PersonalListImport()
        {
            _db.Dispose();
            _db = null;
        }
    }
}
