namespace LogixHealth.EnterpriseLogger.Services.DataContracts
{
    /// <summary>
    /// This contract will be exposed for logging Fault Exceptions thrown on WCF Services
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(Name = "LogixLogFaultException")]
    public class LogixLogFaultException
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid FaultID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public LogixLogException ErrorInfo { get; set; }
    }
}
