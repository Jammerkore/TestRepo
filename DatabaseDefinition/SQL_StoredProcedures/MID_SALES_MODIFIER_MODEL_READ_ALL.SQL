--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads all sales modifier models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_MODIFIER_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SALES_MODIFIER_MODEL

END
GO
