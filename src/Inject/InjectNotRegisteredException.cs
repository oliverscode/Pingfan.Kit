using System;

namespace Pingfan.Kit.Inject
{
    /// <summary>
    /// 未被注册的异常
    /// </summary>
    public class InjectNotRegisteredException : Exception
    {
        public InjectPop Pop { get; set; }

        /// <inheritdoc />
        public InjectNotRegisteredException(string message, InjectPop injectPop) : base(message)
        {
            Pop = injectPop;
        }
    }
}