using System.ComponentModel.DataAnnotations;

namespace LinCms.v1.Excel
{
    public class ExcelFileDto
    {
        [Required(ErrorMessage = "必须传入文件Id")]
        [StringLength(30, ErrorMessage = "图书作者应小于30字符")]
        public long FileId { get; set; }
    }
}
