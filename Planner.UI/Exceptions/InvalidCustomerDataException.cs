﻿using System;

namespace Planner.UI.Exceptions
{
    public class InvalidCustomerDataException : Exception
    {
        public InvalidCustomerDataException(string message)
            :base(message)
        {
        }
    }
}
