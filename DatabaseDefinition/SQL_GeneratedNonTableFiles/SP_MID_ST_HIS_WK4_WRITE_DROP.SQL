if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_HIS_WK4_WRITE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_HIS_WK4_WRITE]
GO
 
