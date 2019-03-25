ALTER TABLE [Logging].[UserEventAdditionalInfo]
	ADD CONSTRAINT [FK_UserEventLog_UserEventAdditionalInfo_UserEventLogID]
	FOREIGN KEY (UserEventLogID)
	REFERENCES [Logging].[UserEventLog] (UserEventLogID)

