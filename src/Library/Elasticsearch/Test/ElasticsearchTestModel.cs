using Microservice.Library.Elasticsearch.Annotations;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.Elasticsearch
{
    [ElasticsearchType(RelationName = "ElasticsearchTestModel_v1", IdProperty = nameof(Id))]
    [ElasticsearchIndiceExtension(IndicesSubType = NestIndexSubType.Week)]
    class ElasticsearchTestModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Number(NumberType.Long)]
        public Int64 Id { get; set; }

        [Number]
        public int Type { get; set; }

        [Number]
        public double? Score { get; set; }

        [Number]
        public decimal? Distance { get; set; }

        [Keyword]
        public string Kewword_0 { get; set; }

        [Keyword]
        public string Kewword_1 { get; set; }

        [Keyword]
        public string Kewword_2 { get; set; }

        [Text]
        public string Text_0 { get; set; }

        [Text]
        public string Text_1 { get; set; }

        [Text]
        public string Text_2 { get; set; }

        [Date]
        public DateTime CreatedTime { get; set; }

        [Date]
        public DateTime? UpdateTime { get; set; }

        ///// <summary>
        ///// 映射
        ///// </summary>
        //public static Func<TypeMappingDescriptor<ElasticsearchTestModel>, ITypeMapping> MapSelector = o =>
        //{
        //    return o.AutoMap()
        //            .Properties(p => p
        //                              .Text(q => q
        //                                          .Name(s => s.Text_0)
        //                                          .Name(s => s.Text_1)
        //                                          .Name(s => s.Text_2)
        //                                   )
        //                              .Keyword(q => q
        //                                          .Name(s => s.Kewword_0)
        //                                          .Name(s => s.Kewword_1)
        //                                          .Name(s => s.Kewword_2)
        //                                   )
        //                       );
        //};
    }


}
