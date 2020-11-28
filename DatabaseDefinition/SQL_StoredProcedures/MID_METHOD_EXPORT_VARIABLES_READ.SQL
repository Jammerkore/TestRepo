--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Reads method export variables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_EXPORT_VARIABLES_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NoRID int = -1;

	SELECT
		COALESCE(METHOD_RID, @NoRID) METHOD_RID,
		COALESCE(VARIABLE_RID, @NoRID) VARIABLE_RID,
		COALESCE(VARIABLE_SEQ, @NoRID) VARIABLE_SEQ
	FROM METHOD_EXPORT_VARIABLES 
	WHERE METHOD_RID = @METHOD_RID

END
GO