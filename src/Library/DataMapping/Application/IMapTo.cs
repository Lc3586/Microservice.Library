namespace Microservice.Library.DataMapping.Application
{
    /// <summary>
    /// 数据映射目标配置
    /// </summary>
    /// <typeparam name="TSource">来源数据类型</typeparam>
    /// <typeparam name="TDestination">目标数据类型</typeparam>
    internal interface IMapTo<TSource, TDestination> : IMap where TSource : class where TDestination : class
    {
        /// <summary>
        /// 成员映射配置
        /// </summary>
        /// <remarks>
        /// <para>必须设置为静态属性</para>
        /// <para>可调用 <see cref="MemberMapOptions{TSource, TDestination}.Add(string, System.Action{AutoMapper.IMemberConfigurationExpression})"/> 方法添加配置</para>
        /// <para>可调用 <see cref="MemberMapOptions{TSource, TDestination}.Add{TSourceMember}(string, System.Func{TSource, TSourceMember})"/> 方法添加配置</para>
        /// <para><![CDATA[public static MemberMapOptions<AModel, AEntity> ToMemberMapOptions = new MemberMapOptions<AModel, AEntity>()
        ///                                                                 .Add(nameof(AEntity.A),o=> o.A1.ToString())
        ///                                                                 .Add(nameof(AEntity.B),o=> o.B1.ToString());]]></para>
        /// </remarks>
        MemberMapOptions<TSource, TDestination> ToMemberMapOptions { get; set; }
    }
}
