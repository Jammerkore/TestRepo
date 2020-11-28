--dv =============================================
--dv Create date:	2/7/2013
--dv Description:	Inserts a Primary Size Code
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZES_INSERT]
		@SIZE_CODE_PRIMARY varchar(100) 

AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO SIZES
		(
		SIZE_CODE_PRIMARY
		)
	VALUES 
		(
		@SIZE_CODE_PRIMARY 
		)

SELECT @@ROWCOUNT
END
GO