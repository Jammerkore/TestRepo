--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts Assortment Properties
--dv History:  TT#2066-MD - JSmith - Ship to Date validation.  Is this how it should be working
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_INSERT]
	@HDR_RID int, 
	@RESERVE float, 
	@RESERVE_TYPE_IND int, 
	@SG_RID int, 
	@VARIABLE_TYPE int, 
	@VARIABLE_NUMBER int, 
	@INCL_ONHAND char(1), 
	@INCL_INTRANSIT char(1), 
	@INCL_SIMILAR_STORES char(1), 
	@INCL_COMMITTED char(1), 
	@AVERAGE_BY int, 
	@CDR_RID int, 
	@USER_RID int, 
	@ANCHOR_HN_RID int, 
	@LAST_PROCESS_DATETIME datetime,
	@BEGIN_DAY_CDR_RID int
AS
BEGIN
	SET NOCOUNT ON
	INSERT INTO ASSORTMENT_PROPERTIES
		(
		HDR_RID, 
		RESERVE, 
		RESERVE_TYPE_IND, 
		SG_RID, 
		VARIABLE_TYPE, 
		VARIABLE_NUMBER,
		INCL_ONHAND, 
		INCL_INTRANSIT, 
		INCL_SIMILAR_STORES, 
		INCL_COMMITTED, 
		AVERAGE_BY, 
		CDR_RID, 
		USER_RID, 
		ANCHOR_HN_RID, 
		LAST_PROCESS_DATETIME,
		BEGIN_DAY_CDR_RID
		)		
	VALUES
		(
		@HDR_RID, 
		@RESERVE, 
		@RESERVE_TYPE_IND, 
		@SG_RID, 
		@VARIABLE_TYPE, 
		@VARIABLE_NUMBER, 
		@INCL_ONHAND, 
		@INCL_INTRANSIT, 
		@INCL_SIMILAR_STORES, 
		@INCL_COMMITTED, 
		@AVERAGE_BY, 
		@CDR_RID, 
		@USER_RID, 
		@ANCHOR_HN_RID, 
		@LAST_PROCESS_DATETIME,
		@BEGIN_DAY_CDR_RID
		)		

    SELECT @@ROWCOUNT;
END

GO