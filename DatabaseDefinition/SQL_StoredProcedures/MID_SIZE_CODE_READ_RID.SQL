--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Reads the size code rid from the size code id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_CODE_READ_RID]
	@SIZE_CODE_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT SIZE_CODE_RID 
	FROM SIZE_CODE 
	WHERE SIZE_CODE_ID = @SIZE_CODE_ID

END
GO