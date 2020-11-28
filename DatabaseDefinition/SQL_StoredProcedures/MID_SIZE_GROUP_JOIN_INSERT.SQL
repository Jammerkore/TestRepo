--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Inserts a size group join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_JOIN_INSERT]
	@SIZE_GROUP_RID int,
	@SIZE_CODE_RID int,
	@SEQ int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SIZE_GROUP_JOIN
		(
		SIZE_GROUP_RID, 
		SIZE_CODE_RID, 
		SEQ 
		)
	VALUES
		(
		@SIZE_GROUP_RID, 
		@SIZE_CODE_RID, 
		@SEQ 
		)

	SELECT @@ROWCOUNT;
END
GO
