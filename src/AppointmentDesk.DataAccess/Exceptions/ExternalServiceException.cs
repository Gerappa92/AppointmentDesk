﻿using System;

namespace AppointmentDesk.DataAccess.Exceptions;

public class ExternalServiceException : Exception
{
    public ExternalServiceException(string message, Exception innerException) : base(message, innerException) { }
}