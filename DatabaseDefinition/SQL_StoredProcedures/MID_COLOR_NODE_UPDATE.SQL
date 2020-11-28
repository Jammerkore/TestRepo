--dv =============================================
--dv Create date:	10/21/2013
--dv Description:	Updates a row in the color node table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COLOR_NODE_UPDATE]
	@HN_RID int,
	@COLOR_CODE_RID int,
	@COLOR_DESCRIPTION varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE COLOR_NODE WITH (ROWLOCK) 
	SET 
		COLOR_CODE_RID = @COLOR_CODE_RID,
        COLOR_DESCRIPTION = @COLOR_DESCRIPTION
    WHERE HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO
