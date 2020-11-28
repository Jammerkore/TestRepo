--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads a Detal Work Override LL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_DETAIL_READ_COUNT]
	@OLL_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT count(*) as 'MyCount'
    FROM OVERRIDE_LL_MODEL_DETAIL 
    WHERE OLL_RID = @OLL_RID
	AND HN_RID = @HN_RID
END
GO