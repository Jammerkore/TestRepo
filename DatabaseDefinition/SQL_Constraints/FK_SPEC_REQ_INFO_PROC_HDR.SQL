/* Add the remaining keys, constraints and indexes for the table "SPECIAL_REQUEST".              */
ALTER TABLE [dbo].[SPECIAL_REQUEST_INFO]  WITH CHECK ADD  CONSTRAINT [FK_SPEC_REQ_INFO_PROC_HDR] FOREIGN KEY([PROCESS_RID])
REFERENCES [dbo].[PROC_HDR] ([PROCESS_RID])
GO
