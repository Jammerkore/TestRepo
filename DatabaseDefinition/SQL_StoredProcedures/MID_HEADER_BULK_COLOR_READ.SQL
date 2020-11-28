--dv =============================================
--dv Create date:	10/16/2013
--dv Modified date: 8/14/2014
--dv Description:	Gets header bulk color data based on the header rid passed in
--dv History:		TT#664 - MD - DOConnell - Placeholder colors display in reverse order on the Content Tab
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	SELECT HBC.* 
	FROM HEADER_BULK_COLOR HBC
	WHERE HBC.HDR_RID = @HDR_RID
	ORDER BY HBC.COLOR_CODE_RID, HBC.SEQ
	
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED

END
GO
