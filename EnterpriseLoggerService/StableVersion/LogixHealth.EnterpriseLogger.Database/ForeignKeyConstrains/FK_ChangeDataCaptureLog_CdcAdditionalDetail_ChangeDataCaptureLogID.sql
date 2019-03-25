ALTER TABLE [Logging].[CdcAdditionalInfo]
	ADD CONSTRAINT [FK_ChangeDataCaptureLog_CdcAdditionalInfo_ChangeDataCaptureLogID]
	FOREIGN KEY (ChangeDataCaptureLogID)
	REFERENCES [Logging].[ChangeDataCaptureLog] (ChangeDataCaptureLogID)
