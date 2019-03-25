CREATE PROCEDURE [Logging].[usp_InsertUserEventLogs]
	@userEventLog [Logging].[udtt_UserEventLog] READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	DECLARE @identityValue INT = 0

	-- 1st : insert into UserEventLog Table
	INSERT INTO Logging.UserEventLog
	(
		ApplicationName, ConnectLoginID, EventName, IsLoggedFromMsmq, LogDateTime
	)
	SELECT	
		ApplicationName, ConnectLoginID, EventName, IsLoggedFromMsmq, GETDATE() AS LogDateTime
	FROM
		@userEventLog

	-- Next : Get the identity value
	SELECT @identityValue = SCOPE_IDENTITY()

	-- Next : insert additional details
	INSERT INTO Logging.UserEventAdditionalInfo
	(
		EventName, EventValue, UserEventLogID
	)
	SELECT
		EventName, EventValue, @identityValue AS UserEventLogID
	FROM
		@userEventLog

END
