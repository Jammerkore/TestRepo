--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the audit OTS forecast set
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_OTS_FORECAST_SET_INSERT]
	@AUDIT_FORECAST_RID int, 
	@SGL_RID int, 
	@SET_NAME varchar(50), 
	@FORECAST_METHOD_TYPE varchar(30),
	@STOCK_MIN_MAX char
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO AUDIT_OTS_FORECAST_SET
		(
		AUDIT_FORECAST_RID, 
		SGL_RID, 
		SET_NAME, 
		FORECAST_METHOD_TYPE, 
		STOCK_MIN_MAX
		)
	VALUES 
		(
		@AUDIT_FORECAST_RID, 
		@SGL_RID, 
		@SET_NAME, 
		@FORECAST_METHOD_TYPE, 
		@STOCK_MIN_MAX
		)

	SELECT @@ROWCOUNT;

END
GO
