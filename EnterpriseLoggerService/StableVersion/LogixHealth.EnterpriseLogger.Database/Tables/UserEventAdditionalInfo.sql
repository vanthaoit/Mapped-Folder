CREATE TABLE [Logging].[UserEventAdditionalInfo]
(
	[UserEventAdditionalInfoID] INT NOT NULL PRIMARY KEY,
	[UserEventLogID] INT NOT NULL,
	[EventName] VARCHAR(75) NOT NULL,
	[EventValue] VARCHAR(500) NOT NULL
)
