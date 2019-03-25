CREATE PROCEDURE [Logging].[usp_SelectCdcByAppName]
	@appName VARCHAR(75)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT
				 cdc.ChangeDataCaptureLogID
				,cdc.ApplicationName
				,cdc.ConnectLoginID
				,cdc.ContainerName
				,cdc.FieldName
				,cdc.OriginalData
				,cdc.NewData
				,cdc.OriginalData
				,cdc.ReferenceRowNumber
				,cdcAd.FieldName AS CdcAdditionalDetailFieldName
				,cdcAd.FieldValue
	FROM		Logging.ChangeDataCaptureLog cdc WITH (NOLOCK)
	LEFT JOIN	Logging.CdcAdditionalInfo cdcAd WITH (NOLOCK) ON cdc.ChangeDataCaptureLogID = cdcAd.ChangeDataCaptureLogID
	WHERE		cdc.ApplicationName = @appName

END
