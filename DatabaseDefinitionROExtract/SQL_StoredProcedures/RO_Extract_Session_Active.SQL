--dv =============================================
--dv Create date: 2/20/2018
--dv Description:	Returns 1 if a session is active
--dv                0 if no session is active
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Extract_Session_Active]
  @ActiveSession INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	
    IF EXISTS(SELECT SessionKey
                FROM Extract_Session
               WHERE SessionActiveInd = 1)
    BEGIN
        SET @ActiveSession = 1
    END
    ELSE
    BEGIN
        SET @ActiveSession = 0
    END

    RETURN @ActiveSession

END
GO
























