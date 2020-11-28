--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the audit OTS forecast set basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_OTS_FORECAST_SET_BASIS_INSERT]
	@AUDIT_FORECAST_RID int, 
	@SGL_RID int, 
	@BASIS_SEQUENCE int, 
	@BASIS_HN_RID int, 
	@BASIS_HN_TEXT varchar(150), 
	@BASIS_FV_RID int, 
	@BASIS_TIME_PERIOD varchar(50),
    @BASIS_WEIGHT float, 
	@BASIS_TYPE_SORT_CODE int, 
	@BASIS_TYPE varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO AUDIT_OTS_FORECAST_SET_BASIS
		(
		AUDIT_FORECAST_RID, 
		SGL_RID, 
		BASIS_SEQUENCE, 
		BASIS_HN_RID, 
		BASIS_HN_TEXT,
        BASIS_FV_RID, 
		BASIS_TIME_PERIOD, 
		BASIS_WEIGHT, 
		BASIS_TYPE_SORT_CODE, 
		BASIS_TYPE
		)
	VALUES 
		(
		@AUDIT_FORECAST_RID, 
		@SGL_RID, 
		@BASIS_SEQUENCE, 
		@BASIS_HN_RID, 
		@BASIS_HN_TEXT, 
		@BASIS_FV_RID, 
		@BASIS_TIME_PERIOD,
        @BASIS_WEIGHT, 
		@BASIS_TYPE_SORT_CODE, 
		@BASIS_TYPE
		)

	SELECT @@ROWCOUNT;

END
GO
