if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MID_ST_HIS_WK8_WRITE]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SP_MID_ST_HIS_WK8_WRITE]
GO
 
