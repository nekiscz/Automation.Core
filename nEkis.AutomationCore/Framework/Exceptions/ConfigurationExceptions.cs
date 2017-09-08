using System;

namespace nEkis.Automation.Core.Framework.Exceptions
{
    class UnknownConfiguration : Exception
    {
        public UnknownConfiguration(string message) : base(message) { }

        public UnknownConfiguration(string message, Exception inner) : base(message, inner) { }
    }
}
