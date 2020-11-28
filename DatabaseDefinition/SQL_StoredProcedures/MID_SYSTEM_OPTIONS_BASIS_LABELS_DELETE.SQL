--dv =============================================
--dv Create date:	11/26/2013
--dv Description:	Deletes a system basis label
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SYSTEM_OPTIONS_BASIS_LABELS_DELETE]
	@SYSTEM_OPTION_RID int,
	@LABEL_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SYSTEM_OPTIONS_BASIS_LABELS 
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTION_RID 
		  AND LABEL_TYPE = @LABEL_TYPE

	SELECT @@ROWCOUNT;

END
GO