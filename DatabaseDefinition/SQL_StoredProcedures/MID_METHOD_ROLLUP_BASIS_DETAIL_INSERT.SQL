--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Inserts Method Rollup Basis detail
--dv History:	    9/10/2014 TT1295-MD -jsobek -Store To Chain flag not saved or not retrieved
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_BASIS_DETAIL_INSERT]
		@METHOD_RID int,
		@DETAIL_SEQ int,
		@FROM_LEVEL_HRID int,
		@FROM_LEVEL_TYPE int,
		@FROM_LEVEL_SEQ int,
		@FROM_LEVEL_OFFSET int,
		@TO_LEVEL_HRID int,
		@TO_LEVEL_TYPE int,
		@TO_LEVEL_SEQ int,
		@TO_LEVEL_OFFSET int,
		@STORE_IND int,
		@CHAIN_IND int,
		@STORE_TO_CHAIN_IND int
 
 AS
	BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_ROLLUP_BASIS_DETAIL 
		(
		METHOD_RID,
		DETAIL_SEQ,
		FROM_LEVEL_HRID,
		FROM_LEVEL_TYPE,
		FROM_LEVEL_SEQ,
		FROM_LEVEL_OFFSET,
		TO_LEVEL_HRID,
		TO_LEVEL_TYPE,
		TO_LEVEL_SEQ,
		TO_LEVEL_OFFSET,
		STORE_IND,
		CHAIN_IND,
		STORE_TO_CHAIN_IND
		)
	VALUES
		(
		@METHOD_RID,
		@DETAIL_SEQ,
		@FROM_LEVEL_HRID,
		@FROM_LEVEL_TYPE,
		@FROM_LEVEL_SEQ,
		@FROM_LEVEL_OFFSET,
		@TO_LEVEL_HRID,
		@TO_LEVEL_TYPE,
		@TO_LEVEL_SEQ,
		@TO_LEVEL_OFFSET,
		@STORE_IND,
		@CHAIN_IND,
		@STORE_TO_CHAIN_IND
		)
			
	SELECT @@ROWCOUNT;
	END

GO