using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinCms.v1.Excel
{
 
    public interface IExcelFileService
    {
        Task CreateDataSoureAsync(long id);

        Task DeleteAsync(long id);

        Task InsertAsync(ExcelFileDto dto);
    }
}
