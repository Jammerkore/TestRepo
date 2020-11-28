--dv =============================================
--dv Create date:	9/5/2013
--dv Description:	Preloads application text in one database call
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRELOAD_APPLICATION_TEXT]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TEXT_CODE, TEXT_VALUE FROM APPLICATION_TEXT 
	WHERE 
	(TEXT_CODE BETWEEN 804500 AND 804596) --Allocation Variables
	OR (TEXT_CODE BETWEEN 804300 AND 804310) --Allocation Variables
	OR TEXT_CODE IN (
	800750,
	800752, 
	800757, 
	300040,
	300105,
	300695,
	200020,
	200034,
	200063,
	803001
	)
END
GO
