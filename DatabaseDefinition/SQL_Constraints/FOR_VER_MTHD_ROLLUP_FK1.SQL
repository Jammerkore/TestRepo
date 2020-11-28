alter table "METHOD_ROLLUP"
	add constraint "FOR_VER_MTHD_ROLLUP_FK1" foreign key ("FV_RID")
	 references "FORECAST_VERSION" ("FV_RID") on update no action on delete no action  

go
