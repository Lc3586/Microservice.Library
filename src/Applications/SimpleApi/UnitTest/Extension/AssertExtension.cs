using AutoMapper.Internal;
using Library.Extension;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UnitTest.Extension
{
    /// <summary>
    /// 断言拓展类
    /// </summary>
    public static class AssertExtension
    {
        /// <summary>
        /// 是否相同
        /// <para>比较对象的类型必须与参考对象相同</para>
        /// </summary>
        /// <param name="obj">参考对象</param>
        /// <param name="bindingAttr">指定成员搜索方式</param>
        /// <param name="objs">比较对象</param>
        public static void AreEqual(this object obj, BindingFlags? bindingAttr, params object[] objs)
        {
            var type = obj?.GetType();

            Assert.NotNull(
                obj,
                $"{type?.FullName} 参考对象为空.");

            Assert.NotZero(
                objs.Length,
                $"比较对象数量为0.");

            for (int i = 0; i < objs.Length; i++)
            {
                Assert.NotNull(
                    objs[i],
                    $"第 {i} 个比较对象 {objs[i].GetType().FullName} 为空.");
            }

            var defaultFlag = bindingAttr ?? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

            var members = new List<MemberInfo>();
            members.AddRange(type.GetProperties(defaultFlag));
            members.AddRange(type.GetFields(defaultFlag));

            foreach (var m in members)
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    var type_target = objs[i].GetType();

                    object value0 = m.GetMemberValue(obj),
                        value1;

                    if (type_target == typeof(Dictionary<string, object>))
                    {
                        var obj_target = objs[i] as Dictionary<string, object>;
                        if (!obj_target.ContainsKey(m.Name))
                            continue;

                        value1 = obj_target[m.Name];
                    }
                    else
                    {
                        var m_target = type_target.GetMember(m.Name, defaultFlag)?[0];

                        if (m_target == null)
                            continue;

                        value1 = m_target.GetMemberValue(objs[i]);
                    }

                    Assert.AreEqual(
                        value0,
                        value1,
                        $"第 {i} 个比较对象 {type_target.FullName} {m.Name} 成员的值 {value1} \r\n与参考对象 {m.ReflectedType.FullName} {m.Name} 成员的值 {value0} \r\n不一致.");
                }
            }
        }
    }
}
