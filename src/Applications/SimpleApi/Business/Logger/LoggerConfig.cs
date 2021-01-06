namespace Business
{
    class LoggerConfig
    {
        public static readonly string LoggerName = "System_Log";
        public static readonly string LogType = "LogType";
        public static readonly string CreatorId = "CreatorId";
        public static readonly string CreatorName = "CreatorName";
        public static readonly string Data = "Data";
        public static readonly string Layout = $@"${{date:format=yyyy-MM-dd HH\:mm\:ss}}|${{level}}|日志类型:${{event-properties:item={LogType}}}|操作员:${{event-properties:item={CreatorName}}}|内容:${{message}}|备份数据:${{event-properties:item={Data}}}";
    }
}