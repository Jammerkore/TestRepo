CREATE TRIGGER [dbo].[TRG_STORES_OPEN_DATESinsert]
on "STORES" 
after insert 
as
BEGIN
  declare
	@rowsAffected int,
	@nullRows int,
	@validRows int, 
	@errorNumber int,
	@errorMsg varchar(255),
	@ErrSeverity int,
	@RID int

  select @rowsAffected = @@rowcount

  if exists(select *
             FROM inserted i
			 where i.SELLING_OPEN_DATE is not null)
			--where i.SELLING_OPEN_DATE is not null
            --   or i.STOCK_OPEN_DATE is not null)
   begin
	  -- use eMIDMessageCode for process and eMIDMessageSenderRecepient
      exec [dbo].[SP_MID_MESSAGE_QUEUE_INSERT] 15, 1, 1, 1, null, @RID output
   end

  return
  errorHandler:
    raiserror ( @errorMsg,@ErrSeverity, @errorNumber)
  rollback transaction
END

GO
