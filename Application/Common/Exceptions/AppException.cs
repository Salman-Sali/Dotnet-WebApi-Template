﻿using System.Net;

namespace Application.Common.Exceptions
{
    public class AppException : ApplicationException
    {
        public string SystemMessage { get; }
        public string FriendlyMessage { get; }
        public HttpStatusCode HttpStatusCode { get; }

        public AppException(string systemMessage, string friendlyMessage, HttpStatusCode httpStatusCode) : base(systemMessage)
        {
            FriendlyMessage = friendlyMessage;
            HttpStatusCode = httpStatusCode;
            SystemMessage = systemMessage;
        }

        public AppException(string friendlyMessage, HttpStatusCode httpStatusCode) : base(friendlyMessage)
        {
            FriendlyMessage = friendlyMessage;
            HttpStatusCode = httpStatusCode;
        }

        public static void ThrowInternalServerError(string message)
        {
            throw new AppException(message, HttpStatusCode.InternalServerError);
        }

        public static void ThrowBadRequest(string message)
        {
            throw new AppException(message, HttpStatusCode.BadRequest);
        }
    }
}
