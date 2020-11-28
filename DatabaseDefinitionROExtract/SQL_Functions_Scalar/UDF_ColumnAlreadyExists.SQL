CREATE FUNCTION [dbo].[UDF_ColumnAlreadyExists]
 (@TableName NVARCHAR(128),@ColumnName NVARCHAR(128))
 RETURNS INTEGER--Returns 0 if column does not exist. Returns 1 if column exists.
 AS
 BEGIN
 --See if the Table already contains the column.
 
IF EXISTS
 (SELECT * FROM sysobjects O INNER JOIN syscolumns C ON O.id=C.id
 WHERE ObjectProperty(O.id,'IsUserTable')=1
 AND O.name=@TableName
 AND C.name=@ColumnName)
 RETURN 1
 --Table does not contain the column.
 RETURN 0
 END

GO
