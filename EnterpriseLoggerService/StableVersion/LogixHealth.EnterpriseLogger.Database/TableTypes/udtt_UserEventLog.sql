CREATE TYPE [Logging].[udtt_UserEventLog] AS TABLE
(
	[UserEventLogID] INT NULL,
	[ApplicationName] VARCHAR(75) NULL,
	[ConnectLoginID] VARCHAR(256) NULL,
	[EventName] VARCHAR(100) NULL,
	[IsLoggedFromMsmq] BIT NULL DEFAULT 0,
	[LogDateTime] DATETIME NULL,
	[UserEventAdditionalDetailID] INT NULL,
	[AdditionalDetailEventName] VARCHAR(75) NULL,
	[EventValue] VARCHAR(500) NULL
)
