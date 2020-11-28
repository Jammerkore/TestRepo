--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Deletes a virtual lock
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VIRTUAL_LOCK_DELETE]
	@LOCK_TYPE int, 
	@LOCK_ID varchar(2000)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM VIRTUAL_LOCK
	WHERE LOCK_TYPE = @LOCK_TYPE 
		  AND LOCK_ID = @LOCK_ID

	SELECT @@ROWCOUNT;

END
GO


