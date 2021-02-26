using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Microservice.Library.DataMapping.Extention
{
    interface MemberConfigurationExpressionExtention : IMemberConfigurationExpression<object, object, object>
    {
        void MapFrom<TSource, TSourceMember>(Expression<Func<TSource, TSourceMember>> mapExpression);
    }
}
