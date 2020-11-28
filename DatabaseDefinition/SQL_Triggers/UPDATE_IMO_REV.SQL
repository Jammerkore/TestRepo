CREATE TRIGGER [dbo].[UPDATE_IMO_REV]
ON [dbo].[STORE_IMO]
after insert, update, delete
AS 
set nocount on

declare @FlashBack datetime
set @FlashBack = getdate()

insert into [dbo].[STORE_IMO_REV]
select  coalesce(I.HN_RID, D.HN_RID), 
		coalesce(I.ST_RID, D.ST_RID), coalesce(D.UNITS,0) - coalesce(I.UNITS,0) as UNITS, @FlashBack
from inserted I
	full outer join deleted D on I.HN_RID = D.HN_RID
		and I.ST_RID = D.ST_RID
where (coalesce(D.UNITS,0) - coalesce(I.UNITS,0))>0

GO
