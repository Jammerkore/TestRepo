CREATE TRIGGER [dbo].[TRG_STORES_OPEN_DATESupdate]
on "STORES" 
for update 
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

  /* Clause for ON UPDATE to referencing table : NO ACTION. */
  if (update("SELLING_OPEN_DATE") or update("STOCK_OPEN_DATE"))
    begin
      if exists(select *
                FROM inserted i 
                INNER JOIN deleted d 
                   ON i.ST_RID = d.ST_RID 
				 WHERE d.SELLING_OPEN_DATE <> i.SELLING_OPEN_DATE)
               -- WHERE d.SELLING_OPEN_DATE <> i.SELLING_OPEN_DATE 
               --    OR d.STOCK_OPEN_DATE <> i.STOCK_OPEN_DATE)
      begin
	     -- use eMIDMessageCode for process and eMIDMessageSenderRecepient
         exec [dbo].[SP_MID_MESSAGE_QUEUE_INSERT] 15, 1, 1, 1, null, @RID output
      end
    end

  return
  errorHandler:
    raiserror ( @errorMsg,@ErrSeverity, @errorNumber)
  rollback transaction
END

GO
