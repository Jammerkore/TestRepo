-- drop all indexes and rebuild
/* Begin TT#739-MD - jsobek -Delete Stores */
--IF EXISTS (SELECT name FROM sysindexes WHERE name = 'STORE_WEEK_HISTORY_BIN_PK') 
--   ALTER TABLE STORE_WEEK_HISTORY_BIN
--     DROP CONSTRAINT STORE_WEEK_HISTORY_BIN_PK
--GO 

--DECLARE @indexName NVARCHAR(128)
--DECLARE @dropIndexSql NVARCHAR(4000)

--DECLARE tableIndexes CURSOR FOR
--SELECT name FROM sysindexes
--WHERE id = OBJECT_ID(N'STORE_WEEK_HISTORY_BIN') AND   
--  indid > 0 AND indid < 255 AND  
--  INDEXPROPERTY(id, name, 'IsStatistics') = 0
--ORDER BY indid DESC

--OPEN tableIndexes
--FETCH NEXT FROM tableIndexes INTO @indexName
--WHILE @@fetch_status = 0
--BEGIN  
--  SET @dropIndexSql = N'DROP INDEX STORE_WEEK_HISTORY_BIN.' + @indexName  
--  EXEC sp_executesql @dropIndexSql  
  
--  FETCH NEXT FROM tableIndexes INTO @indexName
--END

--CLOSE tableIndexes
--DEALLOCATE tableIndexes

--alter table "STORE_WEEK_HISTORY_BIN"
--	add constraint "STORE_WEEK_HISTORY_BIN_PK" primary key ("TIME_ID", "HN_RID", "COLOR_CODE_RID", "SIZE_CODE_RID")   
--go

--IF EXISTS (SELECT name FROM sysindexes WHERE name = 'STORE_WEEK_HISTORY_BIN_IDX2') DROP INDEX STORE_WEEK_HISTORY_BIN.STORE_WEEK_HISTORY_BIN_IDX2
--   CREATE INDEX STORE_WEEK_HISTORY_BIN_IDX2 ON STORE_WEEK_HISTORY_BIN(HN_RID)
--go

--IF EXISTS (SELECT name FROM sysindexes WHERE name = 'STORE_DAY_HISTORY_BIN_PK') 
--   ALTER TABLE STORE_DAY_HISTORY_BIN
--     DROP CONSTRAINT STORE_DAY_HISTORY_BIN_PK
--GO

--DECLARE @indexName NVARCHAR(128)
--DECLARE @dropIndexSql NVARCHAR(4000)

--DECLARE tableIndexes CURSOR FOR
--SELECT name FROM sysindexes
--WHERE id = OBJECT_ID(N'STORE_DAY_HISTORY_BIN') AND   
--  indid > 0 AND indid < 255 AND  
--  INDEXPROPERTY(id, name, 'IsStatistics') = 0
--ORDER BY indid DESC

--OPEN tableIndexes
--FETCH NEXT FROM tableIndexes INTO @indexName
--WHILE @@fetch_status = 0
--BEGIN  
--  SET @dropIndexSql = N'DROP INDEX STORE_DAY_HISTORY_BIN.' + @indexName  
--  EXEC sp_executesql @dropIndexSql  
  
--  FETCH NEXT FROM tableIndexes INTO @indexName
--END

--CLOSE tableIndexes
--DEALLOCATE tableIndexes

--alter table "STORE_DAY_HISTORY_BIN"
--	add constraint "STORE_DAY_HISTORY_BIN_PK" primary key ("TIME_ID", "HN_RID", "COLOR_CODE_RID", "SIZE_CODE_RID")
	
--IF EXISTS (SELECT name FROM sysindexes WHERE name = 'STORE_DAY_HISTORY_BIN_IDX2') DROP INDEX STORE_DAY_HISTORY_BIN.STORE_DAY_HISTORY_BIN_IDX2
--   CREATE INDEX STORE_DAY_HISTORY_BIN_IDX2 ON STORE_DAY_HISTORY_BIN(HN_RID)
--GO
/* End TT#739-MD - jsobek -Delete Stores */