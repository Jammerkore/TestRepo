--dv =============================================
--dv Create date:	02/7/2014
--dv Description:	Returns dimensions rid based on secondary size code 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DIMENSIONS_READ_WITH_SIZE_CODE_SECONDARY]
	@SIZE_CODE_SECONDARY varchar(100)
AS
BEGIN 
	SET NOCOUNT ON;

 	SELECT 
		DIMENSIONS_RID
    FROM DIMENSIONS
    WHERE SIZE_CODE_SECONDARY = @SIZE_CODE_SECONDARY
 
END
GO