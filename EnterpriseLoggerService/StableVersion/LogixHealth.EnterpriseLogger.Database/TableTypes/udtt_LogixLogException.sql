CREATE TYPE [Logging].[udtt_LogixLogException] AS TABLE
(
    [ClientIPAddress] VARCHAR(20) NULL, 
    [ClientMachine] VARCHAR(60) NULL, 
	[ClientOS] VARCHAR(100) NULL,
	[ClientBrowser] VARCHAR(100) NULL,
    [ApplicationPool] VARCHAR(75) NULL,
	[RequestType] VARCHAR(10) NULL,
	[LogonUserIdentityName] VARCHAR(75) NULL,
	[ApplicationPath] VARCHAR(1000) NULL,
	[RequestUrl] VARCHAR(1000) NULL,
	[UserAgent] VARCHAR(1000) NULL,
	[AppMemEstimatedCPUTime] VARCHAR(10) NULL,
	[AppMemEstimatedMemoryUsage] VARCHAR(10) NULL,
	[AppMemRequestsInAppQueue] VARCHAR(10) NULL,
	[SvrMemCPUUsage] VARCHAR(10) NULL,
	[SvrMemAvailableMemory] VARCHAR(10) NULL,
	[SvrMemAppPoolCPUUsage] VARCHAR(10) NULL,
	[SvrMemAppPoolMemoryUsage] VARCHAR(10) NULL,
	[SyncID] UNIQUEIDENTIFIER NULL,
	[IsLoggedFromMsmq] BIT NULL
)
