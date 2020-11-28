------------------------------------------------------------------------------
-- SQL to add default Computation Driver Models to the database
------------------------------------------------------------------------------
set IDENTITY_INSERT COMPUTATION_MODEL ON
insert INTO  COMPUTATION_MODEL (COMP_MODEL_RID, COMP_MODEL_ID, CALC_MODE) values(1, 'Default', 'Default')
set IDENTITY_INSERT COMPUTATION_MODEL OFF
DBCC CHECKIDENT (COMPUTATION_MODEL,reseed,100)
GO

-- COMP_TYPE
-- 0 - none
-- 1 - all
-- 2 - chain
-- 3 - store
insert INTO  
COMPUTATION_MODEL_ENTRY (COMP_MODEL_RID, COMP_MODEL_SEQUENCE, COMP_TYPE, FV_RID, CHANGE_VARIABLE, PRODUCT_LEVEL)
values(1,1,2,2,1,null)

go
