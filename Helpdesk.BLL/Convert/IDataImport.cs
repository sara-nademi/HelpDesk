using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Helpdesk.BLL.Convert
{
    public interface IDataImport
    {
        long InsertedCount { get; }
        long UpdateCount { get; }
        long DeleteCount { get; }
        void Import(DataSet dataSet);
        void Import(string xmlPath);
    }
}
