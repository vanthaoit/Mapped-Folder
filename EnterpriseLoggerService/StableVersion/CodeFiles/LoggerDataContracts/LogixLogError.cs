namespace LogixHealth.EnterpriseLogger.Services.DataContracts
{
    /// <summary>
    /// This contract will be exposed to consumer for logging 
    ///     1. Errors
    ///     2. Debug Info
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(Name = "LogixLogError")]
    public class LogixLogError
    {
        [System.Runtime.Serialization.DataMemberAttribute(Order = 9999)]
        public int LogixLogErrorID { get; set; }

        /// <summary>
        /// Name of the application
        ///     Example -
        ///         LogixCodify
        ///         LogixLearning
        ///         LogixAppointments
        ///         etc.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Detail error message including stacktrace
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ConnectLoginID { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Detail error message including stacktrace
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StackTrace { get; set; }

        /// <summary>
        /// Link from which solution can be find/searched
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HelpLink { get; set; }

        /// <summary>
        /// Actual exception number from microsoft
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int HResult { get; set; }

        /// <summary>
        /// Name of the object from which this exception is throwing
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Source { get; set; }
    }
}
