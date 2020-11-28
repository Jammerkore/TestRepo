--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads all Size Curve Keys based on Curve Name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CURVE_READ_KEY]
	@SIZE_CURVE_NAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sc.*
    FROM SIZE_CURVE sc WITH (NOLOCK)
    WHERE SIZE_CURVE_NAME = @SIZE_CURVE_NAME

END
GO