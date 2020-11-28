--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads the subordinate headers for the header rid passed in
--dv Modify:        10/19/2015 TT#1744-Data Layer Request - RMatelic - Add NOLOCK to MASTER_HEADER stored procedures.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MASTER_HEADER_READ_SUBORD] 
	@MASTER_HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT SUBORD_HDR_RID
    FROM MASTER_HEADER WITH (NOLOCK)
    WHERE MASTER_HDR_RID = @MASTER_HDR_RID

END
GO
