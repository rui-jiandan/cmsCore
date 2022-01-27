using FreeSql.DataAnnotations;

namespace LinCms.Entities
{
    /// <summary>
    /// excel数据源配置
    /// </summary>
    [Table(Name ="excel_config")]
    [Index("fileid_cellname", "fileid,cellname",true)]
    [Index("fileid_dbcolumname", "fileid,dbcolumname", true)]
    public class ExcelDataConfig: FullAduitEntity
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        public long FileId { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        [Column(StringLength = 100)]
        public string CellName { get; set; }
        /// <summary>
        /// 列类型
        /// </summary>
        [Column(StringLength =30)]
        public string CellType { get; set; }
        /// <summary>
        /// 数据库名称
        /// </summary>
        [Column(StringLength = 100)]
        public string DbColumnName { get; set; }
    }
}
