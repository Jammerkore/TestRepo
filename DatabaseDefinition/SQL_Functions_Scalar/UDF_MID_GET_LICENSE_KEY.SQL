--dv =============================================
--dv Create date:	8/14/2014
--dv Description:	Returns the licence key
--dv =============================================
CREATE FUNCTION [dbo].[UDF_MID_GET_LICENSE_KEY]
(
	@SYSTEM_OPTIONS_RID INT
)
RETURNS varchar(140)
AS
BEGIN
	DECLARE @LICENSE_KEY varchar(140)

	SELECT @LICENSE_KEY = LICENSE_KEY
	FROM SYSTEM_OPTIONS WITH (NOLOCK)
	WHERE SYSTEM_OPTION_RID = @SYSTEM_OPTIONS_RID

	RETURN @LICENSE_KEY
END
GO
