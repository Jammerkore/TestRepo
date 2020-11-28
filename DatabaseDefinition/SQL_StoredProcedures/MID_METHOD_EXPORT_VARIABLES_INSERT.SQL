--dv =============================================
--dv Create date: 12/10/2013
--dv Description: Inserts an export method child variable row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_EXPORT_VARIABLES_INSERT] 
	@METHOD_RID int,
	@VARIABLE_RID int,
	@VARIABLE_SEQ int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_EXPORT_VARIABLES
		(
		METHOD_RID, 
		VARIABLE_RID, 
		VARIABLE_SEQ
		)
	VALUES
		(
		@METHOD_RID, 
		@VARIABLE_RID, 
		@VARIABLE_SEQ
		)

	SELECT @@ROWCOUNT;

END
GO


