using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Yad.Database {
    interface IQueryReader {
        void ReadData(IDataReader reader);
        void SetFailure();
    }
}
