﻿using System;
using System.Runtime.Serialization;

namespace Differencial.Domain.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}