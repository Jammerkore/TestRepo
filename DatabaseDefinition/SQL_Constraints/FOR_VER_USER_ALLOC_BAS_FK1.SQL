alter table "USER_ALLOCATION_BASIS"
	add constraint "FOR_VER_USER_ALLOC_BAS_FK1" foreign key (
		"BASIS_FV_RID")
	 references "FORECAST_VERSION" (
		"FV_RID")  
go
