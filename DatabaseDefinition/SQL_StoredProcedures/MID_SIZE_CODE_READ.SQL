--dv =============================================
--dv Create date:	02/7/2014
--dv Description:	Returns Size Code ID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CODE_READ]
	@SIZE_CODE_RID int
AS
BEGIN 
	SET NOCOUNT ON;

 	SELECT 
		SIZE_CODE_PRIMARY,
		SIZE_CODE_SECONDARY
    FROM SIZE_CODE
    WHERE SIZE_CODE_RID = @SIZE_CODE_RID
 
END
GO