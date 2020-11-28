--dv =============================================
--dv Create date:	2/13/2014
--dv Modified date:	9/30/2014
--dv Description:	Returns Headers to be Enqueued
--dv History:		TT#1306-MD - stodd - header locking issue for GA
--dv =============================================
CREATE PROCEDURE [dbo].[MID_IDENTIFY_HDRS_FOR_ENQ_READ]
	@HDR_RID_LIST HDR_RID_TYPE READONLY,
	@ReturnCode int = -100 output
AS 
BEGIN
	-- This Store Procedure gets all non-multi headers with the given color within a given style
declare @hdrtbl  table (HDR_RID int)
declare @grptbl  table (HDR_RID int)
set nocount on
insert @hdrtbl 
--   SELECT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@HdrRID_List, ',')
	SELECT HDR_RID FROM @HDR_RID_LIST

insert @hdrtbl 
   select SUBORD_HDR_RID as HDR_RID 
     from dbo.MASTER_HEADER 
     where MASTER_HDR_RID in (Select HDR_RID from @hdrtbl)
 if (@@rowcount = 0)
 begin
    insert @hdrtbl 
      SELECT MASTER_HDR_RID as HDR_RID from MASTER_HEADER where SUBORD_HDR_RID in (Select HDR_RID from @hdrtbl)
 end
insert @grptbl 
  select HDR_GROUP_RID as HDR_RID 
    from HEADER H 
    where H.HDR_GROUP_RID > 1 
      and (H.HDR_GROUP_RID in 
             (Select HDR_RID from @hdrtbl) 
       or H.HDR_RID in (Select HDR_RID from @hdrtbl))
if (@@rowcount > 0)
  begin  
	insert  @hdrtbl
       select HDR_RID from HEADER H where H.HDR_GROUP_RID > 1 and H.HDR_GROUP_RID in (Select HDR_RID from @grptbl) 
    insert @hdrtbl
       select * from @grptbl   
  end  
/* Begin TT#1306-MD - stodd - header locking issue for GA */    
/* ASSORTMENT and GROUP ALLOCATION */
insert @grptbl 
  SELECT HDR_RID FROM HEADER
  where ASRT_RID in	(SELECT ASRT_RID FROM HEADER where HDR_RID in (Select HDR_RID from @hdrtbl))
if (@@rowcount > 0)
  begin  
    insert @hdrtbl
       select * from @grptbl   
  end  
/* End TT#1306-MD - stodd - header locking issue for GA */   
select DISTINCT HDR_RID from @hdrtbl 
set @ReturnCode = @@rowcount
return @ReturnCode
END

GO