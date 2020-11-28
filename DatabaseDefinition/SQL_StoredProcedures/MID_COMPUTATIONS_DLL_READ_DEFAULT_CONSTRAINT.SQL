--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads the system default constraint name for the give table and column
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATIONS_DLL_READ_DEFAULT_CONSTRAINT]
	@TABLE_NAME varchar(128), 
	@COLUMN_NAME varchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CONSTRAINT_NAME 
	FROM VW_GET_DEFAULT_CONSTRAINT WITH (NOLOCK) 
	WHERE TABLE_NAME = @TABLE_NAME 
		  AND COLUMN_NAME = @COLUMN_NAME

END
GO
