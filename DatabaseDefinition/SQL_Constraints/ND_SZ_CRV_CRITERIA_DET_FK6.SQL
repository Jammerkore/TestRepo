--Begin TT#1076 - JScott - Size Curves by Set
alter table "NODE_SIZE_CURVE_CRITERIA_DETAIL" 
	add constraint "ND_SZ_CRV_CRITERIA_DET_FK6" foreign key ("SG_RID")
	 references "STORE_GROUP" ("SG_RID")  

go
