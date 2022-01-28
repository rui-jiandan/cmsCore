using FreeSql.DataAnnotations;

namespace LinCms.Entities
{
    /// <summary>
    ///  excel数据源文件
    /// </summary>
    [Table(Name ="excel_file")]
    public class ExcelFile : FullAduitEntity
    {

        /// <summary>
        /// 文件ID
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// 是否创建
        /// </summary>
        public bool IsCreate { get; set; }
    }
}
