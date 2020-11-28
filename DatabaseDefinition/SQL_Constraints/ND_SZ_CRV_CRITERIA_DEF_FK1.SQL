--End TT#1076 - JScott - Size Curves by Set
alter table "NODE_SIZE_CURVE_CRITERIA_DEFAULT" 
	add constraint "ND_SZ_CRV_CRITERIA_DEF_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
