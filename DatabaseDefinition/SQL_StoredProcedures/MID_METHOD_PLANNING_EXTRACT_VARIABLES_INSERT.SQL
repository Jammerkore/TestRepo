--dv =============================================
--dv Create date: 2/23/2018
--dv Description: Inserts an PLANNING_EXTRACT method child variable row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_PLANNING_EXTRACT_VARIABLES_INSERT] 
	@METHOD_RID int,
	@VARIABLE_RID int,
	@VARIABLE_TYPE int,
	@VARIABLE_SEQ int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_PLANNING_EXTRACT_VARIABLES
		(
		METHOD_RID, 
		VARIABLE_RID, 
		VARIABLE_TYPE,
		VARIABLE_SEQ
		)
	VALUES
		(
		@METHOD_RID, 
		@VARIABLE_RID, 
		@VARIABLE_TYPE,
		@VARIABLE_SEQ
		)

	SELECT @@ROWCOUNT;

END
GO


