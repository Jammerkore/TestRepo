--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Inserts a user plan basis
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_BASIS_INSERT]
	@USER_RID int, 
	@BASIS_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO USER_PLAN_BASIS
		(
		USER_RID, 
		BASIS_RID
		)
	VALUES
		(
		@USER_RID, 
		@BASIS_RID
		)

	SELECT @@ROWCOUNT;

END
GO


