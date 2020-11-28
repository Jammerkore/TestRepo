/*RELEASE 5.2.0.39 07-31-2013 - TT#3219 -JSmith - Issue during Header purge */
declare @t table (WORKFLOW_RID int)   
insert into @t  select WORKFLOW_RID from WORKFLOW w  where w.WORKFLOW_NAME like '%(generated)%'    and w.WORKFLOW_RID  not in (select distinct API_WORKFLOW_RID from HEADER where API_WORKFLOW_RID is not null) 
select count(*) from ALLOCATION_STEP_COMPONENT where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
delete ALLOCATION_STEP_COMPONENT where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
select count(*) from ALLOCATION_STEP_COMPONENT where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
go 


declare @t table (WORKFLOW_RID int)   
insert into @t  select WORKFLOW_RID from WORKFLOW w  where w.WORKFLOW_NAME like '%(generated)%'    and w.WORKFLOW_RID  not in (select distinct API_WORKFLOW_RID from HEADER where API_WORKFLOW_RID is not null) 
select count(*) from WORKFLOW_STEP_ALLOCATION where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
delete WORKFLOW_STEP_ALLOCATION where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
select count(*) from WORKFLOW_STEP_ALLOCATION where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
go 


declare @t table (WORKFLOW_RID int)   
insert into @t  select WORKFLOW_RID from WORKFLOW w  where w.WORKFLOW_NAME like '%(generated)%'    and w.WORKFLOW_RID  not in (select distinct API_WORKFLOW_RID from HEADER where API_WORKFLOW_RID is not null) 
select count(*) from WORKFLOW_STEP_OTSPLAN where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
delete WORKFLOW_STEP_OTSPLAN where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
select count(*) from WORKFLOW_STEP_OTSPLAN where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
go 


declare @t table (WORKFLOW_RID int)   
insert into @t  select WORKFLOW_RID from WORKFLOW w  where w.WORKFLOW_NAME like '%(generated)%'    and w.WORKFLOW_RID  not in (select distinct API_WORKFLOW_RID from HEADER where API_WORKFLOW_RID is not null) 
select count(*) from WORKFLOW_CHANGE_HISTORY where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
delete WORKFLOW_CHANGE_HISTORY where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
select count(*) from WORKFLOW_CHANGE_HISTORY where WORKFLOW_RID in (select WORKFLOW_RID from @t)   
go 

-- note once you run this one you lose the list to run against 
--declare @t table (WORKFLOW_RID int)   

create table #etemp(WORKFLOW_RID int) 
insert into #etemp  select WORKFLOW_RID from WORKFLOW w (nolock)  where w.WORKFLOW_NAME like '%(generated)%'    and w.WORKFLOW_RID  not in (select distinct API_WORKFLOW_RID from HEADER where API_WORKFLOW_RID is not null) 
select count(*) from WORKFLOW (nolock) where WORKFLOW_RID in (select WORKFLOW_RID from #etemp) 
update HEADER set WORKFLOW_RID = 1 where WORKFLOW_RID in (select WORKFLOW_RID from #etemp)
delete WORKFLOW where WORKFLOW_RID in (select WORKFLOW_RID from #etemp) 
select count(*) from WORKFLOW (nolock) where WORKFLOW_RID in (select WORKFLOW_RID from #etemp) 
drop table #etemp 
go 
