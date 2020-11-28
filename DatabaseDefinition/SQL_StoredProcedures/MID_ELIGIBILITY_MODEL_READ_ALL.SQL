--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads all eligibility models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ELIGIBILITY_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM ELIGIBILITY_MODEL

END
GO
