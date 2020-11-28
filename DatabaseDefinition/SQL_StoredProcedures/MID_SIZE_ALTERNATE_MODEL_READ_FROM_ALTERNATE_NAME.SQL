--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Reads a size alternate model from the name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_ALTERNATE_MODEL_READ_FROM_ALTERNATE_NAME]
	@SIZE_ALTERNATE_NAME varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM SIZE_ALTERNATE_MODEL 
	WHERE SIZE_ALTERNATE_NAME = @SIZE_ALTERNATE_NAME

END
GO
