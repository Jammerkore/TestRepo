--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Inserts capacity fields for the overrride method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_CAPACITY_INSERT]
	@METHOD_RID int, 
	@SGL_RID int, 
	@EXCEED_CAPACITY char, 
	@EXCEED_BY float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_OVERRIDE_CAPACITY 
		(
		METHOD_RID, 
		SGL_RID, 
		EXCEED_CAPACITY, 
		EXCEED_BY
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@EXCEED_CAPACITY, 
		@EXCEED_BY
		)

	SELECT @@ROWCOUNT;
END
GO