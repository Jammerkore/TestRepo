--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Updates a description on the hierarchy characteristic group table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_GROUP_UPDATE]
	@HCG_RID int,
	@HCG_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HIER_CHAR_GROUP WITH (ROWLOCK) 
	SET HCG_ID = @HCG_ID
	WHERE HCG_RID=@HCG_RID

	SELECT @@ROWCOUNT;

END
GO
