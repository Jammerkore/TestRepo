--dv =============================================
--dv Create date: 3/18/2015
--dv Description:	Inserts or updates the STORE_CHAR for the new value.  Insert, updates, or removes the corresponding STORE_CHAR_JOIN entry for this store
--dv				When the value becomes NULL, the join table entry is removed
--dv				If no join entries exist for the STORE_CHAR, the STORE_CHAR row is removed
--dv				Changing a value should only affect that store and not other stores that are using the value
--dv				Assumed that the correct value is set for STORE_CHAR type (based on the SCG_TYPE of the store char group) (A null would indicate this value might need to be removed.)
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_UPSERT] 
@SCG_RID int,
@SC_RID int,  
@ST_RID int,
@TEXT_VALUE VARCHAR(50),
@DATE_VALUE SMALLDATETIME,
@NUMBER_VALUE FLOAT,
@DOLLAR_VALUE FLOAT
AS 
BEGIN 


--determine appropriate type
DECLARE @STORE_CHAR_GROUP_VALUE_TYPE INT;
SELECT @STORE_CHAR_GROUP_VALUE_TYPE = SCG_TYPE FROM STORE_CHAR_GROUP WHERE SCG_RID=@SCG_RID

--determine if the new value current exists


--lets see if other stores are using this value




--DECLARE @ROW_COUNT int;
--DECLARE @USTAT tinyint;
--SET @USTAT = 0;

--SELECT @ROW_COUNT = COUNT(*) 
--FROM VSW_REVERSE_ON_HAND VSW 
--WHERE VSW.HDR_RID =@HDR_RID
--  AND VSW.HN_RID = @HN_RID
--  AND VSW.ST_RID = @ST_RID

--IF @ROW_COUNT = 0
--	BEGIN
--		--insert a new row
--		INSERT INTO VSW_REVERSE_ON_HAND (HDR_RID, HN_RID, ST_RID, USTAT, VSW_REVERSE_ON_HAND_UNITS) 
--		VALUES(
--			@HDR_RID,
--			@HN_RID,
--			@ST_RID,
--			@USTAT,
--			@VSW_REVERSE_ON_HAND_UNITS
--		)
--	END
--ELSE
--	BEGIN
--		--update the existing row
--		UPDATE VSW_REVERSE_ON_HAND
--		SET 
--		USTAT=@USTAT,
--		VSW_REVERSE_ON_HAND_UNITS=@VSW_REVERSE_ON_HAND_UNITS
--		WHERE HDR_RID =@HDR_RID
--			AND HN_RID = @HN_RID
--			AND ST_RID = @ST_RID
--	END

	SELECT @@ROWCOUNT
END
GO
