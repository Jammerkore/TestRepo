--dv =============================================
--dv Create date:	02/7/2014
--dv Description:	Returns all sizes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZES_READ_ALL]
AS
BEGIN 
	SET NOCOUNT ON;

 	SELECT *
    FROM SIZES
 
END
GO