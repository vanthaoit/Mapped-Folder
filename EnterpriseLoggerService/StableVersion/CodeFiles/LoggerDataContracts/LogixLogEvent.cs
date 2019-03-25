namespace LogixHealth.EnterpriseLogger.Services.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(Name = "LogixLogEvent")]
    public class LogixLogEvent
    {
        /// <summary>
        /// Name of the application
        ///     Example -
        ///         LogixCodify
        ///         LogixLearning
        ///         LogixAppointments
        ///         etc.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Application { get; set; }

        /// <summary>
        /// User Login ID - Connect Email Address
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ConnectLoginID { get; set; }

        /// <summary>
        /// Name of the Action ex: ViewChart, Save Chart, Login, Logout
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EventName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AdditionalEventDetail[] AdditionalInfo { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SyncID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsLoggedFromMsmq { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9999)]
        public System.DateTime LogDateTime { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "AdditionalEventDetail")]
    public class AdditionalEventDetail
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value { get; set; }
    }
}
