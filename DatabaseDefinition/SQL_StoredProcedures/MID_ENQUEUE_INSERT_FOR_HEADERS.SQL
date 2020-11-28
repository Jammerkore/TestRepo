--dv =============================================
--dv Create date:	1/30/2014
--dv Description:	Attempts to enqueue headers passed in, otherwise returns them in an error table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_INSERT_FOR_HEADERS]
    @EnqType     int,
    @UserRID     int,
    @ThreadID    int,
    @TranID      bigint,
	@HDR_RID_LIST HDR_RID_TYPE READONLY,
	@ReturnCode  int = -100 output
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Errortbl TABLE (ENQUEUE_TYPE int, USER_RID int, OWNING_THREADID int, OWNING_TRANID bigint, ENQUEUE_TIMESTAMP DateTime, HDR_RID int, HDR_ID VARCHAR(100)) 
	
	INSERT @Errortbl (ENQUEUE_TYPE, USER_RID, OWNING_THREADID, OWNING_TRANID, ENQUEUE_TIMESTAMP, HDR_RID, HDR_ID)
	    SELECT E.ENQUEUE_TYPE, E.USER_RID, coalesce(E.OWNING_THREADID,0), coalesce(E.OWNING_TRANID,0), E.ENQUEUE_TIMESTAMP, E.RID, 'unknown Header ID'
		FROM dbo.MID_ENQUEUE E
		WHERE ENQUEUE_TYPE = @EnqType
		      AND E.RID IN (SELECT HDR_RID FROM @HDR_RID_LIST)


	IF (@@ROWCOUNT = 0)
	  BEGIN   
		INSERT dbo.MID_ENQUEUE WITH (ROWLOCK) (ENQUEUE_TYPE, USER_RID, OWNING_THREADID, OWNING_TRANID, RID)
		   SELECT @EnqType, @UserRID, @ThreadID, @TranID, HDR_RID FROM @HDR_RID_LIST
		SET @ReturnCode = @@ROWCOUNT
	  END
	ELSE
	  BEGIN
	    SET @ReturnCode = 0
		UPDATE @Errortbl 
		   SET HDR_ID = H.HDR_ID
		FROM @Errortbl E INNER JOIN HEADER H ON E.HDR_RID = H.HDR_RID
	  END  
	
	SELECT *
	FROM @Errortbl

	RETURN @ReturnCode


END 
GO


