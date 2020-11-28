--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the modify sales table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_MODIFY_SALES_INSERT]
	@AUDIT_FORECAST_RID int, 
	@FILTER_NAME varchar(50), 
	@AVERAGE_BY varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO AUDIT_MODIFY_SALES
		(
		AUDIT_FORECAST_RID, 
		FILTER_NAME, 
		AVERAGE_BY
		)
	VALUES 
		(
		@AUDIT_FORECAST_RID, 
		@FILTER_NAME, 
		@AVERAGE_BY
		)

	SELECT @@ROWCOUNT;

END
GO
