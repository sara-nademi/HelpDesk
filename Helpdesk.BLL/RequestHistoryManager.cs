using System;
using Helpdesk.BusinessObjects;
using Helpdesk.Common;

namespace Helpdesk.BLL
{
    public class RequestHistoryManager : ManagerBase<RequestHistory>
    {
        public void LogRequestHistory(Request requestEntity)
        {
            var requestHistory = new RequestHistory();
            var requestHistoryManager = new RequestHistoryManager();
            requestHistory.AssetNummber = requestEntity.AssetNummber;
            requestHistory.CalculatedPriority = requestEntity.CalculatedPriority;
            requestHistory.Comment = requestEntity.Comment;
            requestHistory.ConfirmationDate = requestEntity.ConfirmationDate;
            requestHistory.EntityIP = requestEntity.EntityIP;
            requestHistory.EntityMobile = requestEntity.EntityMobile;
            requestHistory.EntityPhone = requestEntity.EntityPhone;
            requestHistory.FinishDate = requestEntity.FinishDate;
            requestHistory.FromSilverlightID = requestEntity.FromSilverlightID;
            requestHistory.InsertDate = requestEntity.InsertDate;
            requestHistory.InsertUser = requestEntity.InsertUser;
            requestHistory.IsConfirmed = requestEntity.IsConfirmed;
            requestHistory.IsFinished = requestEntity.IsFinished;
            requestHistory.OptionalLocation = requestEntity.OptionalLocation;
            requestHistory.OptionalLocationID = requestEntity.OptionalLocationID;
            requestHistory.OwnnerEntityID = requestEntity.OwnnerEntityID;
            requestHistory.ParentRequestID = requestEntity.ParentRequestID;
            requestHistory.RegisterByName = requestEntity.RegisterByName;
            requestHistory.RegisteredByEntityID = requestEntity.RegisteredByEntityID;
            requestHistory.RequestID = requestEntity.RequestID;
            requestHistory.RequestPriority = requestEntity.RequestPriority;
            requestHistory.RequestTypeID = requestEntity.RequestTypeID;
            requestHistory.StatusID = requestEntity.StatusID;
            requestHistory.TS = requestEntity.TS;
            requestHistory.UpdateDate = requestEntity.UpdateDate;
            requestHistory.UpdateUser = requestEntity.UpdateUser;

            requestHistory.HistoryInsertDate = DateTime.Now;
            requestHistory.HistoryInsertUser = Utility.CurrentUserName;
            requestHistoryManager.Insert(requestHistory);
        }
    }
}
