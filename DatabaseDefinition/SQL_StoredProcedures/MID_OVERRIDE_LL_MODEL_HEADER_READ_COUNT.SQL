--dv =============================================
--dv Create date:	01/28/2014
--dv Description:	Reads a Header Override LL Model
--dv History:		Added nolock for TT#1277-MD -jsobek -Saving new model after changing Exclude for merchandise gets database error
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OVERRIDE_LL_MODEL_HEADER_READ_COUNT]
	@OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT count(*) as 'MyCount'
    FROM OVERRIDE_LL_MODEL_HEADER WITH (NOLOCK)
    WHERE OLL_RID = @OLL_RID
END
GO