using Microservice.Library.Container;
using SuperSocket.ProtoBase;
using System;
using System.Buffers;

namespace Microservice.Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// JT协议流数据拦截器
    /// </summary>
    public abstract class JTFilter : PipelineFilterBase<MessagePackageInfo>
    {
        /// <summary>
        /// JT协议
        /// </summary>
        protected static JTProtocol JT { get; set; } = AutofacHelper.GetService<JTProtocol>();

        private readonly ReadOnlyMemory<byte> _beginMark;

        private readonly ReadOnlyMemory<byte> _endMark;

        private bool _foundBeginMark;

        protected JTFilter()
        {
            _beginMark = JT.HeadFlagValue;
            _endMark = JT.EndFlagValue;
            Decoder = new JTDecoder(JT);
        }

        public override MessagePackageInfo Filter(ref SequenceReader<byte> reader)
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
