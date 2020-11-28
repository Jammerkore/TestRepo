/* Add foreign key constraints to table "METHOD_VELOCITY_SELL_THRU".                          */
alter table "METHOD_VELOCITY_SELL_THRU"
	add constraint "MT_VEL_MT_VEL_SL_THRU_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_VELOCITY" (
		"METHOD_RID")  

go
