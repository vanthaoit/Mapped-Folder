CREATE TYPE [Logging].[udtt_ChangeDataCaptureLog] AS TABLE
(
	[ChangeDataCaptureLogID] INT NULL,
	[ApplicationName] VARCHAR(75) NULL,
	[ConnectLoginID] VARCHAR(256) NULL,
	[ContainerName] VARCHAR(75) NULL,
	[FieldName] VARCHAR(75) NULL,
	[ReferenceRowNumber] INT NULL,
	[OriginalData] VARCHAR(500) NULL,
	[NewData] VARCHAR(500) NULL,
	[IsLoggedFromMsmq] BIT NULL DEFAULT 0,
	[LogDateTime] DATETIME NULL,
	[CdcAdditionalDetailID] INT NULL,
    [CdcAdditionalDetailFieldName] VARCHAR(75) NULL, 
    [FieldValue] VARCHAR(500) NULL
)
