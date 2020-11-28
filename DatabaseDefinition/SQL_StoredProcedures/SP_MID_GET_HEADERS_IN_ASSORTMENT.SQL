CREATE PROCEDURE [dbo].[SP_MID_GET_HEADERS_IN_ASSORTMENT] 
	(@ASRT_RID INT)
AS
BEGIN
	DECLARE @SEQ INT

	SET @SEQ = 0

	CREATE TABLE #myAsrtHeaders (
		HDR_RID INT,
		PLACEHOLDER_RID INT,
		PH_SEQ INT,
		HEADER_SEQ INT,
		HDR_ID VARCHAR(100),
		HDR_DAY DATETIME,
		DISPLAY_TYPE INT
		)

	INSERT INTO #myAsrtHeaders
	SELECT HDR_RID,
		PLACEHOLDER_RID,
		ASRT_PLACEHOLDER_SEQ,
		ASRT_HEADER_SEQ,
		HDR_ID,
		HDR_DAY,
		DISPLAY_TYPE
	FROM HEADER
	WHERE HDR_RID = @ASRT_RID
		OR ASRT_RID = @ASRT_RID
	ORDER BY ASRT_PLACEHOLDER_SEQ,
		ASRT_HEADER_SEQ --HDR_DAY, HDR_ID

	UPDATE #myAsrtHeaders
	SET PLACEHOLDER_RID = HDR_RID
	WHERE DISPLAY_TYPE = 800740

	--DECLARE @hdrRID int, @phRID int, @lastPhRID int
	--SET @lastPhRID = 0
	--DECLARE C1 cursor for select HDR_RID, PLACEHOLDER_RID from #myAsrtHeaders where DISPLAY_TYPE != 800739 and DISPLAY_TYPE != 800740 order by PLACEHOLDER_RID
	--OPEN C1
	--fetch next from C1 into @hdrRID, @phRID 
	--while @@FETCH_STATUS = 0
	--begin
	-- if @phRID = @lastPhRID
	--  begin
	--    set @SEQ = @SEQ + 1
	--  end 
	-- else
	--  begin
	--    set @lastPhRID = @phRID
	--    set @SEQ = 1
	--  end
	-- UPDATE #myAsrtHeaders SET SEQ = @SEQ WHERE HDR_RID = @hdrRID 
	-- fetch next from C1 into @hdrRID, @phRID 
	--end
	--CLOSE C1
	--DEALLOCATE C1
	SELECT *
	FROM #myAsrtHeaders
	ORDER BY PLACEHOLDER_RID,
		PH_SEQ,
		HEADER_SEQ,
		HDR_DAY,
		HDR_ID

	DROP TABLE #myAsrtHeaders
END
	/* End TT#2 - stodd - assortment */
GO


