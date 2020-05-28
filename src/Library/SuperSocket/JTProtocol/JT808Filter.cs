using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// JT808协议流数据拦截器
    /// </summary>
    public abstract class JT808Filter<TPackageInfo> : PipelineFilterBase<TPackageInfo>
        where TPackageInfo : class
    {
        private readonly ReadOnlyMemory<byte> _beginMark;

        private readonly ReadOnlyMemory<byte> _endMark;

        private bool _foundBeginMark;

        /// <summary>
        /// 分包
        /// key:消息ID
        /// value:消息体buffer数组
        /// </summary>
        private Dictionary<UInt16, object[]> Subpackage { get; set; }

        protected JT808Filter(ReadOnlyMemory<byte> beginMark, ReadOnlyMemory<byte> endMark)
        {
            _beginMark = beginMark;
            _endMark = endMark;
        }

        public override TPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            if (!_foundBeginMark)
            {
                var beginMark = _beginMark.Span;

                tryAdvance:
                if (!reader.TryAdvanceTo(beginMark[0]))
                    return null;

                if (beginMark.Length > 1)
                    if (!reader.IsNext(beginMark.Slice(1), advancePast: true))
                        goto tryAdvance;

                _foundBeginMark = true;
            }

            var endMark = _endMark.Span;

            if (!reader.TryReadTo(out ReadOnlySequence<byte> buffer, endMark, advancePastDelimiter: false))
            {
                return null;
            }

            reader.Advance(endMark.Length);
            return DecodePackage(ref buffer);
        }

        public override void Reset()
        {
            _foundBeginMark = false;
        }
    }
}
