--dv =============================================
--dv Create date: 2/20/2018
--dv Description:	Read All Extract Session Data
--dv =============================================
CREATE PROCEDURE [dbo].[RO_Extract_Session_Read_All]
AS
BEGIN
	SET NOCOUNT ON
	SELECT es.SessionKey,
           es.SessionActiveInd,
           es.SessionStartDateTime,
           es.SessionEndDateTime	
      FROM Extract_Session es WITH (NOLOCK)

END
GO






















