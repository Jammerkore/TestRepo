create trigger [dbo].[TRG_HEADER_FOR_ENABLE_WORKFLOWinsert] 
   on [dbo].[HEADER]
   for insert
   as
   BEGIN
   declare
     @errorNumber int,
     @errorMsg varchar(255),
     @ErrSeverity int
    
     if (select API_WORKFLOW_RID from inserted) > 99
       begin
         update HEADER 
           set API_WORKFLOW_TRIGGER = '1'
               where HDR_RID in (select HDR_RID from inserted) 
       end
     return
 
     errorHandler:
       raiserror ( @errorMsg,@ErrSeverity, @errorNumber)
     rollback transaction
   END
GO