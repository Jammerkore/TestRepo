--dv =============================================
--dv Create date:	03/27/2015
--dv Description:	Reads the API Process Control Table for a specific API Process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_API_PROCESS_CONTROL_RULES_READ]
	@API_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM API_PROCESS_CONTROL_RULES WITH (NOLOCK)
    WHERE API_ID = @API_ID

END
GO

