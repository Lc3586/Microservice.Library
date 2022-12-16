namespace Microservice.Library.DataMapping.Application
{
    /// <summary>
    /// 数据映射来源配置
    /// </summary>
    /// <typeparam name="TSource">来源数据类型</typeparam>
    /// <typeparam name="TDestination">目标数据类型</typeparam>
    internal interface IMapFrom<TSource, TDestination> : IMap where TSource : class where TDestination : class
    {
        /// <summary>
        /// 成员映射配置
        /// </summary>
        /// <remarks>
        /// <para>必须设置为静态属性</para>
        /// <para>可调用 <see cref="MemberMapOptions{TSource, TDestination}.Add(string, System.Action{AutoMapper.IMemberConfigurationExpression})"/> 方法添加配置</para>
        /// <para>可调用 <see cref="MemberMapOptions{TSource, TDestination}.Add{TSourceMember}(string, System.Func{TSource, TSourceMember})"/> 方法添加配置</para>
        /// <para><![CDATA[public static MemberMapOptions<AEntity, AModel> FromMemberMapOptions = new MemberMapOptions<AEntity, AModel>()
        ///                                                                 .Add(nameof(AModel.A1),o=> o.A.ToString())
        ///                                                                 .Add(nameof(AModel.B1),o=> o.B.ToString());]]></para>
        /// </remarks>
        MemberMapOptions<TSource, TDestination> FromMemberMapOptions { get; set; }
    }
}
