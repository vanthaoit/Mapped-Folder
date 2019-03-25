namespace LogixHealth.EnterpriseLogger.Extensions
{
    using LogixHealth.EnterpriseLogger.Services.DataContracts;

    internal sealed class LogixLogger : ILogixLogger
    {
        private QueueLogger _queueLogger;
        private DatabaseLogger _databaseLogger;
        private NLog.Logger _fileLogger;

        public LogixLogger(NLog.Logger logger, Configuration configuration)
        {
            _fileLogger = logger;
            _databaseLogger = new DatabaseLogger(configuration.ServiceBinding, configuration.ServiceAddress);
            _queueLogger = new QueueLogger();

            Configuration = configuration;
        }

        public Configuration Configuration { get; set; }

        void ILogixExceptionLogger.LogException(LogixLogError payLoad)
        {
            LogixLogException logixLogException = new LogixLogException
            {
                UserErrorInfo = new LogixLogError
                {
                    ApplicationName = payLoad.ApplicationName,
                    ConnectLoginID = payLoad.ConnectLoginID,
                    HelpLink = payLoad.HelpLink,
                    HResult = payLoad.HResult,
                    ErrorMessage = payLoad.ErrorMessage,
                    Source = payLoad.Source,
                    StackTrace = payLoad.StackTrace
                },

                ApplicationPath = payLoad.Source,
                ApplicationPool = null,
                ClientIPAddress = System.Net.Dns.GetHostAddresses(System.Environment.MachineName).ToString(),
                ClientMachine = System.Environment.MachineName,
                UserAgent = null,
                LogonUserIdentityName = payLoad.ConnectLoginID,
                RequestType = null,
                RequestUrl = null,

                AppMemEstimatedCPUTime = null,
                AppMemEstimatedMemoryUsage = null,
                AppMemRequestsInAppQueue = null,

                SvrMemAppPoolCPUUsage = null,
                SvrMemAppPoolMemoryUsage = null,
                SvrMemAvailableMemory = null,
                SvrMemCPUUsage = null,

                SyncID = System.Guid.NewGuid(),
                IsLoggedFromMsmq = false, // Default value
                LogDateTime = System.DateTime.Now, // Default value
            };

            try
            {
                if (Configuration.UseMessageQueue.ToUpper() == "YES")
                {
                    logixLogException.IsLoggedFromMsmq = true;
                    _queueLogger.LogExceptions(logixLogException);
                }
                else
                {
                    _databaseLogger.LogExceptions(logixLogException);
                }
            }
            catch (System.ServiceModel.FaultException<LogixLogFaultException> faultException)
            {
                logixLogException = faultException.Detail.ErrorInfo;
            }
            catch (System.Exception generalException)
            {
                logixLogException.UserErrorInfo.StackTrace += ExtractExceptionDetail(generalException);
            }

            WriteExceptions(logixLogException);
        }

        void ILogixExceptionLogger.LogException(LogixLogError payLoad, dynamic httpContext)
        {
            Microsoft.AspNetCore.Http.HttpContext context = httpContext as Microsoft.AspNetCore.Http.HttpContext;

            LogixLogException logixLogException = new LogixLogException
            {
                UserErrorInfo = new LogixLogError
                {
                    ApplicationName = payLoad.ApplicationName,
                    ConnectLoginID = payLoad.ConnectLoginID,
                    HelpLink = payLoad.HelpLink,
                    HResult = payLoad.HResult,
                    ErrorMessage = payLoad.ErrorMessage,
                    Source = payLoad.Source,
                    StackTrace = payLoad.StackTrace
                },

                ApplicationPath = context.Request.PathBase.Value,
                ApplicationPool = null,
                ClientIPAddress = context.Connection.RemoteIpAddress.ToString(),
                ClientMachine = System.Net.Dns.GetHostEntry(context.Connection.RemoteIpAddress).HostName.ToString(),
                UserAgent = context.Request.Headers["User-Agent"],
                LogonUserIdentityName = context.User.Identity.Name,
                RequestType = context.Request.Method,
                RequestUrl = context.Request.PathBase + "/" + context.Request.QueryString,

                AppMemEstimatedCPUTime = null,
                AppMemEstimatedMemoryUsage = null,
                AppMemRequestsInAppQueue = null,

                SvrMemAppPoolCPUUsage = null,
                SvrMemAppPoolMemoryUsage = null,
                SvrMemAvailableMemory = null,
                SvrMemCPUUsage = null,

                SyncID = System.Guid.NewGuid(),
                IsLoggedFromMsmq = false, // Default value
                LogDateTime = System.DateTime.Now, // Default value
            };

            try
            {
                ClientOsBrowser browser = new ClientOsBrowser(context.Request.Headers["User-Agent"]);
                if (browser != null)
                {
                    logixLogException.ClientOS = browser.OSName + " " + browser.OSVersion;
                    logixLogException.ClientBrowser = browser.BrowserName + " " + browser.BrowserMajorVersion + " (" + browser.BrowserVersion + ")";
                }
            }
            catch (System.Exception)
            {
                // DO NOTHING
            }

            try
            {
                if (Configuration.UseMessageQueue.ToUpper() == "YES")
                {
                    logixLogException.IsLoggedFromMsmq = true;
                    _queueLogger.LogExceptions(logixLogException);
                }
                else
                {
                    _databaseLogger.LogExceptions(logixLogException);
                }
            }
            catch (System.ServiceModel.FaultException<LogixLogFaultException> faultException)
            {
                logixLogException = faultException.Detail.ErrorInfo;
            }
            catch (System.Exception generalException)
            {
                logixLogException.UserErrorInfo.StackTrace += ExtractExceptionDetail(generalException);
            }

            WriteExceptions(logixLogException);
        }

        void ILogixChangeDataCaptureLogger.LogChangeDataCapture(LogixLogChangeDataCapture payLoad)
        {
            LogixLogChangeDataCapture logixLogCdc = new LogixLogChangeDataCapture
            {
                AdditionalInfo = payLoad.AdditionalInfo,
                Application = payLoad.Application,
                ConnectLoginID = payLoad.ConnectLoginID,
                ContainerName = payLoad.ContainerName,
                FieldName = payLoad.FieldName,
                NewData = payLoad.NewData,
                OriginalData = payLoad.OriginalData,
                RowNumber = payLoad.RowNumber,

                SyncID = System.Guid.NewGuid(),
                IsLoggedFromMsmq = false, // Default value
                LogDateTime = System.DateTime.Now, // Default value
            };

            try
            {
                if (Configuration.UseMessageQueue.ToUpper() == "YES")
                {
                    logixLogCdc.IsLoggedFromMsmq = true;
                    _queueLogger.LogChangeDataCapture(logixLogCdc);
                }
                else
                {
                    _databaseLogger.LogChangeDataCapture(logixLogCdc);
                }
            }
            catch (System.ServiceModel.FaultException<LogixLogFaultException> faultException)
            {
                LogException
                    (
                        new LogixLogError
                        {
                            ApplicationName = payLoad.Application,
                            ConnectLoginID = payLoad.ConnectLoginID,
                            ErrorMessage = faultException.Message,
                            Source = "Error occured in ILogixChangeDataCaptureLogger.LogChangeDataCapture(LogixLogChangeDataCapture payLoad) > _databaseLogger.LogChangeDataCapture(logixLogCdc)",
                            StackTrace = ExtractExceptionDetail(faultException)
                        },
                        faultException
                    );
            }
            catch (System.Exception generalException)
            {
                LogException
                    (
                        new LogixLogError
                        {
                            ApplicationName = payLoad.Application,
                            ConnectLoginID = payLoad.ConnectLoginID,
                            ErrorMessage = generalException.Message,
                            Source = "Error occured in ILogixChangeDataCaptureLogger.LogChangeDataCapture(LogixLogChangeDataCapture payLoad) > _databaseLogger.LogChangeDataCapture(logixLogCdc)",
                            StackTrace = ExtractExceptionDetail(generalException)
                        },
                        generalException
                    );
            }

            WriteChangeDataCapture(logixLogCdc);
        }

        void ILogixUserEventLogger.LogEvents(LogixLogEvent payLoad)
        {
            LogixLogEvent logixLogEvent = new LogixLogEvent
            {
                AdditionalInfo = payLoad.AdditionalInfo,
                Application = payLoad.Application,
                ConnectLoginID = payLoad.ConnectLoginID,
                EventName = payLoad.EventName,

                SyncID = System.Guid.NewGuid(),
                IsLoggedFromMsmq = false, // Default value
                LogDateTime = System.DateTime.Now, // Default value
            };

            try
            {
                if (Configuration.UseMessageQueue.ToUpper() == "YES")
                {
                    logixLogEvent.IsLoggedFromMsmq = true;
                    _queueLogger.LogUserEvents(logixLogEvent);
                }
                else
                {
                    _databaseLogger.LogUserEvents(logixLogEvent);
                }
            }
            catch (System.ServiceModel.FaultException<LogixLogFaultException> faultException)
            {
                LogException
                    (
                        new LogixLogError
                        {
                            ApplicationName = payLoad.Application,
                            ConnectLoginID = payLoad.ConnectLoginID,
                            ErrorMessage = faultException.Message,
                            Source = "Error occured in ILogixUserEventLogger.LogEvents(LogixLogEvent payLoad) > _databaseLogger.LogUserEvents(logixLogEvent)",
                            StackTrace = ExtractExceptionDetail(faultException)
                        },
                        faultException
                    );
            }
            catch (System.Exception generalException)
            {
                LogException
                    (
                        new LogixLogError
                        {
                            ApplicationName = payLoad.Application,
                            ConnectLoginID = payLoad.ConnectLoginID,
                            ErrorMessage = generalException.Message,
                            Source = "Error occured in ILogixUserEventLogger.LogEvents(LogixLogEvent payLoad) > _databaseLogger.LogUserEvents(logixLogEvent)",
                            StackTrace = ExtractExceptionDetail(generalException)
                        },
                        generalException
                    );
            }

            WriteUserEvents(logixLogEvent);
        }

        private void LogException(LogixLogError payLoad, dynamic exception)
        {
            LogixLogException logixLogException = new LogixLogException
            {
                UserErrorInfo = new LogixLogError
                {
                    ApplicationName = payLoad.ApplicationName,
                    ConnectLoginID = payLoad.ConnectLoginID,
                    HelpLink = payLoad.HelpLink,
                    HResult = payLoad.HResult,
                    ErrorMessage = payLoad.ErrorMessage,
                    Source = payLoad.Source,
                    StackTrace = payLoad.StackTrace
                },

                ApplicationPath = payLoad.Source,
                ApplicationPool = null,
                ClientIPAddress = System.Net.Dns.GetHostAddresses(System.Environment.MachineName).ToString(),
                ClientMachine = System.Environment.MachineName,
                UserAgent = null,
                LogonUserIdentityName = payLoad.ConnectLoginID,
                RequestType = null,
                RequestUrl = null,

                AppMemEstimatedCPUTime = null,
                AppMemEstimatedMemoryUsage = null,
                AppMemRequestsInAppQueue = null,

                SvrMemAppPoolCPUUsage = null,
                SvrMemAppPoolMemoryUsage = null,
                SvrMemAvailableMemory = null,
                SvrMemCPUUsage = null,

                SyncID = System.Guid.NewGuid(),
                IsLoggedFromMsmq = false, // Default value
                LogDateTime = System.DateTime.Now, // Default value
            };

            try
            {
                _databaseLogger.LogExceptions(logixLogException);
            }
            catch (System.ServiceModel.FaultException<LogixLogFaultException> faultException)
            {
                logixLogException = faultException.Detail.ErrorInfo;
            }
            catch (System.Exception generalException)
            {
                logixLogException.UserErrorInfo.StackTrace += ExtractExceptionDetail(generalException);
            }

            WriteExceptions(logixLogException);
        }

        private void WriteExceptions(LogixLogException payLoad)
        {
            _fileLogger.Log(NLog.LogLevel.Error, Newtonsoft.Json.JsonConvert.SerializeObject(payLoad));
        }

        private void WriteUserEvents(LogixLogEvent payLoad)
        {
            _fileLogger.Log(NLog.LogLevel.Info, Newtonsoft.Json.JsonConvert.SerializeObject(payLoad));
        }

        private void WriteChangeDataCapture(LogixLogChangeDataCapture payLoad)
        {
            _fileLogger.Log(NLog.LogLevel.Info, Newtonsoft.Json.JsonConvert.SerializeObject(payLoad));
        }

        private static string ExtractExceptionDetail(dynamic exception)
        {
            string stackTrace = string.Empty;

            if (exception != null)
            {
                dynamic innerException = exception;
                while (innerException != null)
                {
                    stackTrace += innerException.Message + System.Environment.NewLine + innerException.StackTrace;
                    innerException = innerException.InnerException;
                }
            }

            string error = System.Environment.NewLine + " - Detailed Exception - " + System.Environment.NewLine;

            if (stackTrace.Length > 0)
                error += stackTrace;
            else
                error = stackTrace;

            return error;
        }
    }
}