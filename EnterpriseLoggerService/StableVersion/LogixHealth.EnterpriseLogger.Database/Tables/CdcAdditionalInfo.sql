CREATE TABLE [Logging].[CdcAdditionalInfo]
(
	[CdcAdditionalInfoID] INT NOT NULL PRIMARY KEY,
    [ChangeDataCaptureLogID] INT NOT NULL, 
    [FieldName] VARCHAR(75) NOT NULL, 
    [FieldValue] VARCHAR(500) NOT NULL
)
