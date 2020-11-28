--dv =============================================
--dv Modified date: 2/28/2014
--dv Flags: BYPASS_SCOPEIDENTITY_VALIDATION
--dv Description:	Inserts or updates build pack criteria
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_BLD_PCK_CRIT_INS] 
	@BPC_NAME VARCHAR(50),
	@BPC_COMP_MIN INT,
	@BPC_SIZE_MULT INT,
	@BPC_PACK_MULT INT,
	@BPC_COMBO_NAME VARCHAR(50),
	@BPC_COMBO_MAX_PACKS INT
AS
DECLARE @BPC_RID INT
DECLARE @BPC_COMBO_RID INT

BEGIN
	IF EXISTS (
			SELECT BPC_RID
			FROM BUILD_PACK_CRITERIA
			-- Begin TT#756 - JScott - Can not delete or hide build packs no longer viable
			-- WHERE BPC_NAME = @BPC_NAME
			-- AND BPC_COMP_MIN = @BPC_COMP_MIN
			-- AND BPC_SIZE_MULT = @BPC_SIZE_MULT)
			WHERE BPC_NAME = @BPC_NAME
			)
		-- End TT#756 - JScott - Can not delete or hide build packs no longer viable
	BEGIN
		SELECT @BPC_RID = BPC_RID
		FROM BUILD_PACK_CRITERIA
		WHERE BPC_NAME = @BPC_NAME

		-- Begin TT#756 - JScott - Can not delete or hide build packs no longer viable
		--	 AND BPC_COMP_MIN = @BPC_COMP_MIN
		--	 AND BPC_SIZE_MULT = @BPC_SIZE_MULT
		UPDATE BUILD_PACK_CRITERIA
		SET BPC_COMP_MIN = @BPC_COMP_MIN
		WHERE BPC_RID = @BPC_RID

		UPDATE BUILD_PACK_CRITERIA
		SET BPC_SIZE_MULT = @BPC_SIZE_MULT
		WHERE BPC_RID = @BPC_RID
			-- End TT#756 - JScott - Can not delete or hide build packs no longer viable
	END
	ELSE
	BEGIN
		INSERT INTO BUILD_PACK_CRITERIA (
			BPC_NAME,
			BPC_COMP_MIN,
			BPC_SIZE_MULT
			)
		VALUES (
			@BPC_NAME,
			@BPC_COMP_MIN,
			@BPC_SIZE_MULT
			)

		SELECT @BPC_RID = SCOPE_IDENTITY() -- TT#3256 - JSmith - Replace @@IDENTITY with SCOPE_IDENTITY()
	END

	IF EXISTS (
			SELECT BPC_COMBO_RID
			FROM BUILD_PACK_COMBO
			WHERE BPC_RID = @BPC_RID
				AND BPC_COMBO_NAME = @BPC_COMBO_NAME
			)
	BEGIN
		SELECT @BPC_COMBO_RID = BPC_COMBO_RID
		FROM BUILD_PACK_COMBO
		WHERE BPC_RID = @BPC_RID
			AND BPC_COMBO_NAME = @BPC_COMBO_NAME
	END
	ELSE
	BEGIN
		INSERT INTO BUILD_PACK_COMBO (
			BPC_RID,
			BPC_COMBO_NAME
			)
		VALUES (
			@BPC_RID,
			@BPC_COMBO_NAME
			)

		SELECT @BPC_COMBO_RID = SCOPE_IDENTITY() -- TT#3256 - JSmith - Replace @@IDENTITY with SCOPE_IDENTITY()
	END

	-- Begin TT#756 - JSmith - Can not delete or hide build packs no longer viable	
	IF NOT EXISTS (
			SELECT *
			FROM BUILD_PACK_CONFIG
			WHERE BPC_COMBO_RID = @BPC_COMBO_RID
				-- Begin TT#756 - JSmith - Can not delete or hide build packs no longer viable
				-- AND BPC_PACK_MULT = @BPC_PACK_MULT
				-- AND BPC_COMBO_MAX_PACKS = @BPC_COMBO_MAX_PACKS)
				AND BPC_PACK_MULT = @BPC_PACK_MULT
			)
		-- End TT#756 - JSmith - Can not delete or hide build packs no longer viable
	BEGIN
		INSERT INTO BUILD_PACK_CONFIG (
			BPC_COMBO_RID,
			BPC_PACK_MULT,
			BPC_COMBO_MAX_PACKS
			)
		VALUES (
			@BPC_COMBO_RID,
			@BPC_PACK_MULT,
			@BPC_COMBO_MAX_PACKS
			)
	END
			-- Begin TT#756 - JSmith - Can not delete or hide build packs no longer viable
	ELSE
	BEGIN
		UPDATE BUILD_PACK_CONFIG
		SET BPC_COMBO_MAX_PACKS = @BPC_COMBO_MAX_PACKS
		WHERE BPC_COMBO_RID = @BPC_COMBO_RID
			AND BPC_PACK_MULT = @BPC_PACK_MULT
	END
			-- End TT#756 - JSmith - Can not delete or hide build packs no longer viable
	SELECT @@ROWCOUNT
END
GO


