--dv =============================================
--dv Create date:	2/18/2014
--dv Description:	Deletes VSW Reverse On Hand rows based on the header list provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VSW_REVERSE_ON_HAND_DELETE_FROM_HEADER]
	@HEADER_LIST HDR_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM VSW_REVERSE_ON_HAND 
	WHERE HDR_RID IN (SELECT HDR_RID FROM @HEADER_LIST)

	SELECT @@ROWCOUNT;
END
GO