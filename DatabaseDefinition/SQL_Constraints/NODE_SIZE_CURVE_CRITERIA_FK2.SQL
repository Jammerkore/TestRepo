alter table "NODE_SIZE_CURVE_CRITERIA" 
	add constraint "NODE_SIZE_CURVE_CRITERIA_FK2" foreign key ("NSCCD_RID")
	 references "NODE_SIZE_CURVE_CRITERIA_DETAIL" ("NSCCD_RID")  

go
