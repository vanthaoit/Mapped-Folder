CREATE TABLE [Logging].[UserEventLog]
(
	[UserEventLogID] INT NOT NULL PRIMARY KEY,
	[ApplicationName] VARCHAR(75) NOT NULL,
	[ConnectLoginID] VARCHAR(256) NOT NULL,
	[EventName] VARCHAR(100) NOT NULL,
	[IsLoggedFromMsmq] BIT NOT NULL DEFAULT 0,
	[LogDateTime] DATETIME NOT NULL
)
