--dv =============================================
--dv Create date:	10/18/2013
--dv Description:	Reads all subordinate headers from the master header table
--dv Modify:        10/19/2015 TT#1744-Data Layer Request - RMatelic - Add NOLOCK to MASTER_HEADER stored procedures.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_MASTER_HEADER_READ_ALL_SUBORD] 
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		SUBORD_HDR_RID, 
		MASTER_HDR_RID 
	FROM MASTER_HEADER WITH (NOLOCK)

END
GO
