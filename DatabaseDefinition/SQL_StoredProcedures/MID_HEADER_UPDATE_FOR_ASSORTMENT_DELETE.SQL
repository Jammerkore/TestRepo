--dv =============================================
--dv Create date:	2/12/2014
--dv Description:	Updates the Header table when deleting
--dv				a Assortment
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_UPDATE_FOR_ASSORTMENT_DELETE]
	@HDR_RID INT
AS
BEGIN
	SET NOCOUNT ON
	UPDATE HEADER
	SET	ASRT_RID = null, 
	PLACEHOLDER_RID = null
	WHERE ASRT_RID = @HDR_RID
	AND DISPLAY_TYPE != '800740'
	SELECT @@ROWCOUNT;
END
GO