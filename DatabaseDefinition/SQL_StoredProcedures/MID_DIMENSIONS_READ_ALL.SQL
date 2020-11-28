--dv =============================================
--dv Create date:	02/7/2014
--dv Description:	Returns all dimensions
--dv History: 4/29/2014 -jsobek -Fixed issue by adding the size code secondary field
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DIMENSIONS_READ_ALL]
AS
BEGIN 
	SET NOCOUNT ON;

 	SELECT 
		DIMENSIONS_RID,
		SIZE_CODE_SECONDARY
    FROM DIMENSIONS
 
END
GO
