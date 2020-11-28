--dv =============================================
--dv Create date:	02/7/2014
--dv Description:	Returns the sizes based on the primary size code
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZES_READ_SIZE_CODE_PRIMARY]
	@SIZE_CODE_PRIMARY varchar(100)
AS
BEGIN 
	SET NOCOUNT ON;

 	SELECT 
		SIZES_RID
    FROM SIZES
    WHERE SIZE_CODE_PRIMARY = @SIZE_CODE_PRIMARY
 
END
GO