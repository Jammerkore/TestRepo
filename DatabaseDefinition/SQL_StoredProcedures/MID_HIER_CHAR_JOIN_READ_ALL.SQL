--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads all hierarchy characteristic joins
--dv History:		TT#3523 - JSmith - Performance of Anthro morning processing jobs
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HIER_CHAR_JOIN_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM HIER_CHAR_JOIN hcj WITH (NOLOCK)

END
GO