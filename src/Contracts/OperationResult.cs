using System;
using System.Collections.Generic;
using System.Text;

namespace CommunAxiom.Accounts.Contracts
{
    public class OperationResult
    {
        public const string ERR_NOTFOUND = "NOTFOUND";
        public const string ERR_NULLREF = "NULLREF";
        public const string ERR_INVALID = "INVALID";
        public bool IsError { get; set; }
        public string ErrorCode { get; set; }
        public string[] Fields { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

    }

    public class OperationResult<TResult>: OperationResult
    {
        public TResult Result { get; set; }
    }
}
