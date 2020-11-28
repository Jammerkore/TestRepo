--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads all forward weeks of supply (FWOS) max models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM FWOS_MAX_MODEL

END
GO
