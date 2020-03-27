using Library.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Handler
{
    public class Elasticsearch
    {
        public ElasticsearchTest elasticsearchTest;

        public Elasticsearch()
        {
            elasticsearchTest = new ElasticsearchTest();
        }

        public void Generate(int total, bool consoleLog)
        {
            elasticsearchTest.AddTestData(consoleLog, total);
        }

        public void Search(string keyword)
        {
            elasticsearchTest.Search(keyword);
        }
    }
}
