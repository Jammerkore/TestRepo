--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Updates the description on the hierarchy characteristic table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_UPDATE]
	@HC_RID int,
	@HC_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE HIER_CHAR WITH (ROWLOCK)
	SET 
		HC_ID = @HC_ID
	WHERE HC_RID = @HC_RID

	SELECT @@ROWCOUNT;

END
GO
