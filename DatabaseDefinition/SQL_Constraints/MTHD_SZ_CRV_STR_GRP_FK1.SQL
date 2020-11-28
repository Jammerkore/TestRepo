--Begin TT#1076 - JScott - Size Curves by Set
alter table "METHOD_SIZE_CURVE"
	add constraint "MTHD_SZ_CRV_STR_GRP_FK1" foreign key ("SIZE_CURVES_BY_SG_RID")
	 references "STORE_GROUP" ("SG_RID")  

go
