using Model.Common.WeChatUserInfoDTO;
using Model.System.Pagination;
using System.Collections.Generic;

namespace Business.Interface.Common
{
    /// <summary>
    /// 微信用户信息业务接口类
    /// </summary>
    public interface IWeChatUserInfoBusiness
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination">分页设置</param>
        /// <returns></returns>
        List<List> GetList(PaginationDTO pagination);

        /// <summary>
        /// 获取详情数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Detail GetDetail(string id);

        /// <summary>
        /// 获取State参数
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        string GetState(StateInfo data);
    }
}
