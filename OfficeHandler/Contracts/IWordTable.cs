using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;

namespace OfficeHandler.Contracts
{
    public interface IWordTable
    {
        DataTable FindTable(string filepath);
    }
}
