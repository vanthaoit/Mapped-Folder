using LogixHealth.EnterpriseLibrary.DataAccess;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;

namespace LogixHealth.EnterpriseLogger.Services
{
    public static class MsmqHelper
    {
        private const string USP_InsertExceptionLogs_ParamName_Udtt_LogixLogException = "udtt_LogixLogException";
        private const string USP_InsertExceptionLogs_ParamName_Udtt_LogixLogError = "udtt_LogixLogError";
        private const string Udtt_Name_LogixLogException = "[Logging].[udtt_LogixLogException]";
        private const string Udtt_Name_LogixLogError = "[Logging].[udtt_LogixLogError]";
        private const string USP_InsertExceptionLogs_Name = "[Logging].[usp_InsertExceptionLogs]";
        private const string CanWriteToQueue_VALUE_YES = "YES";

        private static readonly string CanWriteToQueue = ConfigurationManager.AppSettings["WriteToMSMQ"];
        private static readonly string MsmqExceptionLog = ConfigurationManager.AppSettings["ExceptionLogQueue"];
        private const string MsmqExceptionLogTarget = "ExceptionLog";
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["LogixLoggerDatabase"].ConnectionString;

        public static void WriteExceptionLogs(DataContracts.LogixLogException payLoad)
        {
            if (CanWriteToQueue.ToUpper() == CanWriteToQueue_VALUE_YES)
                WriteExceptionToQueue(payLoad);
            else
                WriteExceptionToDatabase(payLoad);
        }

        public static void WriteChangeDataCaptureLogToQueue(DataContracts.LogixLogChangeDataCapture model)
        {
            throw new NotImplementedException();
        }

        private static void WriteExceptionToQueue(DataContracts.LogixLogException payLoad)
        {
            using (MessageQueue queue = new MessageQueue(MsmqExceptionLog.Replace("MACHINENAME", Environment.MachineName)))
            {
                payLoad.IsLoggedFromMsmq = true;

                Message message = new Message(payLoad)
                {
                    Formatter = new XmlMessageFormatter
                    (
                        new List<Type>
                        {
                            payLoad.GetType()
                        }.ToArray()
                    )
                };

                queue.Send(message, MsmqExceptionLogTarget);
            }
        }

        private static bool WriteExceptionToDatabase(DataContracts.LogixLogException payLoad)
        {
            using (IQueryRepository<DataContracts.LogixLogException> repository = new QueryRepository<DataContracts.LogixLogException>(ConnectionString))
            {
                payLoad.IsLoggedFromMsmq = false;

                ICollection<NameValueType> nameValuePair = new System.Collections.ObjectModel.Collection<NameValueType>
                {
                    new NameValueType
                    {
                        Name = USP_InsertExceptionLogs_ParamName_Udtt_LogixLogException,
                        Value = payLoad,
                        TableTypeName = Udtt_Name_LogixLogException,
                        IsTableType = true
                    },
                    new NameValueType
                    {
                        Name = USP_InsertExceptionLogs_ParamName_Udtt_LogixLogError,
                        Value = payLoad.UserErrorInfo,
                        TableTypeName = Udtt_Name_LogixLogError,
                        IsTableType = true
                    }
                };

                return repository.InsertOrUpdate(USP_InsertExceptionLogs_Name, nameValuePair);
            }
        }

        //private static string SerializeToXml(dynamic payload)
        //{
        //    System.Xml.Serialization.XmlSerializer serializer = null;
        //    string xml = string.Empty;

        //    if (payload is DataContracts.ApplicationPerformanceCounter)
        //    {
        //        serializer = new System.Xml.Serialization.XmlSerializer(payload.GetType());
        //    }
        //    else if (payload is DataContracts.RequestDetail)
        //    {
        //        serializer = new System.Xml.Serialization.XmlSerializer(payload.GetType());
        //    }
        //    else if (payload is DataContracts.ServerPerformanceCounter)
        //    {
        //        serializer = new System.Xml.Serialization.XmlSerializer(payload.GetType());
        //    }
        //    else if (payload is DataContracts.AdditionalCdcDetail)
        //    {
        //        serializer = new System.Xml.Serialization.XmlSerializer(payload.GetType());
        //    }
        //    else if (payload is DataContracts.AdditionalEventDetail)
        //    {
        //        serializer = new System.Xml.Serialization.XmlSerializer(payload.GetType());
        //    }

        //    if (serializer != null)
        //    {
        //        using (StringWriter stringWriter = new StringWriter())
        //        {
        //            using (XmlWriter writer = XmlWriter.Create(stringWriter))
        //            {
        //                serializer.Serialize(writer, payload);
        //                xml = stringWriter.ToString();
        //            }
        //        }
        //    }

        //    return xml;
        //}
    }
}