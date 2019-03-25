namespace LogixHealth.EnterpriseLogger.Services.DataContracts
{
    /// <summary>
    /// This contract will be exposed to helper for logging additional information
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(Name = "LogixLogException")]
    public class LogixLogException
    {
        [System.Runtime.Serialization.DataMemberAttribute(Order = 9999)]
        public int LogixLogExceptionID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientIPAddress { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientMachine { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientOS { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientBrowser { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationPool { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RequestType { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LogonUserIdentityName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationPath { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RequestUrl { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserAgent { get; set; }

        // estimated CPU time for your specific application
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppMemEstimatedCPUTime { get; set; }

        // estimated memory usage for your specific application
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppMemEstimatedMemoryUsage { get; set; }

        // Requests in Application Queue
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AppMemRequestsInAppQueue { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SvrMemCPUUsage { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SvrMemAvailableMemory { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SvrMemAppPoolCPUUsage { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SvrMemAppPoolMemoryUsage { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SyncID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsLoggedFromMsmq { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9999)]
        public System.DateTime LogDateTime { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9999)]
        public LogixLogError UserErrorInfo { get; set; }

        public LogixLogException()
        {
            UserErrorInfo = new LogixLogError();
        }
    }
}
