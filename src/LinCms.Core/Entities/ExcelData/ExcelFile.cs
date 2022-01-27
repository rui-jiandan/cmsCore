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
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 是否创建
        /// </summary>
        public bool IsCreate { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 大小 KB
        /// </summary>
        public decimal? Size { get; set; }
    }
}
