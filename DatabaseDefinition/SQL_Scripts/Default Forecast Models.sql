------------------------------------------------------------------------------
-- SQL to add default Forecast Models to the database
-- 12/19/2008 MID Track #5773 - KJohnson - Planned FWOS Enhancement
------------------------------------------------------------------------------

IF NOT EXISTS(SELECT * FROM FORECAST_MODEL where FORECAST_MOD_RID = 1)
BEGIN
    set IDENTITY_INSERT FORECAST_MODEL ON
    insert INTO  FORECAST_MODEL (FORECAST_MOD_RID,FORECAST_MOD_ID,DEFAULT_IND,CALC_MODE) 
		values(1,'Total $', 0, 'Default')
    set IDENTITY_INSERT FORECAST_MODEL OFF
    DBCC CHECKIDENT (FORECAST_MODEL,reseed,100)

    insert INTO  FORECAST_MODEL_VARIABLE values(1,0,1,800662,-1,0,0,0,0,0,0,1,0,0)
    insert INTO  FORECAST_MODEL_VARIABLE values(1,1,16,800661,1,1,1,1,1,1,1,0,0,0)
END
ELSE
BEGIN
	UPDATE FORECAST_MODEL
	set FORECAST_MOD_ID = 'Total $', 
		DEFAULT_IND = 0,  CALC_MODE = 'Default'
		where FORECAST_MOD_RID = 1	 
	delete from FORECAST_MODEL_VARIABLE where FORECAST_MOD_RID = 1
	insert INTO  FORECAST_MODEL_VARIABLE values(1,0,1,800662,-1,0,0,0,0,0,0,1,0,0)
	insert INTO  FORECAST_MODEL_VARIABLE values(1,1,16,800661,1,1,1,1,1,1,1,0,0,0)
END
go


