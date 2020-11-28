--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Updates a row on the store grades table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_UPDATE]
	@HN_RID int,
	@GRADE_CODE varchar(20),
	@BOUNDARY int,
	@WOS_INDEX float
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE STORE_GRADES WITH (ROWLOCK) 
	SET 
		GRADE_CODE = @GRADE_CODE,
		WOS_INDEX = @WOS_INDEX
	WHERE HN_RID = @HN_RID 
	AND BOUNDARY = @BOUNDARY

	SELECT @@ROWCOUNT;

END
GO
