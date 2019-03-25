namespace LogixHealth.EnterpriseLogger.Services.DataContracts
{
    [System.Runtime.Serialization.DataContractAttribute(Name = "LogixLogException")]
    public class LogixLogChangeDataCapture
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
        /// Name of the Web Page / Table 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ContainerName { get; set; }

        /// <summary>
        /// Name of the Column / Web Page Field
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FieldName { get; set; }

        /// <summary>
        /// Original Data Row Number or Indentifier
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RowNumber { get; set; }

        /// <summary>
        /// Original Data - Before change
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OriginalData { get; set; }

        /// <summary>
        /// New Data - After change
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NewData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public AdditionalCdcDetail[] AdditionalInfo { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid SyncID { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsLoggedFromMsmq { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute(Order = 9999)]
        public System.DateTime LogDateTime { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute(Name = "AdditionalCdcDetail")]
    public class AdditionalCdcDetail
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FieldName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FieldValue { get; set; }
    }
}
