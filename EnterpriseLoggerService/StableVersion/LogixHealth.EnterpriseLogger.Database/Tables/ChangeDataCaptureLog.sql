CREATE TABLE [Logging].[ChangeDataCaptureLog]
(
	[ChangeDataCaptureLogID] INT NOT NULL PRIMARY KEY,
	[ApplicationName] VARCHAR(75) NOT NULL,
	[ConnectLoginID] VARCHAR(256) NOT NULL,
	[ContainerName] VARCHAR(75) NOT NULL,
	[FieldName] VARCHAR(75) NOT NULL,
	[ReferenceRowNumber] INT NOT NULL,
	[OriginalData] VARCHAR(500) NOT NULL,
	[NewData] VARCHAR(500) NOT NULL,
	[IsLoggedFromMsmq] BIT NOT NULL DEFAULT 0,
	[LogDateTime] DATETIME NOT NULL
)
