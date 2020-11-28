--dv =============================================
--dv Create date:	02/7/2014
--dv Description:	Returns distinct size code product categories
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CODE_READ_ALL_PRODUCT_CATEGORY]
AS
BEGIN 
	SET NOCOUNT ON;

 	SELECT DISTINCT
		SIZE_CODE_PRODUCT_CATEGORY
    FROM SIZE_CODE
 
END
GO