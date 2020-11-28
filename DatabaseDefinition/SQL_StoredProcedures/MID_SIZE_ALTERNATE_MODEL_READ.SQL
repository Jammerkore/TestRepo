--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads a size alternate model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALTERNATE_MODEL_READ]
	@SIZE_ALTERNATE_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SIZE_ALTERNATE_MODEL 
	WHERE SIZE_ALTERNATE_RID = @SIZE_ALTERNATE_RID

END
GO
