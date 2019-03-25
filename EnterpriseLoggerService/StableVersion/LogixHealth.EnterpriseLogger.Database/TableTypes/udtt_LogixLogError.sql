CREATE TYPE [Logging].[udtt_LogixLogError] AS TABLE
(
    [ApplicationName] VARCHAR(75) NULL, 
    [ConnectLoginID] VARCHAR(256) NULL, 
    [ErrorMessage] VARCHAR(500) NULL, 
    [StackTrace] VARCHAR(4000) NULL,
	[HelpLink] VARCHAR(1000) NULL,
	[HResult] INT NULL,
	[Source] VARCHAR(1000) NULL
)