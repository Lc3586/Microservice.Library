using Entity.Common;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using Library.OpenApi.Annotations;
using Microsoft.AspNetCore.Http;

/* 
 * 文件信息业务模型
 */
namespace Model.Common.FileDTO
{
    /// <summary>
    /// MD5校验输出信息
    /// </summary>
    public class CheckMD5Response
    {
        /// <summary>
        /// 是否已上传过了
        /// </summary>
        /// <remarks>
        /// <para>如已上传则返回文件信息<see cref="FileInfo"/></para>
        /// </remarks>
        public bool Uploaded { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileInfo FileInfo { get; set; }
    }

    /// <summary>
    /// 图片上传参数
    /// </summary>
    public class ImageUploadParams
    {
        /// <summary>
        /// 文件
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// 文件名
        /// 注意:不指定文件名时将使用原始名称,使用Base64时使用Guid
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 链接或Base64字符串
        /// </summary>
        public string UrlOrBase64 { get; set; }

        /// <summary>
        /// 下载外链资源链接
        /// </summary>
        public bool Download { get; set; } = false;

        /// <summary>
        /// 保存至数据库
        /// </summary>
        public bool Save2Db { get; set; } = true;

        /// <summary>
        /// 图片转Base64链接
        /// </summary>
        public bool ToBase64Url { get; set; } = false;

        /// <summary>
        /// 图片转Base64
        /// </summary>
        public bool ToBase64 { get; set; } = false;

        /// <summary>
        /// 压缩图片
        /// 默认开启
        /// </summary>
        public bool IsCompress { get; set; } = true;

        /// <summary>
        /// 图片压缩选项
        /// 默认配置:按照200像素的宽度等比压缩图片，并且保存原图
        /// </summary>
        public ImageCompressOption CompressOption { get; set; } = new ImageCompressOption();
    }

    /// <summary>
    /// 图片压缩选项
    /// </summary>
    public class ImageCompressOption
    {
        /// <summary>
        /// 保存原图
        /// </summary>
        public bool SaveOriginal { get; set; } = true;

        /// <summary>
        /// 压缩后的宽度
        /// 注意:只设置高度时将进行等比压缩
        /// </summary>
        public int Width { get; set; } = 200;

        /// <summary>
        /// 压缩后的高度
        /// </summary>
        public int Height { get; set; } = 0;
    }

    /// <summary>
    /// 文件上传参数
    /// </summary>
    public class FileUploadParams
    {
        /// <summary>
        /// 文件
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// 文件名
        /// 注意:不指定文件名时将使用原始名称,使用Base64时使用Guid
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 外链资源链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 下载外链资源链接
        /// </summary>
        public bool Download { get; set; } = false;

        /// <summary>
        /// 保存至数据库
        /// </summary>
        public bool Save2Db { get; set; } = true;

        /// <summary>
        /// 压缩文件
        /// 默认关闭
        /// </summary>
        public bool IsCompress { get; set; } = false;

        /// <summary>
        /// 文件压缩选项
        /// </summary>
        public FileCompressOption CompressOption { get; set; } = new FileCompressOption();
    }

    /// <summary>
    /// 文件压缩选项
    /// </summary>
    public class FileCompressOption
    {
        /// <summary>
        /// 压缩比例
        /// </summary>
        public double Level { get; set; } = 0.8;
    }

    /// <summary>
    /// 文件信息
    /// </summary>
    [MapFrom(typeof(Common_File))]
    [MapTo(typeof(Common_File))]
    [OpenApiMainTag("FileInfo")]
    public class FileInfo : Common_File
    {
        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Common_File, FileInfo> FromMemberMapOptions =
            new MemberMapOptions<Common_File, FileInfo>();

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<FileInfo, Common_File> ToMemberMapOptions =
            new MemberMapOptions<FileInfo, Common_File>();
    }
}
