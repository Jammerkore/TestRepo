--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Deletes a header pack rounding row based on the header rid passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_PACK_ROUNDING_DELETE]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE HEADER_PACK_ROUNDING WITH (ROWLOCK) 
	WHERE HDR_RID=@HDR_RID

	SELECT @@ROWCOUNT;

END
GO
