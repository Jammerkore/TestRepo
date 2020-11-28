/*RELEASE 19.01.0.01 7-1-2-19 RO-3992 - AGAllagher -RO User ID to be increased to 64 characters (from 30) */
alter table APPLICATION_USER alter column USER_NAME varchar(256) not null
GO
