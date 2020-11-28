--dv =============================================
--dv Create date:	10/14/2013
--dv Description:	Returns header group based on header group RID passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_GROUP_READ]
	@HDR_GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HEADER_GROUP 
	WHERE HDR_GROUP_RID=@HDR_GROUP_RID

END
GO
