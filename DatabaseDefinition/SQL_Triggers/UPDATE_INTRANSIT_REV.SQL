CREATE TRIGGER [dbo].[UPDATE_INTRANSIT_REV]
ON [dbo].[STORE_INTRANSIT]
after insert, update, delete
AS 
set nocount on

declare @FlashBack datetime
set @FlashBack = getdate()

insert into [dbo].[STORE_INTRANSIT_REV]
select @FlashBack, coalesce(I.TIME_ID, D.TIME_ID), coalesce(I.HN_RID, D.HN_RID), 
		coalesce(I.ST_RID, D.ST_RID), coalesce(D.UNITS,0) - coalesce(I.UNITS,0) as UNITS
from inserted I
	full outer join deleted D on I.HN_RID = D.HN_RID
		and I.TIME_ID = D.TIME_ID
		and I.ST_RID = D.ST_RID
where (coalesce(D.UNITS,0) - coalesce(I.UNITS,0))>0

GO
