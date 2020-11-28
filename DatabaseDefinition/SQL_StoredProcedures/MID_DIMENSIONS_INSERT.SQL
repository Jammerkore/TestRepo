--dv =============================================
--dv Create date:	2/3/2013
--dv Description:	Inserts a Size Code Dimension
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DIMENSIONS_INSERT]
		@SIZE_CODE_SECONDARY varchar(100) 

AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO DIMENSIONS
		(
		SIZE_CODE_SECONDARY 
		)
	VALUES 
		(
		@SIZE_CODE_SECONDARY 
		)
SELECT @@ROWCOUNT

END
GO