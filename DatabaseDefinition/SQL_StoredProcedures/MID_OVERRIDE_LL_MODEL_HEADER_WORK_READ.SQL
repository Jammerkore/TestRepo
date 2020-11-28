--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads a Detal Work Override LL Model RID
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_WORK_READ]
	@NAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT OLL_RID
    FROM OVERRIDE_LL_MODEL_HEADER_WORK 
    WHERE NAME = @NAME
END
GO



