alter table "NODE_SIZE_OUTOFSTOCK_QUANTITY"
	add constraint "ND_SZ_OOS_QUANTITY_FK1" foreign key ("HN_RID")
	 references "NODE_SIZE_OUTOFSTOCK" ("HN_RID")  

go
