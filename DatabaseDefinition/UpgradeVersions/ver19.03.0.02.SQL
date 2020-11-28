/*RELEASE 19.01.0.02 8-7-2-19 RO-4201 - AGAllagher -RO User ID to be increased to 256 characters (from 64) */
alter table APPLICATION_USER alter column USER_NAME varchar(256) not null
GO
