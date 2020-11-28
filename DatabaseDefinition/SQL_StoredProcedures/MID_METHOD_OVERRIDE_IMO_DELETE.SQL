--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Deletes an IMO override method for a store
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_IMO_DELETE]
	@METHOD_RID int,
	@ST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_OVERRIDE_IMO 
	WHERE METHOD_RID = @METHOD_RID
          AND ST_RID = @ST_RID

	SELECT @@ROWCOUNT;
END
GO