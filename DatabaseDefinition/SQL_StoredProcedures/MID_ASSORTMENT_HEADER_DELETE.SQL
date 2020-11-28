--dv =============================================
--dv Create date:	8/13/2014
--dv Description:	This "unhooks" any real headers from the assortment 
--dv				Then deletes the placeholders and finally the assortment header
--dv				This delete also works for Group Allocations
--dv History:	    TT#1091-MD - STodd - Add Header Purge Criteria group allocation
--dv				10/7/2014 TT#1326-MD -jsobek -Error with MID_ASSORTMENT_HEADER_DELETE
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_HEADER_DELETE]
(
	@ASRT_RID INT,
	@HEADER_DELETE_COUNT INT OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @RC int
	DECLARE @hdrRid int, @displayType int
	DECLARE @hdrAsrtRid int, @phRid int, @asrtType char, @phSeq int, @hdrSeq int
	DECLARE @hdrId varchar(50)
	DECLARE C1 CURSOR FOR SELECT [HDR_RID]
	,[HDR_ID]
	,[ASRT_RID]
	,[PLACEHOLDER_RID]
	,[ASRT_TYPE]
	,[ASRT_PLACEHOLDER_SEQ]
	,[ASRT_HEADER_SEQ]
	,[DISPLAY_TYPE]
	FROM [HEADER]
	where HDR_RID = @ASRT_RID or ASRT_RID = @ASRT_RID
	order by ASRT_PLACEHOLDER_SEQ desc, ASRT_HEADER_SEQ desc /* TT#936 - MD - stodd - constraint error */

	set @HEADER_DELETE_COUNT = 0		/* TT#1091-MD - STodd - Add Header Purge Criteria group allocation */
	OPEN C1
	FETCH NEXT FROM C1 INTO @hdrRid, @hdrId, @hdrAsrtRid, @phRid, @asrtType, @phSeq, @hdrSeq, @displayType
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--select @hdrRid, @hdrId, @hdrAsrtRid, @phRid, @asrtType, @phSeq, @hdrSeq
		if @displayType = 800739 or @displayType = 800740 
		begin
			EXECUTE @RC = SP_MID_HEADER_DELETE @hdrRid, 0
			set @HEADER_DELETE_COUNT = @HEADER_DELETE_COUNT + 1		/* TT#1091-MD - STodd - Add Header Purge Criteria group allocation */
		end
		else
		begin
			update HEADER set ASRT_RID = NULL, PLACEHOLDER_RID = null, ASRT_PLACEHOLDER_SEQ = null, ASRT_HEADER_SEQ = null where HDR_RID = @hdrRid	/* TT#936 - MD - stodd - constraint error */
			update HEADER_BULK_COLOR set ASRT_BC_RID = null where  HDR_RID = @hdrRid	/* TT#928 - MD - stodd - Database error deleting a Group Allocation that contains headers */
		end
		FETCH NEXT FROM C1 INTO @hdrRid, @hdrId, @hdrAsrtRid, @phRid, @asrtType, @phSeq, @hdrSeq, @displayType
	END
	CLOSE C1
	DEALLOCATE C1

	SELECT @HEADER_DELETE_COUNT

END
GO