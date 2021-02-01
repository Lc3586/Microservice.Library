using AutoMapper;
using Business.Interface.Common;
using Business.Utils;
using Entity.Common;
using FreeSql;
using Library.Container;
using Library.DataMapping.Gen;
using Library.Extension;
using Library.File;
using Library.FreeSql.Extention;
using Library.FreeSql.Gen;
using Library.Http;
using Library.Models;
using Library.OpenApi.Extention;
using Microsoft.AspNetCore.Http;
using Model.Common;
using Model.Common.FileDTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FileInfo = Model.Common.FileDTO.FileInfo;

namespace Business.Implementation.Common
{
    /// <summary>
    /// 文件处理业务类
    /// </summary>
    public class FileBusiness : BaseBusiness, IFileBusiness, IDependency
    {
        #region DI

        public FileBusiness(
            IFreeSqlProvider freeSqlProvider,
            IAutoMapperProvider autoMapperProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            Orm = freeSqlProvider.GetFreeSql();
            Mapper = autoMapperProvider.GetMapper();
            Repository = Orm.GetRepository<Common_File, string>();
            HttpContextAccessor = httpContextAccessor;
            PreviewDir = Path.GetDirectoryName("\\filetypes\\");
            BaseDir = Path.GetDirectoryName($"\\Upload\\{Operator.UserId}\\{DateTime.Now.ToUnixTimestamp()}\\");
            BaseDirPath = PathHelper.GetAbsolutePath($"~{BaseDir}\\");
        }

        #endregion

        #region 私有成员

        IMapper Mapper { get; set; }

        IFreeSql Orm { get; set; }

        IBaseRepository<Common_File, string> Repository { get; set; }

        IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 文件类型预览图存储路径根目录相对路径
        /// </summary>
        string PreviewDir { get; set; }

        /// <summary>
        /// 存储路径根目录相对路径
        /// </summary>
        string BaseDir { get; set; }

        /// <summary>
        /// 存储路径根目录绝对路径
        /// </summary>
        string BaseDirPath { get; set; }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="path">绝对路径</param>
        private void Save(Image image, string path)
        {
            try
            {
                image.Save(path);
            }
            catch (Exception ex)
            {
                var code = Marshal.GetLastWin32Error();
                throw new ApplicationException($"文件不支持: {code}", ex);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bytes">文件</param>
        /// <param name="path">绝对路径</param>
        private async Task Save(byte[] bytes, string path)
        {
            try
            {
                using var stream = File.Create(path);
                await stream.WriteAsync(bytes);
            }
            catch (Exception ex)
            {
                var code = Marshal.GetLastWin32Error();
                throw new ApplicationException($"文件不支持: {code}", ex);
            }
        }

        /// <summary>
        /// 获取预览图
        /// </summary>
        /// <param name="suffix">文件后缀</param>
        /// <returns></returns>
        private string GetPreviewImage(string suffix = null)
        {
            if (suffix.IsNullOrEmpty())
                goto empty;

            var preview = Path.Combine(PreviewDir, $"{suffix.TrimStart('.')}.png");
            if (File.Exists(PathHelper.GetAbsolutePath($"~{preview}\\")))
                return preview;

            empty:
            return Path.Combine(PreviewDir, "empty.png");
        }

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <param name="suffix">文件后缀(.jpg)</param>
        /// <returns></returns>
        private string GetFileType(string suffix)
        {
            switch (suffix)
            {
                case ".jpg":
                case ".png":
                case ".gif":
                case ".webp":
                    return FileType.图片;
                case ".mp3":
                case ".wmv":
                case ".flac":
                    return FileType.音频;
                case ".mp4":
                case ".avi":
                case ".mkv":
                case ".rmvb":
                case ".flv":
                    return FileType.视频;
                case ".xls":
                case ".xlsx":
                    return FileType.电子表格;
                case ".doc":
                case ".docx":
                case ".pdf":
                    return FileType.电子文档;
                case ".zip":
                case ".rar":
                case ".7z":
                case ".tar":
                case ".gz":
                default:
                    return FileType.压缩包;
            }
        }

        /// <summary>
        /// 输出图片
        /// </summary>
        /// <param name="response"></param>
        /// <param name="img"></param>
        private void ResponseImage(HttpResponse response, Image img)
        {
            using MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);
            var bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);

            response.ContentLength = bytes.Length;
            response.Body.Write(bytes);
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="response"></param>
        /// <param name="path"></param>
        private void ResponseFile(HttpResponse response, string path)
        {
            if (!File.Exists(path))
                throw new ApplicationException("文件不存在或已被删除");

            //response.SendFileAsync(path);
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            response.ContentLength = bytes.Length;
            response.Body.Write(bytes);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        private void DeleteFile(string path)
        {
            if (!File.Exists(path))
                return;

            File.Delete(path);
        }

        #endregion

        #region 外部接口

        public FileInfo SingleImage(ImageUploadParams option)
        {
            FileInfo result;
            Image image = null;

            if (!option.UrlOrBase64.IsNullOrEmpty())
            {
                var o = option.UrlOrBase64;

                result = new FileInfo
                {
                    Name = option.Name ?? Guid.NewGuid().ToString(),
                    FileType = FileType.图片
                };

                if (o.Contains("data:image"))
                {
                    result.FullName = $"{result.Name}.jpg";
                    result.Suffix = ".jpg";
                    result.ContentType = "image/jpg";

                    image = ImgHelper.GetImgFromBase64Url(o);
                }
                else
                {
                    result.FullName = $"{result.Name}";

                    result.StorageType = StorageType.Url;
                    result.Path = o;
                    result.ThumbnailPath = result.Path;
                }
            }
            else
            {
                result = new FileInfo
                {
                    FullName = option.File.FileName,
                    Name = option.File.FileName.Substring(0, option.File.FileName.LastIndexOf('.'))
                };

                result.Suffix = option.File.FileName.Replace(result.Name, "").ToLower();
                result.ContentType = option.File.ContentType;
                result.FileType = GetFileType(result.Suffix);

                using MemoryStream ms = new MemoryStream();
                {
                    option.File.CopyTo(ms);
                    image = new Bitmap(Image.FromStream(ms));
                }

                if (option.ToBase64 || option.ToBase64Url)
                {
                    if (option.ToBase64)
                    {
                        result.StorageType = StorageType.Base64;
                        result.Path = ImgHelper.ToBase64String(image);
                    }

                    if (option.ToBase64Url)
                    {
                        result.StorageType = StorageType.Base64Url;
                        result.Path = ImgHelper.ToBase64StringUrl(image);
                    }
                }
            }

            //路径是网络链接时不进行处理
            if (!result.ThumbnailPath.IsNullOrEmpty())
                goto success;

            if (option.IsCompress)
            {
                if (image.Width <= option.CompressOption.Width && (option.CompressOption.Height == 0 || image.Height <= option.CompressOption.Height))
                    goto Ignore;

                Image image_compress;
                if (option.CompressOption.Height <= 0)
                    image_compress = ImgHelper.CompressImg(image, option.CompressOption.Width);
                else
                    image_compress = ImgHelper.CompressImg(image, option.CompressOption.Width, option.CompressOption.Height);

                if (option.ToBase64 || option.ToBase64Url)
                {
                    if (option.ToBase64)
                        result.ThumbnailPath = ImgHelper.ToBase64String(image_compress);

                    if (option.ToBase64Url)
                        result.ThumbnailPath = ImgHelper.ToBase64StringUrl(image_compress);
                }
                else
                {
                    var thumbnailDir = Path.GetDirectoryName($"{BaseDir}\\Thumbnail\\");
                    var thumbnailDirPath = PathHelper.GetAbsolutePath($"~{thumbnailDir}\\");
                    var thumbnailPath = Path.Combine(thumbnailDir, result.FullName);

                    if (!Directory.Exists(thumbnailDirPath))
                        Directory.CreateDirectory(thumbnailDirPath);

                    Save(image_compress, Path.Combine(thumbnailDirPath, result.FullName));

                    result.ThumbnailPath = thumbnailPath;
                    //result.ThumbnailPath = $"{SystemConfig.systemConfig.WebRootUrl}{thumbnailPath}";

                    if (!option.CompressOption.SaveOriginal)
                        result.Path = result.ThumbnailPath;
                }
            }

            Ignore:

            if (!result.Path.IsNullOrEmpty())
                goto success;

            var originalPath = Path.Combine(BaseDir, result.FullName);

            result.Path = originalPath;
            //result.Path = $"{SystemConfig.systemConfig.WebRootUrl}{originalPath}";

            if (result.ThumbnailPath.IsNullOrEmpty())
                result.ThumbnailPath = result.Path;

            if (!Directory.Exists(BaseDirPath))
                Directory.CreateDirectory(BaseDirPath);

            Save(image, Path.Combine(BaseDirPath, result.FullName));

            result.Server = Config.WebRootUrl;
            result.Path = result.Path?.Replace('\\', '/');
            result.ThumbnailPath = result.ThumbnailPath?.Replace('\\', '/');
            result.StorageType = StorageType.Path;

            success:
            if (option.Save2Db)
            {
                var entity = Mapper.Map<Common_File>(result).InitEntity();
                Repository.Insert(entity);
            }

            return result;
        }

        public async Task<FileInfo> SingleFile(FileUploadParams option)
        {
            FileInfo result;

            byte[] bytes = null;

            if (!option.Url.IsNullOrEmpty())
            {
                result = new FileInfo
                {
                    Name = option.Name ?? Guid.NewGuid().ToString(),
                    FileType = FileType.外链资源
                };

                result.FullName = $"{result.Name}";

                result.StorageType = StorageType.Url;
                result.Path = option.Url;
                result.ThumbnailPath = GetPreviewImage();
            }
            else
            {
                result = new FileInfo
                {
                    FullName = option.File.FileName,
                    Name = option.File.FileName.Substring(0, option.File.FileName.LastIndexOf('.'))
                };

                result.Suffix = option.File.FileName.Replace(result.Name, "").ToLower();
                result.ContentType = option.File.ContentType;

                using MemoryStream ms = new MemoryStream();
                {
                    option.File.CopyTo(ms);
                    bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, bytes.Length);
                }
            }

            //路径是网络链接时不进行处理
            if (!result.ThumbnailPath.IsNullOrEmpty())
                goto success;

            if (option.IsCompress)
            {
                throw new ApplicationException("暂不支持文件在线压缩");
            }

            //Ignore:

            if (!result.Path.IsNullOrEmpty())
                goto success;

            var originalPath = Path.Combine(BaseDir, result.FullName);

            result.Path = originalPath;

            if (!Directory.Exists(BaseDirPath))
                Directory.CreateDirectory(BaseDirPath);

            await Save(bytes, Path.Combine(BaseDirPath, result.FullName));

            result.Server = Config.WebRootUrl;
            result.Path = result.Path?.Replace('\\', '/');
            result.FileType = GetFileType(result.Suffix);
            result.ThumbnailPath = GetPreviewImage(result.Suffix);
            result.StorageType = StorageType.Path;

            success:
            if (option.Save2Db)
            {
                var entity = Mapper.Map<Common_File>(result).InitEntity();
                Repository.Insert(entity);
            }

            return result;
        }

        public void Preview(string id)
        {
            var file = Repository.GetAndCheckNull(id, "文件不存在或已被删除");
            var path = PathHelper.GetAbsolutePath($"~{file.ThumbnailPath}");

            var response = HttpContextAccessor.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;

            var type = file.ThumbnailPath[file.ThumbnailPath.LastIndexOf('.')..];
            if (type == "jpg")
                type = "jpeg";

            response.ContentType = $"image/{type.TrimStart('.')}";
            response.Headers.Add("Content-Disposition", $"attachment; filename=\"{file.FullName}\"");
            ResponseFile(response, path);
        }

        public void Browse(string id)
        {
            var file = Repository.GetAndCheckNull(id, "文件不存在或已被删除");

            var response = HttpContextAccessor.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;

            response.ContentType = file.ContentType;
            response.Headers.Add("Content-Disposition", $"attachment; filename=\"{file.FullName}\"");

            switch (file.StorageType)
            {
                case StorageType.Base64Url:
                    ResponseImage(response, ImgHelper.GetImgFromBase64Url(file.Path));
                    break;
                case StorageType.Base64:
                    ResponseImage(response, ImgHelper.GetImgFromBase64(file.Path));
                    break;
                case StorageType.Url:
                    response.Redirect(file.Path);
                    break;
                case StorageType.Path:
                    ResponseFile(response, PathHelper.GetAbsolutePath($"~{file.Path}"));
                    break;
                default:
                    break;
            }
        }

        public void Download(string id)
        {
            var file = Repository.GetAndCheckNull(id, "文件不存在或已被删除");

            var response = HttpContextAccessor.HttpContext.Response;
            response.StatusCode = StatusCodes.Status200OK;

            response.ContentType = "applicatoin/octet-stream";
            response.Headers.Add("Content-Disposition", $"attachment; filename=\"{file.FullName}\"");//

            switch (file.StorageType)
            {
                case StorageType.Base64Url:
                    ResponseImage(response, ImgHelper.GetImgFromBase64Url(file.Path));
                    break;
                case StorageType.Base64:
                    ResponseImage(response, ImgHelper.GetImgFromBase64(file.Path));
                    break;
                case StorageType.Url:
                    response.Redirect(file.Path);
                    break;
                case StorageType.Path:
                    ResponseFile(response, PathHelper.GetAbsolutePath($"~{file.Path}"));
                    break;
                default:
                    break;
            }
        }

        public List<FileInfo> GetList(Pagination pagination)
        {
            var list = Orm.Select<Common_File>()
                        .Where(o => Operator.IsAdmin || o.CreatorId == Operator.UserId)
                        .ToDtoList<Common_File, FileInfo>(pagination, typeof(FileInfo).GetNamesWithTagAndOther(true, "_List"));

            return list;
        }

        public FileInfo GetDetail(string id)
        {
            var result = Mapper.Map<FileInfo>(Repository.GetAndCheckNull(id));
            return result;
        }

        public List<FileInfo> GetDetails(string ids)
        {
            var list = GetList(new Pagination { PageIndex = -1, DynamicFilterInfo = new List<PaginationDynamicFilterInfo> { new PaginationDynamicFilterInfo { Field = "Id", Compare = FilterCompare.inSet, Value = ids } } });
            return list;
        }

        public List<FileInfo> GetDetails(List<string> ids)
        {
            var list = GetList(new Pagination { PageIndex = -1, DynamicFilterInfo = new List<PaginationDynamicFilterInfo> { new PaginationDynamicFilterInfo { Field = "Id", Compare = FilterCompare.inSet, Value = ids } } });
            return list;
        }

        public void Delete(List<string> ids)
        {
            var files = Repository.Select.Where(c => ids.Contains(c.Id)).ToList(c => new { c.Id, c.ThumbnailPath, c.Path });

            foreach (var file in files)
            {
                if (!file.ThumbnailPath.IsNullOrEmpty())
                    DeleteFile(file.ThumbnailPath);
                if (!file.Path.IsNullOrEmpty())
                    DeleteFile(file.Path);
            }

            if (Repository.Delete(o => ids.Contains(o.Id)) <= 0)
                throw new ApplicationException("未删除任何数据");
        }

        #endregion
    }
}
