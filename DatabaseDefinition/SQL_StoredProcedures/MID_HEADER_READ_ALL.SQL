--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Returns all headers except the default header
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DEFAULT_HDR_RID int = 1;

	SELECT * 
	FROM UDF_HEADER_READ (null)
	WHERE HDR_RID <> @DEFAULT_HDR_RID
	ORDER BY HDR_DAY, HDR_ID

END
GO
