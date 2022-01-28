using System.Threading.Tasks;
using LinCms.Aop.Filter;
using LinCms.Data;
using LinCms.v1.Excel;
using Microsoft.AspNetCore.Mvc;

namespace LinCms.Controllers.v1
{
    public class ExcelFileController: ControllerBase
    {
        private readonly IExcelFileService _service;

        public ExcelFileController(IExcelFileService service)
        {
            _service = service;
        }

        [HttpPost("")]
        public async Task<UnifyResponseDto> CreateAsync([FromBody] ExcelFileDto dto) 
        {
            await _service.InsertAsync(dto);
            return UnifyResponseDto.Success();
        }
    }
}
