using System.Collections.Generic;
using System.Linq;
using Helpdesk.BusinessObjects;
using Infra.Common;
using System;
using System.Linq.Expressions;

namespace Helpdesk.BLL
{
    public class AttachmentManager : ManagerBase<Attachment>
    {
        //public IQueryable<Attachment> GetSpecificQuery(Expression<Func<Attachment, bool>> predicate)
        //{
        //    var result = GetQuery(predicate);

        //    return result.Select(m => new Attachment
        //    {
        //        AttachmentID = m.AttachmentID,
        //        RequestID = m.RequestID,
        //        FileName = m.FileName,
        //        FileType = m.FileType,
        //        FileContent = m.FileContent
        //    });
        //}
    }
}
