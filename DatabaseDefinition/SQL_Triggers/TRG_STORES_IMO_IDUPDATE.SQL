IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[TRG_STORES_IMO_IDUpdate]') AND OBJECTPROPERTY(id, N'IsTrigger') = 1)
DROP TRIGGER [dbo].[TRG_STORES_IMO_IDUpdate]
GO

CREATE TRIGGER [dbo].[TRG_STORES_IMO_IDUpdate]
on dbo.[STORES]
AFTER UPDATE
as
BEGIN
  declare
	@rowsAffected int,
	@nullRows int,
	@validRows int, 
	@errorNumber int,
	@errorMsg varchar(255),
	@ErrSeverity int

  select @rowsAffected = @@rowcount

  if (update("IMO_ID"))
  begin
      delete from NODE_IMO where ST_RID in
	   (
	     select d.ST_RID from deleted d
	       join inserted i on i.ST_RID = d.ST_RID
		  where i.IMO_ID is null and d.IMO_ID is not null
	   )
  end

  return
  errorHandler:
    raiserror ( @errorMsg,@ErrSeverity, @errorNumber)
  rollback transaction
END

GO
