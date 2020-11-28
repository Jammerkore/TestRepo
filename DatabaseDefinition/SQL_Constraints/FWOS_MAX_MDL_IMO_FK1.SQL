/*BEGIN TT#485-MD-DOConnell-After creating a new demo database in Version 5 (not an upgrade), Received Invalid Column name for IMO_FWOS_MAX_RID and IMO_FWOS_MAX_TYPE when opening Style Review or when opening the VSW tab on Node Properties.*/
ALTER TABLE [dbo].[NODE_IMO]  WITH CHECK ADD CONSTRAINT [FWOS_MAX_MDL_IMO_FK1] FOREIGN KEY([IMO_FWOS_MAX_RID])
REFERENCES [dbo].[FWOS_MAX_MODEL] ([FWOSMAX_RID])
GO
