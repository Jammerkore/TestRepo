-- Requirements
--    Database is initially created.  
--    Organizational Hierarchy and Levels must be defined.  
--    Fiscal Calendar must be adjusted if needed (different from the typical retail fiscal calendar - Feb - Jan).
--    There are no Task Lists or Jobs already created on the database. 
--    Adjust default settings below if different from default locations.


declare @installDir varchar(50), @dataDir varchar(50), @rollFromColor int

-- default settings
--   @installDir - the location where the MIDRetail application is found
--   @dataDir - the location where the MIDRetailData is found
--   @rollFromColor - identifies if lowest roll level is color: 1=True;0=False (will roll from Style level)
set @installDir = 'C:\MIDRetail'
set @dataDir = 'C:\MIDRetailData'
set @rollFromColor = 1

set nocount on
declare @orgHierarchyRID int, @noHierarchyLevels int, @noTaskLists int, @noJobs int, @styleLevel int, @colorLevel int, @HN_RID int
declare @debug bit, @FV_ActualRID int, @FV_ActionRID int
declare @explorerFolderRID int, @folderRID int,  @taskListRID int,  @taskRID int

set @orgHierarchyRID = -1
set @noHierarchyLevels = -1
set @styleLevel = -1
set @colorLevel = -1
set @noTaskLists = 0
set @FV_ActualRID = 1
set @FV_ActionRID = 2

set @debug = 0

-- Verify script can be executed

select @orgHierarchyRID = PH_RID from PRODUCT_HIERARCHY ph where ph.PH_TYPE = 800000

if @orgHierarchyRID = -1
begin
   print 'No organizational hierarchy found'
   return
end

select @noHierarchyLevels = max(PHL_SEQUENCE) from PRODUCT_HIERARCHY_LEVELS

if @noHierarchyLevels = -1
begin
   print 'No organizational hierarchy levels found'
   return
end

select @noTaskLists = count(*) from TASKLIST

if @noTaskLists > 0
begin
   print 'TaskLists are defined. Script cannot be executed'
   return
end

select @noJobs = count(*) from JOB

if @noJobs > 0
begin
   print 'Jobs are defined. Script cannot be executed'
   return
end

select @styleLevel = PHL_SEQUENCE from PRODUCT_HIERARCHY_LEVELS phl where phl.PHL_TYPE = 800202
select @colorLevel = PHL_SEQUENCE from PRODUCT_HIERARCHY_LEVELS phl where phl.PHL_TYPE = 800203
select @HN_RID = HN_RID from HIERARCHY_NODE hn where hn.HOME_PH_RID = @orgHierarchyRID and hn.HOME_LEVEL = 0

if @debug = 1
begin
   print 'Organizational hierarchy key = ' + convert(varchar, @orgHierarchyRID)
   print 'Number of hierarchy levels = ' + convert(varchar, @noHierarchyLevels)
   print 'Style level = ' + convert(varchar, @styleLevel)
   print 'Color level = ' + convert(varchar, @colorLevel)
end

-- Add Task Lists
-- define folder, tasklists and tasks
declare @currentDate datetime, @taskSequence int, @administratorUserRID int, @systemUserRID int, @globalUserRID int, @errorLevel int, @severeLevel int, @useConfigSettings int
declare @cdrRID int, @rollLevel int

select @currentDate = getdate()
set @administratorUserRID = 2
set @systemUserRID = 3
set @globalUserRID = 4
set @errorLevel = 6
set @severeLevel = 7
set @useConfigSettings = 802873

if @colorLevel > -1 and @rollFromColor = 1
begin
   set @rollLevel = @colorLevel
end
else
begin
   set @rollLevel = @styleLevel
end

-- add daily task list folder and task lists
-- get System Task Lists
select @explorerFolderRID = FOLDER_RID from FOLDER f where f.FOLDER_TYPE = 44 

-- add Daily folder
exec [dbo].[SP_MID_FOLDER_INSERT] @systemUserRID,'Daily',45,@folderRID OUTPUT 
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@explorerFolderRID, @folderRID, 45)
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,45,@folderRID,@systemUserRID)

-- add Alternate Hierarchy Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Alternate Hierarchy Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800401, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Alternate Hierarchy', 'trg', 1, '0', ' ', @useConfigSettings)
set @taskSequence = 1
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800401, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Alternate Hierarchy', 'movtrg', 1, '0', ' ', @useConfigSettings)
set @taskSequence = 2
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800401, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Alternate Hierarchy', 'deltrg', 1, '0', ' ', @useConfigSettings)            

-- add Alternate Hierarchy Reclass task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Alternate Hierarchy Reclass',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800420, @errorLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_PROGRAM(TASKLIST_RID, TASK_SEQUENCE, PROGRAM_PATH, PROGRAM_PARMS) 
      VALUES(@taskListRID, @taskSequence, @installDir + '\Batch\HierarchyReclass.exe', '"' + @dataDir + '\Alternate Reclass" " ' + @dataDir + '\Alternate Hierarchy"')

-- add Color Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Color Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800407, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Color', 'trg', 1, '0', ' ', @useConfigSettings)

-- add Daily History task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Daily History',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
-- Load History
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800403, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Daily History', 'trg', 6, '0', ' ', @useConfigSettings)
-- Sales Ending Date
set @taskSequence = 1
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800403, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Daily History', 'setrg', 1, '0', ' ', @useConfigSettings)

-- add Intransit task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Daily Intransit',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800403, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Daily Intransit', 'trg', 1, '0', ' ', @useConfigSettings)

-- add Daily Intransit Rollup task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Daily Intransit Rollup',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800411, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
exec [dbo].[SP_MID_CALENDARDTRANGE_INSERT] 0,0,800858,800851,0,'','0',0, @cdrRID OUTPUT
INSERT INTO TASK_ROLLUP(TASKLIST_RID, TASK_SEQUENCE, ROLLUP_SEQUENCE, HN_RID, FV_RID, ROLLUP_CDR_RID, FROM_PH_OFFSET_IND, FROM_PH_RID, FROM_PHL_SEQUENCE, FROM_OFFSET, 
					TO_PH_OFFSET_IND, TO_PH_RID, TO_PHL_SEQUENCE, TO_OFFSET, POSTING_IND, HIERARCHY_LEVELS_IND, DAY_TO_WEEK_IND, DAY_IND, WEEK_IND, STORE_IND, CHAIN_IND,
					STORE_TO_CHAIN_IND, INTRANSIT_IND, RECLASS_IND)
      VALUES(@taskListRID, @taskSequence, 0, @HN_RID, @FV_ActualRID, @cdrRID, 2, @orgHierarchyRID, @rollLevel, null,
	         null, null, null, null, 1, 0, 0, 0, 0, 0, 0,
			 0, 1, 0)

-- add Daily Rollup task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Daily Rollup',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800411, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
exec [dbo].[SP_MID_CALENDARDTRANGE_INSERT] 0,0,800858,800851,0,'','0',0, @cdrRID OUTPUT
INSERT INTO TASK_ROLLUP(TASKLIST_RID, TASK_SEQUENCE, ROLLUP_SEQUENCE, HN_RID, FV_RID, ROLLUP_CDR_RID, FROM_PH_OFFSET_IND, FROM_PH_RID, FROM_PHL_SEQUENCE, FROM_OFFSET, 
					TO_PH_OFFSET_IND, TO_PH_RID, TO_PHL_SEQUENCE, TO_OFFSET, POSTING_IND, HIERARCHY_LEVELS_IND, DAY_TO_WEEK_IND, DAY_IND, WEEK_IND, STORE_IND, CHAIN_IND,
					STORE_TO_CHAIN_IND, INTRANSIT_IND, RECLASS_IND)
      VALUES(@taskListRID, @taskSequence, 0, @HN_RID, @FV_ActualRID, @cdrRID, 2, @orgHierarchyRID, @rollLevel, null,
	         2, @orgHierarchyRID, null, null, 0, 1, 0, 1, 0, 1, 0,
			 0, 0, 0)

-- add Generate Relieve Intransit task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Generate Relieve Intransit',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800420, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_PROGRAM(TASKLIST_RID, TASK_SEQUENCE, PROGRAM_PATH, PROGRAM_PARMS) 
      VALUES(@taskListRID, @taskSequence, @installDir + '\Batch\RelieveHeaders.exe', '')

-- add Hierarchy Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Hierarchy Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800401, @errorLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Hierarchy', 'trg', 1, '0', ' ', @useConfigSettings)
set @taskSequence = 1
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800401, @errorLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Hierarchy', 'movtrg', 1, '0', ' ', @useConfigSettings)
set @taskSequence = 2
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800401, @errorLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Hierarchy', 'deltrg', 1, '0', ' ', @useConfigSettings)            

-- add Hierarchy Reclass task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Hierarchy Reclass',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800420, @errorLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_PROGRAM(TASKLIST_RID, TASK_SEQUENCE, PROGRAM_PATH, PROGRAM_PARMS) 
      VALUES(@taskListRID, @taskSequence, @installDir + '\Batch\HierarchyReclass.exe', '')

-- add Relieve Intransit task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Relieve Intransit',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800416, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Hierarchy', 'trg', 1, '0', ' ', @useConfigSettings)

-- add Size Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Size Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800408, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Size', 'trg', 1, '0', ' ', @useConfigSettings)

-- add Store Characteristics Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Store Characteristics Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800402, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Store Attributes', 'trg', 1, '0', ' ', @useConfigSettings)

-- add Store Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Store Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800402, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Stores', 'trg', 1, '0', ' ', @useConfigSettings)


-- add Other folder
exec [dbo].[SP_MID_FOLDER_INSERT] @systemUserRID,'Other',45,@folderRID OUTPUT 
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@explorerFolderRID, @folderRID, 45)
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,45,@folderRID,@systemUserRID)

-- add Size Curves task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Size Curves',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800434, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_SIZE_CURVE_GENERATE(TASKLIST_RID, TASK_SEQUENCE, GENERATE_SEQUENCE, METHOD_NODE_TYPE, HN_RID, METHOD_RID, EXECUTE_CDR_RID) 
      VALUES(@taskListRID, @taskSequence, 0, 2, @HN_RID, null, null)
INSERT INTO TASK_SIZE_CURVE_GENERATE_NODE(TASKLIST_RID, TASK_SEQUENCE, CONCURRENT_PROCESSES) 
	VALUES(@taskListRID, @taskSequence, 5)

-- add Reclass Rollup task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Reclass Rollup',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800411, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_ROLLUP(TASKLIST_RID, TASK_SEQUENCE, ROLLUP_SEQUENCE, HN_RID, FV_RID, ROLLUP_CDR_RID, FROM_PH_OFFSET_IND, FROM_PH_RID, FROM_PHL_SEQUENCE, FROM_OFFSET, 
					TO_PH_OFFSET_IND, TO_PH_RID, TO_PHL_SEQUENCE, TO_OFFSET, POSTING_IND, HIERARCHY_LEVELS_IND, DAY_TO_WEEK_IND, DAY_IND, WEEK_IND, STORE_IND, CHAIN_IND,
					STORE_TO_CHAIN_IND, INTRANSIT_IND, RECLASS_IND)
      VALUES(@taskListRID, @taskSequence, 0, null, null, null, 1, null, null, null, 
					null, null, null, null, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 1)

-- add Header Load task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Header Load',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800409, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Headers', 'trg', 4, '0', ' ', @useConfigSettings)


-- add Header Reconcile task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Header Reconcile',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800452, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_HEADER_RECONCILE(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, OUTPUT_DIRECTORY, TRIGGER_SUFFIX, REMOVE_TRANS_FILE_NAME, REMOVE_TRANS_TRIGGER_SUFFIX, HEADER_TYPES, HEADER_KEYS_FILE_NAME) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Header Reconcile', @dataDir + '\Headers', 'hdrtrg', 'RemoveHeaders', 'remtrg', 'All', @installDir + '\Batch\HeaderKeys.txt')

-- add Header Reconcile - Header Load remove task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Header Reconcile - Load Removes',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800409, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Headers', 'remtrg', 6, '0', ' ', @useConfigSettings)

-- add Header Reclass - Header Load add task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Header Reconcile - Load Updates',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800409, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Headers', 'hdrtrg', 6, '0', ' ', @useConfigSettings)




-- add Weekly folder
exec [dbo].[SP_MID_FOLDER_INSERT] @systemUserRID,'Weekly',45,@folderRID OUTPUT 
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@explorerFolderRID, @folderRID, 45)
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,45,@folderRID,@systemUserRID)

-- add Size Day to Week Summary task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Size Day to Week Summary',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800435, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_SIZE_DAY_TO_WEEK_SUMMARY(TASKLIST_RID, TASK_SEQUENCE, CDR_RID, HN_RID)
	VALUES(@taskListRID, @taskSequence, 1, null)

-- add Weekly History task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Weekly History',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800403, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
INSERT INTO TASK_POSTING(TASKLIST_RID, TASK_SEQUENCE, INPUT_DIRECTORY, FILE_MASK, CONCURRENT_FILES, RUN_UNTIL_FILE_PRESENT_IND, RUN_UNTIL_FILE_MASK, FILE_PROCESSING_DIRECTION) 
      VALUES(@taskListRID, @taskSequence, @dataDir + '\Weekly History', 'trg', 6, '0', ' ', @useConfigSettings)

-- add Weekly Purge task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Weekly Purge',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800417, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)

-- add Weekly Rollup task list and task
exec [dbo].[SP_MID_TASKLIST_INSERT] 'Weekly Rollup',@systemUserRID,0,@systemUserRID,@currentDate,@administratorUserRID,@currentDate, @taskListRID OUTPUT
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,46,@taskListRID,@systemUserRID)
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @taskListRID, 46)
set @taskSequence = 0
INSERT INTO TASK(TASKLIST_RID, TASK_SEQUENCE, TASK_TYPE, MAX_MESSAGE_LEVEL, 
                EMAIL_SUCCESS_FROM, EMAIL_SUCCESS_TO, EMAIL_SUCCESS_CC, EMAIL_SUCCESS_BCC, EMAIL_SUCCESS_SUBJECT, EMAIL_SUCCESS_BODY,
                EMAIL_FAILURE_FROM, EMAIL_FAILURE_TO, EMAIL_FAILURE_CC, EMAIL_FAILURE_BCC, EMAIL_FAILURE_SUBJECT, EMAIL_FAILURE_BODY) 
                VALUES(@taskListRID, @taskSequence, 800411, @severeLevel, 
                null, null, null, null, null, null, 
                null, null, null, null, null, null)
exec [dbo].[SP_MID_CALENDARDTRANGE_INSERT] -1,0,800858,800851,0,'','0',0, @cdrRID OUTPUT
INSERT INTO TASK_ROLLUP(TASKLIST_RID, TASK_SEQUENCE, ROLLUP_SEQUENCE, HN_RID, FV_RID, ROLLUP_CDR_RID, FROM_PH_OFFSET_IND, FROM_PH_RID, FROM_PHL_SEQUENCE, FROM_OFFSET, 
					TO_PH_OFFSET_IND, TO_PH_RID, TO_PHL_SEQUENCE, TO_OFFSET, POSTING_IND, HIERARCHY_LEVELS_IND, DAY_TO_WEEK_IND, DAY_IND, WEEK_IND, STORE_IND, CHAIN_IND,
					STORE_TO_CHAIN_IND, INTRANSIT_IND, RECLASS_IND)
      VALUES(@taskListRID, @taskSequence, 0, @HN_RID, @FV_ActualRID, @cdrRID, 2, @orgHierarchyRID, @rollLevel, null,
	         2, @orgHierarchyRID, null, null, 0, 1, 0, 0, 1, 1, 0,
			 0, 0, 0)
exec [dbo].[SP_MID_CALENDARDTRANGE_INSERT] -1,0,800858,800851,0,'','0',0, @cdrRID OUTPUT
INSERT INTO TASK_ROLLUP(TASKLIST_RID, TASK_SEQUENCE, ROLLUP_SEQUENCE, HN_RID, FV_RID, ROLLUP_CDR_RID, FROM_PH_OFFSET_IND, FROM_PH_RID, FROM_PHL_SEQUENCE, FROM_OFFSET, 
					TO_PH_OFFSET_IND, TO_PH_RID, TO_PHL_SEQUENCE, TO_OFFSET, POSTING_IND, HIERARCHY_LEVELS_IND, DAY_TO_WEEK_IND, DAY_IND, WEEK_IND, STORE_IND, CHAIN_IND,
					STORE_TO_CHAIN_IND, INTRANSIT_IND, RECLASS_IND)
      VALUES(@taskListRID, @taskSequence, 1, @HN_RID, @FV_ActualRID, @cdrRID, 2, @orgHierarchyRID, @rollLevel, null,
	         2, @orgHierarchyRID, @rollLevel, null, 0, 0, 0, 0, 0, 0, 0,
			 1, 0, 0)
exec [dbo].[SP_MID_CALENDARDTRANGE_INSERT] -1,0,800858,800851,0,'','0',0, @cdrRID OUTPUT
INSERT INTO TASK_ROLLUP(TASKLIST_RID, TASK_SEQUENCE, ROLLUP_SEQUENCE, HN_RID, FV_RID, ROLLUP_CDR_RID, FROM_PH_OFFSET_IND, FROM_PH_RID, FROM_PHL_SEQUENCE, FROM_OFFSET, 
					TO_PH_OFFSET_IND, TO_PH_RID, TO_PHL_SEQUENCE, TO_OFFSET, POSTING_IND, HIERARCHY_LEVELS_IND, DAY_TO_WEEK_IND, DAY_IND, WEEK_IND, STORE_IND, CHAIN_IND,
					STORE_TO_CHAIN_IND, INTRANSIT_IND, RECLASS_IND)
      VALUES(@taskListRID, @taskSequence, 2, @HN_RID, @FV_ActualRID, @cdrRID, 2, @orgHierarchyRID, @rollLevel, null,
	         2, @orgHierarchyRID, null, null, 0, 1, 0, 0, 1, 0, 1,
			 0, 0, 0)


-- Add Jobs
declare @jobRID int
-- get System Job
select @explorerFolderRID = FOLDER_RID from FOLDER f where f.FOLDER_TYPE = 47 

-- add Daily job folder
exec [dbo].[SP_MID_FOLDER_INSERT] @systemUserRID,'Daily',45,@folderRID OUTPUT 
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@explorerFolderRID, @folderRID, 45)
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,45,@folderRID,@systemUserRID)

-- add Daily Hierarchy job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Daily Hierarchy','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Hierarchy Reclass'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Hierarchy Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 1)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Alternate Hierarchy Reclass'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 2)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Alternate Hierarchy Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 3)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Reclass Rollup'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 4)

-- add Daily History job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Daily History','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Daily History'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Daily Intransit'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 1)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Daily Rollup'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 2)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Daily Intransit Rollup'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 3)

-- add Daily Master Data job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Daily Master Data','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Color Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Size Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 1)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Store Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 2)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Store Characteristics Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 3)

-- add Daily Relieve Intransit job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Daily Relieve Intransit','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Generate Relieve Intransit'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Relieve Intransit'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 1)


-- add Other job folder
exec [dbo].[SP_MID_FOLDER_INSERT] @systemUserRID,'Other',45,@folderRID OUTPUT 
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@explorerFolderRID, @folderRID, 45)
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,45,@folderRID,@systemUserRID)

-- add Size Curves job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Size Curves','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Size Curves'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)

-- add Header Load job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Header Load','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Header Load'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)


-- add Header Reconcile job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Header Reconcile and Load','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Header Reconcile'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Header Reconcile - Load Removes'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 1)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Header Reconcile - Load Updates'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 2)


-- add Weekly job folder
exec [dbo].[SP_MID_FOLDER_INSERT] @systemUserRID,'Weekly',45,@folderRID OUTPUT 
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@explorerFolderRID, @folderRID, 45)
insert into USER_ITEM(USER_RID, ITEM_TYPE, ITEM_RID, OWNER_USER_RID) values (@systemUserRID,45,@folderRID,@systemUserRID)

-- add Size Day to Week Summary job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Size Day to Week Summary','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Size Day to Week Summary'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)

-- add Weekly History job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Weekly History','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Weekly History'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Weekly Rollup'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 1)

-- add Weekly Purge job and task lists
exec [dbo].[SP_MID_JOB_INSERT] 'Weekly Purge','0',@administratorUserRID,@currentDate,@administratorUserRID,@currentDate, @jobRID OUTPUT
insert into FOLDER_JOIN(PARENT_FOLDER_RID, CHILD_ITEM_RID, CHILD_ITEM_TYPE) values (@folderRID, @jobRID, 48)
select @taskListRID = TASKLIST_RID from TASKLIST t where t.TASKLIST_NAME = 'Weekly Purge'
INSERT INTO JOB_TASKLIST_JOIN(JOB_RID, TASKLIST_RID, TASKLIST_SEQUENCE)
	VALUES(@jobRID, @taskListRID, 0)
