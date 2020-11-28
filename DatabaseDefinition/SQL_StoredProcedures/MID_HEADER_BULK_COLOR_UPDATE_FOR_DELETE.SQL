--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Updates Header Bulk Color
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_UPDATE_FOR_DELETE]
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE HEADER_BULK_COLOR 
	SET	ASRT_BC_RID = null
	WHERE ASRT_BC_RID in (select HDR_BC_RID FROM HEADER_BULK_COLOR where HDR_RID = @HDR_RID)
	SELECT @@ROWCOUNT;
END
GO