--dv Create date:	9/30/2013
--dv Description:	Retrieves pack, pack color, and pack size info for a given header in separate data tables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GET_PACK_DATA_FOR_HEADER]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @tempHeaderPack TABLE	 
					( 
					HDR_PACK_RID int, 
					HDR_RID int,  
					HDR_PACK_NAME varchar(100),  
					PACKS int,  
					MULTIPLE int, 
					RESERVE_PACKS int, 
					GENERIC_IND char(1), 
					ASSOCIATED_PACK_RID int, 
					SEQ int
					) 
	DECLARE @tempHeaderPackColor TABLE	 
					( 
					HDR_PACK_RID int, 
					HDR_PC_RID int,  
					COLOR_CODE_RID int,  
					UNITS int,  
					SEQ int, 
					NAME varchar(50), 
					DESCRIPTION varchar(100), 
					LAST_PCSZ_KEY_USED int
					PRIMARY KEY (HDR_PC_RID) 
					) 

	INSERT INTO @tempHeaderPack SELECT * FROM HEADER_PACK WHERE HDR_RID = @HDR_RID
	SELECT * FROM @tempHeaderPack

	INSERT INTO @tempHeaderPackColor SELECT * FROM  HEADER_PACK_COLOR WHERE HDR_PACK_RID in (SELECT HDR_PACK_RID FROM @tempHeaderPack) ORDER BY SEQ
	SELECT * FROM @tempHeaderPackColor

	SELECT HPC.COLOR_CODE_RID, HPCS.*
	FROM HEADER_PACK_COLOR_SIZE HPCS
	INNER JOIN @tempHeaderPackColor HPC ON HPCS.HDR_PC_RID=HPC.HDR_PC_RID
	ORDER BY HPCS.SEQ

	SELECT * FROM HEADER_PACK_ROUNDING WHERE HDR_RID = @HDR_RID ORDER BY PACK_MULTIPLE_RID
END
GO
