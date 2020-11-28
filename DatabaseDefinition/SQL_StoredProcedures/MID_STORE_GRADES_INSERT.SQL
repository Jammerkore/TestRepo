--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row into the store grades table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GRADES_INSERT]
	@HN_RID int,
	@GRADE_CODE varchar(20),
	@BOUNDARY int,
	@WOS_INDEX float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STORE_GRADES(HN_RID, GRADE_CODE, BOUNDARY, WOS_INDEX)
	VALUES (@HN_RID, @GRADE_CODE, @BOUNDARY, @WOS_INDEX)

	SELECT @@ROWCOUNT;

END
GO
