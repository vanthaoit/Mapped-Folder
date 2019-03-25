CREATE PROCEDURE [Logging].[usp_InsertChangeDataCaptureLogs]
	@cdcLog [Logging].[udtt_ChangeDataCaptureLog] READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @identityValue INT = 0

	-- 1st : insert into UserEventLog Table
	INSERT INTO Logging.ChangeDataCaptureLog
	(
		ApplicationName, ConnectLoginID, ContainerName, FieldName, IsLoggedFromMsmq, LogDateTime,
		NewData, OriginalData, ReferenceRowNumber
	)
	SELECT	
		ApplicationName, ConnectLoginID, ContainerName, FieldName, IsLoggedFromMsmq, GETDATE() AS LogDateTime,
		NewData, OriginalData, ReferenceRowNumber
	FROM
		@cdcLog

	-- Next : Get the identity value
	SELECT @identityValue = SCOPE_IDENTITY()

	-- Next : insert additional details
	INSERT INTO Logging.CdcAdditionalInfo
	(
		ChangeDataCaptureLogID, FieldName, FieldValue
	)
	SELECT
		@identityValue AS ChangeDataCaptureLogID, FieldName, FieldValue
	FROM
		@cdcLog

END
