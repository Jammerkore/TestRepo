alter table "NODE_SIZE_OUTOFSTOCK_GRPLVL"
	add constraint "ND_SZ_OOS_GRPLVL_FK1" foreign key ("HN_RID")
	 references "NODE_SIZE_OUTOFSTOCK" ("HN_RID")  

go
