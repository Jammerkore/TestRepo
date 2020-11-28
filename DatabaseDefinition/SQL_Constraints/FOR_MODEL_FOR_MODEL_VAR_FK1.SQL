/* Add foreign key constraints to table "FORECAST_MODEL_VARIABLE".                            */
alter table "FORECAST_MODEL_VARIABLE"
	add constraint "FOR_MODEL_FOR_MODEL_VAR_FK1" foreign key (
		"FORECAST_MOD_RID")
	 references "FORECAST_MODEL" (
		"FORECAST_MOD_RID")  

go
