using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LinCms.Data;
using LinCms.Entities;
using LinCms.Exceptions;
using LinCms.Extensions;
using LinCms.IRepositories;

namespace LinCms.v1.Excel
{
    public class ExcelFileService : ApplicationService, IExcelFileService
    {
        private readonly IAuditBaseRepository<ExcelFile> _excelFileRepository;

        private readonly IAuditBaseRepository<LinFile, long> _fileRepository;

        public ExcelFileService(IAuditBaseRepository<ExcelFile> excelFileRepository, IAuditBaseRepository<LinFile, long> fileRepository)
        {
            _excelFileRepository=excelFileRepository;
            _fileRepository=fileRepository;
        }

        public Task CreateDataSoureAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(ExcelFileDto dto)
        {
            var linFile = await _fileRepository.GetAsync(dto.FileId);
            throw new NotImplementedException();
        }
    }
}
