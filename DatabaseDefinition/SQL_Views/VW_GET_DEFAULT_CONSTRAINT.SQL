CREATE VIEW [dbo].[VW_GET_DEFAULT_CONSTRAINT]
as
select	db_name()				as CONSTRAINT_CATALOG
	,t_obj.name 				as TABLE_NAME
	,user_name(c_obj.uid)			as CONSTRAINT_SCHEMA
	,c_obj.name				as CONSTRAINT_NAME
	,col.name				as COLUMN_NAME
	,col.colid				as ORDINAL_POSITION
	,com.text				as DEFAULT_CLAUSE

from	sysobjects	c_obj
join 	syscomments	com on 	c_obj.id = com.id
join 	sysobjects	t_obj on c_obj.parent_obj = t_obj.id  
join    sysconstraints con on c_obj.id	= con.constid
join 	syscolumns	col on t_obj.id = col.id
			and con.colid = col.colid
where
	c_obj.uid	= user_id()
	and c_obj.xtype	= 'D'

GO
