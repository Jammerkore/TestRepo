--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Updates Forecast Model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_UPDATE]
		@SIZE_CURVE_RID int,
		@SIZE_CURVE_NAME varchar(250)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SIZE_CURVE 
	SET 
		SIZE_CURVE_NAME = @SIZE_CURVE_NAME
	WHERE
        SIZE_CURVE_RID = @SIZE_CURVE_RID

	SELECT @@ROWCOUNT
END
GO