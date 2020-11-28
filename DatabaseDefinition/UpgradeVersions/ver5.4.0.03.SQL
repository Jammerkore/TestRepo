/*RELEASE 5.4.0.03 09-25-2014 TT#1312-MD - stodd - Group Allocation Delete is orphaning Assortment Headers on the database */
begin
SET NOCOUNT ON 
/*=================================================================*/
/* Cleans up empty Group Allocations */
/*=================================================================*/
DECLARE @RC int
DECLARE @hdrRid int

/* Find Group Allocation headers that have no children (Placeholders or real headers) */
DECLARE C1 CURSOR FOR SELECT [HDR_RID] FROM [HEADER] h
  where NOT EXISTS(SELECT * FROM HEADER h2 WHERE h2.ASRT_RID = h.HDR_RID and h2.ASRT_RID <> h2.HDR_RID) and h.ASRT_TYPE = 3

OPEN C1
FETCH NEXT FROM C1 INTO @hdrRid
WHILE @@FETCH_STATUS = 0
BEGIN
    --select @hdrRid
	DELETE FROM ASSORTMENT_USER_VIEW_JOIN WITH (ROWLOCK)
		WHERE HDR_RID = @hdrRid
	EXECUTE @RC = SP_MID_HEADER_DELETE   @hdrRid
	FETCH NEXT FROM C1 INTO @hdrRid
END
CLOSE C1
DEALLOCATE C1

END
GO
