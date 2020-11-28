/*RELEASE 5.2.0.22 07-29-2013 -TT#571-MD -jsobek -Allocation Audit needs to keep iterations of allocation */
IF .dbo.UDF_ColumnAlreadyExists('AUDIT_HEADER', 'LAST_ENTRY')=0
BEGIN
alter table AUDIT_HEADER add LAST_ENTRY bit null
END
go

ALTER TABLE [dbo].[AUDIT_HEADER] ADD  CONSTRAINT [DF_AUDIT_HEADER_LAST_ENTRY]  DEFAULT ((1)) FOR [LAST_ENTRY]
update AUDIT_HEADER SET LAST_ENTRY=1
go
