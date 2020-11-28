--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Inserts a virtual lock
--dv =============================================
CREATE PROCEDURE [dbo].[MID_VIRTUAL_LOCK_INSERT]
	@LOCK_TYPE int, 
	@LOCK_ID varchar(2000)
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO VIRTUAL_LOCK
		(
		LOCK_TYPE, 
		LOCK_ID
		)
	VALUES
		(
		@LOCK_TYPE, 
		@LOCK_ID
		)

	SELECT @@ROWCOUNT;

END
GO