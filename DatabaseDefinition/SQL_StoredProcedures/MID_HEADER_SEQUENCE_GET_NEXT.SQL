--dv ==================================================================
--dv Create date:	04/20/2015
--dv Description:	Returns the next available header sequence number
--dv ==================================================================
CREATE PROCEDURE [dbo].[MID_HEADER_SEQUENCE_GET_NEXT]
	@SEQ_LENGTH int, 
	@SEQ_NO INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @seqNo int
	DECLARE @newSeqNo int
	DECLARE @seqNostring char(9)

	BEGIN TRANSACTION

	SELECT @seqNo = SEQ_NO FROM HEADER_SEQUENCE (TABLOCKX)
	SET @SEQ_NO = @seqNo

	set @seqNostring = CAST(@seqNo AS char(9)) 

	/* if seq becomes all 9's for the @SEQ_LENGTH, reset seq to 1 */
	IF len(@seqNostring) - len(replace(@seqNostring,'9','')) = @SEQ_LENGTH
		SET @newSeqNo = 1
	ELSE
		SET @newSeqNo = @seqNo + 1

	UPDATE HEADER_SEQUENCE SET SEQ_NO = @newSeqNo

	COMMIT
END
GO
