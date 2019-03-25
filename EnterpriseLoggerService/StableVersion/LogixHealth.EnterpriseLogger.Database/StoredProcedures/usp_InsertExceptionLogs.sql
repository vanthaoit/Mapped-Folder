CREATE PROCEDURE [Logging].[usp_InsertExceptionLogs]
	@udtt_LogixLogException [Logging].[udtt_LogixLogException] READONLY,
	@udtt_LogixLogError [Logging].[udtt_LogixLogError] READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	BEGIN TRY
		IF @@TRANCOUNT < 1
			BEGIN TRANSACTION
				DECLARE @ApplicationName VARCHAR(75) = NULL
				DECLARE @ConnectLoginID VARCHAR(256) = NULL
				DECLARE @ErrorMessage VARCHAR(500) = NULL
				DECLARE @StackTrace VARCHAR(4000) = NULL
				DECLARE @HelpLink VARCHAR(1000) = NULL
				DECLARE @HResult INT = NULL
				DECLARE @Source VARCHAR(1000) = NULL

				SELECT 
						@ApplicationName = [ApplicationName], @ConnectLoginID = [ConnectLoginID], @ErrorMessage = [ErrorMessage], 
						@StackTrace = [StackTrace], @HelpLink = [HelpLink], @HResult = [HResult], @Source = [Source]
				FROM 
						@udtt_LogixLogError

				INSERT INTO [Logging].[LogixLogException]
				(
					[ApplicationName], [ConnectLoginID], [ErrorMessage], [StackTrace],
					[HelpLink], [HResult], [Source], 
					[ClientIPAddress], [ClientMachine], [ClientOS], [ClientBrowser], [ApplicationPool], 
					[RequestType], [LogonUserIdentityName], [ApplicationPath], 
					[RequestUrl], [UserAgent], 
					[AppMemEstimatedCPUTime], [AppMemEstimatedMemoryUsage], [AppMemRequestsInAppQueue], 
					[SvrMemCPUUsage], [SvrMemAvailableMemory], [SvrMemAppPoolCPUUsage], [SvrMemAppPoolMemoryUsage],
					[SyncID], [IsLoggedFromMsmq], [LogDateTime]
				)
				SELECT	
					@ApplicationName AS ApplicationName, @ConnectLoginID AS ConnectLoginID, @ErrorMessage AS ErrorMessage, @StackTrace AS StackTrace, 
					@HelpLink AS HelpLink, @HResult AS HResult, @Source AS 'Source', 
					[ClientIPAddress], [ClientMachine], [ClientOS], [ClientBrowser], [ApplicationPool], 
					[RequestType], [LogonUserIdentityName], [ApplicationPath], 
					[RequestUrl], [UserAgent], 
					[AppMemEstimatedCPUTime], [AppMemEstimatedMemoryUsage], [AppMemRequestsInAppQueue], 
					[SvrMemCPUUsage], [SvrMemAvailableMemory], [SvrMemAppPoolCPUUsage], [SvrMemAppPoolMemoryUsage],
					[SyncID], [IsLoggedFromMsmq], GETDATE() AS 'LogDateTime'
				FROM
					@udtt_LogixLogException
		IF @@TRANCOUNT > 0
			COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		THROW
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
	END CATCH
END
