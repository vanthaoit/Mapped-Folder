namespace LogixHealth.EnterpriseLogger.Services.ServiceContracts
{
    /// <summary>
    /// ILogixLogMessageService interface used for logging/writing logs
    /// </summary>
    [System.ServiceModel.ServiceContractAttribute()]
    public interface ILogixLoggerService
    {
        /// <summary>
        /// This method used for reading Exception Logs
        /// </summary>
        /// <param name="payLoad">Exception data contract which contains exception details</param>
        [System.ServiceModel.OperationContractAttribute()]
        [System.ServiceModel.FaultContractAttribute(typeof(LogixHealth.EnterpriseLogger.Services.DataContracts.LogixLogFaultException))]
        void LogExceptions(LogixHealth.EnterpriseLogger.Services.DataContracts.LogixLogException payLoad);

        /// <summary>
        /// This method used for reading Exception Logs
        /// </summary>
        /// <param name="payLoad"></param>
        [System.ServiceModel.OperationContractAttribute()]
        [System.ServiceModel.FaultContractAttribute(typeof(LogixHealth.EnterpriseLogger.Services.DataContracts.LogixLogFaultException))]
        void LogChageDataCapture(LogixHealth.EnterpriseLogger.Services.DataContracts.LogixLogChangeDataCapture payLoad);

        /// <summary>
        /// This method used for reading Exception Logs
        /// </summary>
        /// <param name="payLoad"></param>
        [System.ServiceModel.OperationContractAttribute()]
        [System.ServiceModel.FaultContractAttribute(typeof(LogixHealth.EnterpriseLogger.Services.DataContracts.LogixLogFaultException))]
        void LogEvents(LogixHealth.EnterpriseLogger.Services.DataContracts.LogixLogEvent payLoad);
    }
}
