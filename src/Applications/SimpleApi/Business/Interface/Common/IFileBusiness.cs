using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Model.Common.FileDTO;
using System.Threading.Tasks;

namespace Business.Interface.Common
{
    /// <summary>
    /// 文件处理业务接口类
    /// </summary>
    public interface IFileBusiness
    {
        /// <summary>
        /// MD5校验
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        CheckMD5Response CheckMD5(string md5);

        /// <summary>
        /// 单图上传
        /// </summary>
        /// <remarks>单个上传</remarks>
        /// <param name="option">分页设置</param>
        /// <returns></returns>
        FileInfo SingleImage(ImageUploadParams option);

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <remarks>单个上传</remarks>
        /// <param name="option">分页设置</param>
        /// <returns></returns>
        Task<FileInfo> SingleFile(FileUploadParams option);



        /// <summary>
        /// 预览
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        void Preview(string id);

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        void Browse(string id);

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        void Download(string id);

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<FileInfo> GetList(Pagination pagination);

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FileInfo GetDetail(string id);

        /// <summary>
        /// 获取详情数据集合
        /// </summary>
        /// <param name="ids">id逗号拼接</param>
        /// <returns></returns>
        List<FileInfo> GetDetails(string ids);

        /// <summary>
        /// 获取详情数据集合
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        List<FileInfo> GetDetails(List<string> ids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">Id集合</param>
        /// <returns></returns>
        void Delete(List<string> ids);
    }
}
