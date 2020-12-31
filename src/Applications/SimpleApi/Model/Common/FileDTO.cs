using Entity.Common;
using Library.OpenApi.Annotations;
using Library.DataMapping.Annotations;
using Library.DataMapping.Application;
using System;
using Microsoft.AspNetCore.Http;

/// <summary>
/// 文件信息业务模型
/// </summary>
namespace Model.Common.FileDTO
{
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
        /// Id
        /// </summary>
        public string Id_ { get; set; }

        /// <summary>
        /// 来源成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<Common_File, FileInfo> FromMemberMapOptions =
            new MemberMapOptions<Common_File, FileInfo>().Add(nameof(Id_), o => o.Id.ToString());

        /// <summary>
        /// 当前成员映射选项
        /// </summary>
        [OpenApiIgnore]
        public static MemberMapOptions<FileInfo, Common_File> ToMemberMapOptions =
            new MemberMapOptions<FileInfo, Common_File>().Add(nameof(Id), o => Convert.ToInt64(o.Id_));
    }
}
