--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads all stock modifer models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MODIFIER_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM STOCK_MODIFIER_MODEL

END
GO
